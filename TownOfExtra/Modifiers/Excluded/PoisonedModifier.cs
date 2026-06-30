using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public sealed class PoisonedModifier(PlayerControl poisoner) : TimedModifier
{
    public override string ModifierName => "Poisoned";
    public override float Duration => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonLength;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.PoisonedModifierIcon;

    public override string GetDescription()
    {
        return $"Time until death: {TimeRemaining:F1}s";
    }

    public override void OnActivate()
    {
        if (Player != PlayerControl.LocalPlayer) return;
        
        Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.PoisonColour, Duration));
        var notif = Helpers.CreateAndShowNotification(
            $"You have been {TownOfExtraColours.PoisonColour.ToTextColor()}poisoned</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonedModifierIcon.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnTimerComplete()
    {
        poisoner.RpcSpecialMurder(
            Player,
            isIndirect: true,
            ignoreShield: true,
            teleportMurderer: false,
            showKillAnim: false,
            playKillSound: true,
            causeOfDeath: "Poisoned"
        );

        if (Player == PlayerControl.LocalPlayer)
        {
            var notif = Helpers.CreateAndShowNotification(
                $"You have been {TownOfExtraColours.PoisonColour.ToTextColor()}poisoned</color> to {Palette.ImpostorRed.ToTextColor()}death</color>!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonedModifierIcon.LoadAsset());
            notif.AdjustNotification();
        }

        if (poisoner == PlayerControl.LocalPlayer)
        {
            var poisonerNotif = Helpers.CreateAndShowNotification(
                $"Your {TownOfExtraColours.PoisonColour.ToTextColor()}poison</color> has {Palette.ImpostorRed.ToTextColor()}killed</color> {Player.Data.PlayerName}!",
                Color.white, new Vector3(0f, 1.8f, -20f), spr: TownOfExtraAssets.PoisonedModifierIcon.LoadAsset());
            poisonerNotif.AdjustNotification();
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<PoisonedModifier>();
    }
}