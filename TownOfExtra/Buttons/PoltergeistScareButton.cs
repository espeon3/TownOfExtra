using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class PoltergeistScareButton : TownOfUsRoleButton<PoltergeistRole, PlayerControl>
{
    public override string Name => "Scare";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.PoltergeistRoleColour;
    public override float Cooldown => OptionGroupSingleton<PoltergeistRoleOptions>.Instance.ScareCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PoltergeistScareButton;

    public override PlayerControl GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
                x => !x.IsLover() && !x.HasModifier<ScaredModifier>() && !x.HasModifier<PossessedModifier>());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
            x => !x.HasModifier<ScaredModifier>() && !x.HasModifier<PossessedModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null || Target.HasModifier<ScaredModifier>() || Target.HasModifier<PossessedModifier>()) return;
        
        Target.RpcAddModifier<ScaredModifier>();
    }
}