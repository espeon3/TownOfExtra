using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Events.Custom;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class BarbarianTargetModifier : TimedModifier
{
    public override string ModifierName => "Barbarian's Target";
    public override bool HideOnUi => true;
    public override float Duration => OptionGroupSingleton<BarbarianRoleOptions>.Instance.TargetDuration;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.FreezerRoleIcon;

    public override string GetDescription()
    {
        return
            $"You are the barbarian's target!\n" + 
            $"<b>Time Remaining: {TimeRemaining:F1}s</b>";
    }
}