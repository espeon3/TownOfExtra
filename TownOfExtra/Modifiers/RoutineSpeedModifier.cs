using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public class RoutineSpeedModifier : TimedModifier
{
    public override string ModifierName => "Speed Boost";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineSpeedBoostDuration.Value;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.LightningIcon;
    
    public float NormalSpeed;

    public override string GetDescription()
    {
        return $"You have a speed boost for {TimeRemaining:F1}s!";
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;
        
        NormalSpeed = Player.MyPhysics.Speed;
        Player.MyPhysics.Speed = NormalSpeed * OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineSpeedBoost.Value;
        
        var notif = Helpers.CreateAndShowNotification(
            $"You have gained a {Palette.CrewmateBlue.ToTextColor()}speed boost</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.LightningIcon.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnDeactivate()
    {
        if (!Player.AmOwner) return;
        Player.MyPhysics.Speed = NormalSpeed;

        var notif = Helpers.CreateAndShowNotification(
            $"You have lost your {Palette.CrewmateBlue.ToTextColor()}speed boost</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.LightningIcon.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<RoutineSpeedModifier>();
    }
}