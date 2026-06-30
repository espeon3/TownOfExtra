using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class BarbarianTargetButton : TownOfUsRoleButton<BarbarianRole, PlayerControl>
{
    public override string Name => "Target";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.BarbarianRoleColour;
    public override float Cooldown => OptionGroupSingleton<BarbarianRoleOptions>.Instance.TargetCooldown;
    public override float EffectDuration => OptionGroupSingleton<BarbarianRoleOptions>.Instance.TargetDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.BarbarianTargetButton;
    
    protected override void OnClick()
    {
        if (Target == null) return;

        Target.RpcAddModifier<BarbTargetModifier>();
    }

    public override PlayerControl GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }
}