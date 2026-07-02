using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfUs.Modifiers;
using TownOfUs.Networking;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public sealed class ImpendingDoomModifier(PlayerControl initiator, bool selfKill) : BaseRevealModifier
{
    public override string ModifierName => "Impending Doom";
    public override float Duration => OptionGroupSingleton<StrikerRoleOptions>.Instance.ImpendingDoomDuration;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.StrikerStrikeButton;
    public override bool HideOnUi => !OptionGroupSingleton<StrikerRoleOptions>.Instance.AnnounceDoomed;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;

    public override string GetDescription()
    {
        var msg = OptionGroupSingleton<StrikerRoleOptions>.Instance.ShowDoomedTimer ? $"Time until death: {TimeRemaining:F1}s" : "";
        return msg;
    }

    public override void OnActivate()
    {
        if (Player != PlayerControl.LocalPlayer) return;
        
        Player.RpcSendNotification(
            $"You have been locked on to by a {Palette.ImpostorRed.ToTextColor()}striker</color>! Your doom is inevitable.",
            "StrikerStrikeButton",
            "ImpButton",
            flashColour: Palette.ImpostorRed);
    }
    
    public override void OnDeactivate()
    {
        ExtraNameText = "";
    }

    public override void OnTimerComplete()
    {
        initiator.RpcSpecialMurder(
            Player,
            MeetingCheck.OutsideMeeting,
            true,
            true,
            teleportMurderer: false,
            resetKillTimer: false,
            showKillAnim: false,
            playKillSound: false,
            causeOfDeath: !selfKill ? "Struck" : "Miscalculated");
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (MeetingHud.Instance) return;

        ExtraNameText = TimerActive && OptionGroupSingleton<StrikerRoleOptions>.Instance.ShowDoomedTimer
            ? $"<br><size=70%>{Palette.ImpostorRed.ToTextColor()}Impending Doom: {TimeRemaining:F1}s</color></size>"
            : "";
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ImpendingDoomModifier>();
    }
}