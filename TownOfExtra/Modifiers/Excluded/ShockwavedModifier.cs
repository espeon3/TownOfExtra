using BepInEx.Unity.IL2CPP.Utils.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Options;
using TownOfUs.Modifiers;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class ShockwavedModifier : BaseRevealModifier
{
    public override string ModifierName => "Shockwaved";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveEffectDuration.Value;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ShockwaveModifierIcon;

    public float NormalSpeed;

    public override string GetDescription()
    {
        return $"You have been shockwaved!\n<b>Time Remaining: {TimeRemaining:F1}s</b>";
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;

        var clr = TownOfExtraColours.ShockwaveModifierColour.ToTextColor();
        Player.RpcSendNotification(
            $"You have been {clr}shockwaved</color>! Your {clr}vision & speed</color> has been reduced for {Duration}s.",
            "ShockwaveShockwaveButton",
            "ImpModButton",
            flashColour: TownOfExtraColours.ShockwaveModifierColour
        );
        
        NormalSpeed = Player.MyPhysics.Speed;
        Player.MyPhysics.Speed *= OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveSpeedDebuffMultiplier.Value;
        
        Coroutines.Start(
            Effects.Shake(HudManager.Instance.PlayerCam.transform, 1f, 0.08f, true, true).WrapToManaged());
    }

    public override void OnDeactivate()
    {
        ExtraNameText = "";
        if (!Player.AmOwner) return;
        Player.MyPhysics.Speed = NormalSpeed;
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (MeetingHud.Instance) return;

        ExtraNameText = TimerActive 
            ? $"<br><size=70%>{TownOfExtraColours.ShockwaveModifierColour.ToTextColor()}Shockwaved: {TimeRemaining:F1}s</color></size>"
            : "";
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ShockwavedModifier>();
    }
}