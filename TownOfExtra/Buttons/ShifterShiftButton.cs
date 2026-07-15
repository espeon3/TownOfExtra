using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ShifterShiftButton : TownOfUsRoleButton<ShifterRole, PlayerControl>
{
    public override string Name => "Shift";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.ShifterRoleColour;
    public override float Cooldown => OptionGroupSingleton<ShifterRoleOptions>.Instance.ShiftCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ShifterShiftButton;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, predicate:
            x => !x.HasModifier<PreviouslyShiftedModifier>() && !x.HasModifier<WaitingOnShiftModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        Target.RpcAddModifier<WaitingOnShiftModifier>();
        PlayerControl.LocalPlayer.RpcSendNotification(
            $"Your role will be {TownOfExtraColours.ShifterRoleColour.ToTextColor()}shifted</color> with {Target.name} after the next meeting!",
            "ShifterRoleIcon",
            "NeutRoleIcon",
            flashColour: TownOfExtraColours.ShifterRoleColour
        );
    }
}