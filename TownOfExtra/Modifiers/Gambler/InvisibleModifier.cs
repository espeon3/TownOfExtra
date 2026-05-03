using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Modifiers.Gambler;

public class InvisibleModifier : TimedModifier, IVisualAppearance
{
    public override string ModifierName => "Invisible";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<GamblerRoleOptions>.Instance.InvisibilityDuration.Value;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public bool VisualPriority => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.GamblerRoleIcon;

    public override string GetDescription()
    {
        return $"You are invisible for {TimeRemaining:F1} seconds!";
    }
    
    public VisualAppearance GetVisualAppearance()
    {
        var playerColor = PlayerControl.LocalPlayer.IsImpostorAligned()
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

    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
        Player.cosmetics.ToggleNameVisible(false);
    }

    public override void OnDeactivate()
    {
        Player.ResetAppearance();
        Player.cosmetics.ToggleNameVisible(true);
    }

    public override void OnDeath(DeathReason reason)
    {
        Player.RpcRemoveModifier<InvisibleModifier>();
    }

    public override void OnMeetingStart()
    {
        Player.RemoveModifier(this);
    }
}