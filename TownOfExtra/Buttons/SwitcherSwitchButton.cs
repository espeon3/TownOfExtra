using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Modifiers;
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
    public override float Cooldown => 0f;
    public override bool Disabled => ButtonDisabled;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.SwitcherSwitchButton;

    public static bool ButtonDisabled;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        Target.RpcAddModifier<SwitchedModifier>();
        Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.SwitcherRoleColour));
        var notif = Helpers.CreateAndShowNotification(
            $"Your role will be {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switched</color> with {Target.name} after the next meeting!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SwitcherRoleIcon.LoadAsset());
        notif.AdjustNotification();
        
        ButtonDisabled = true;
    }
}