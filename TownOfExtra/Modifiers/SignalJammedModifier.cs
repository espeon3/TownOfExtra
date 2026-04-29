using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public class SignalJammedModifier : TimedModifier
{
    public override string ModifierName => "Jammed";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<SignalJammerRoleOptions>.Instance.JamDuration;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.SignalJammerRoleIcon;

    public override string GetDescription()
    {
        return $"Your signals are jammed for {TimeRemaining:F1}s!";
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<SignalJammedModifier>();
    }
}