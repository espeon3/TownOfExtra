using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class SwitcherSwitchButton : TownOfUsKillRoleButton<SwitcherRole, PlayerControl>, IKillButton
{
    public override string Name => "Switch";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.SwitcherRoleColour;
    public override float Cooldown => OptionGroupSingleton<SwitcherRoleOptions>.Instance.SwitchCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.SwitcherSwitchButton;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            p.RpcRemoveModifier<SwitchedModifier>();
        }
        
        Target.RpcAddModifier<SwitchedModifier>();

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"Your role will be {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switched</color> with {Target.name} after the next meeting!",
            "SwitcherRoleIcon",
            "NeutRoleIcon",
            356,
            TownOfExtraColours.SwitcherRoleColour
        );
    }
}