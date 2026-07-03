using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Killing;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class CommanderCommandButton : TownOfUsRoleButton<CommanderRole, PlayerControl>
{
    public override string Name => "Command";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.CommanderRoleColour;
    public override float Cooldown => OptionGroupSingleton<CommanderRoleOptions>.Instance.CommandCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.CommanderCommandButton;
    public override int MaxUses => (int)OptionGroupSingleton<CommanderRoleOptions>.Instance.CommandUses;

    public override PlayerControl GetTarget()
    {
        if (OptionGroupSingleton<LoversOptions>.Instance.BothLoversDie && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
                x => !x.IsLover() && !x.HasModifier<BrawlerModifier>());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, predicate:x => !x.HasModifier<BrawlerModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        Target.RpcAddModifier<BrawlerModifier>();
    }
}