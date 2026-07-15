using BepInEx.Unity.IL2CPP.Utils.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfUs;
using TownOfUs.Modifiers;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class SlippedModifier : BaseRevealModifier
{
    public override string ModifierName => "Slipped";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<SquidRoleOptions>.Instance.DebuffDurations;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.SquidSpillButton;

    public float NormalSpeed;

    public override string GetDescription()
    {
        return $"You have slipped in some ink!\n<b>Time Remaining: {TimeRemaining:F1}s</b>";
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;

        var clr = TownOfExtraColours.SquidRoleColour.ToTextColor();
        Player.RpcSendNotification(
            $"You have {clr}slipped</color> in some ink! Your {clr}vision & speed</color> has been reduced, and you cannot {clr}use any abilities</color> for {Duration}s.",
            "SquidInkPuddle",
            "NeutMisc",
            225,
            TownOfExtraColours.SquidRoleColour
        );
        
        NormalSpeed = Player.MyPhysics.Speed;
        Player.MyPhysics.Speed *= OptionGroupSingleton<SquidRoleOptions>.Instance.SpeedDebuffMultiplier;
        
        Coroutines.Start(
            Effects.Shake(HudManager.Instance.PlayerCam.transform, 1f, 0.08f, true, true).WrapToManaged());
    }

    public override void OnDeactivate()
    {
        ExtraNameText = "";
        if (!Player.AmOwner) return;
        
        Coroutines.Start(
            Effects.Shake(HudManager.Instance.PlayerCam.transform, 1f, 0.08f, true, true).WrapToManaged());
        
        Player.RpcSendNotification(
            $"You have {TownOfUsColors.Medic.ToTextColor()}recovered</color> from your {TownOfExtraColours.SquidRoleColour.ToTextColor()}slip</color>!",
            "SquidInkPuddle",
            "NeutMisc",
            225,
            TownOfExtraColours.SquidRoleColour
        );
        
        Player.MyPhysics.Speed = NormalSpeed;
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (MeetingHud.Instance) return;

        ExtraNameText = TimerActive 
            ? $"<br><size=70%>{TownOfExtraColours.SquidRoleColour.ToTextColor()}Slipped: {TimeRemaining:F1}s</color></size>"
            : "";
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<SlippedModifier>();
    }

    public override void OnMeetingStart()
    {
        Player.RemoveModifier(this);
    }
}