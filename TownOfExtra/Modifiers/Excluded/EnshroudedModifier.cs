using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfExtra.Buttons;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Options;
using TownOfUs.Patches;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

// CREDITS TO ATONY/TOU MIRA FOR SWOOPER MODIFIER CODE

public sealed class EnshroudedModifier : ConcealedModifier, IVisualAppearance
{
    public override string ModifierName => "Enshrouded";
    public override float Duration => OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.EnshroudDuration;
    public override bool HideOnUi => true;
    public override bool AutoStart => true;
    public override bool VisibleToOthers => false;
    public bool VisualPriority => true;
    
    public float NormalSpeed;

    public VisualAppearance GetVisualAppearance()
    {
        var playerColor = 
            PlayerControl.LocalPlayer.Data.Role is ShadowWalkerRole ||
            (PlayerControl.LocalPlayer.DiedOtherRound() &&
            OptionGroupSingleton<GeneralOptions>.Instance.TheDeadKnow)
            ? new Color(0f, 0f, 0f, 0.1f)
            : Color.clear;

        return new VisualAppearance(Player.GetDefaultModifiedAppearance(), TownOfUsAppearances.Swooper)
        {
            HatId = "hat_NoHat",
            SkinId = "skin_None",
            VisorId = "visor_EmptyVisor",
            PlayerName = string.Empty,
            PetId = "pet_EmptyPet",
            RendererColor = playerColor,
            NameColor = Color.clear,
            ColorBlindTextColor = Color.clear
        };
    }

    public override void OnDeath(DeathReason reason)
    {
        Player.RemoveModifier(this);
    }

    public override void OnMeetingStart()
    {
        Player.RemoveModifier(this);
    }

    public override void OnActivate()
    {
        if (Player.AmOwner)
        {
            TouAudio.PlaySound(TouAudio.SwooperActivateSound);

            var button = CustomButtonSingleton<ShadowWalkerEnshroudButton>.Instance;
            button.OverrideName("Enshrouding...");
            ShadowWalkerRole.Enshrouded = true;
            
             NormalSpeed = Player.MyPhysics.Speed;
             Player.MyPhysics.Speed = NormalSpeed * OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.EnshroudSpeedMultiplier;
        }

        Player.RawSetAppearance(this);
        Player.cosmetics.ToggleNameVisible(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (VanillaSystemCheckPatches.ShroomSabotageSystem && VanillaSystemCheckPatches.ShroomSabotageSystem.IsActive)
        {
            Player.RawSetAppearance(this);
            Player.cosmetics.ToggleNameVisible(false);
        }
    }

    public override void OnDeactivate()
    {
        Player.ResetAppearance();
        Player.cosmetics.ToggleNameVisible(true);

        if (Player.AmOwner)
        {
            var button = CustomButtonSingleton<ShadowWalkerEnshroudButton>.Instance;
            button.OverrideName("Enshroud");
            button.ResetCooldownAndOrEffect();
            ShadowWalkerRole.Enshrouded = false;
            if (!MeetingHud.Instance)
            {
                TouAudio.PlaySound(TouAudio.SwooperDeactivateSound);
            }
            
            Player.MyPhysics.Speed = NormalSpeed;
        }

        if (HudManagerPatches.CamouflageCommsEnabled)
        {
            Player.cosmetics.ToggleNameVisible(false);
        }

        if (VanillaSystemCheckPatches.ShroomSabotageSystem && VanillaSystemCheckPatches.ShroomSabotageSystem.IsActive)
        {
            MushroomMixUp(VanillaSystemCheckPatches.ShroomSabotageSystem, Player);
        }
    }

    public static void MushroomMixUp(MushroomMixupSabotageSystem instance, PlayerControl player)
    {
        if (player != null && !player.Data.IsDead && instance.currentMixups.ContainsKey(player.PlayerId))
        {
            var condensedOutfit = instance.currentMixups[player.PlayerId];
            var playerOutfit = instance.ConvertToPlayerOutfit(condensedOutfit);
            playerOutfit.NamePlateId = player.Data.DefaultOutfit.NamePlateId;

            player.MixUpOutfit(playerOutfit);
        }
    }
}