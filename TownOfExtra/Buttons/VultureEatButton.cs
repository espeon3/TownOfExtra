using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class VultureEatButton : TownOfUsRoleButton<VultureRole, DeadBody>
{
    public override string Name => "Eat";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.VultureRoleColour;
    public override float Cooldown => OptionGroupSingleton<VultureRoleOptions>.Instance.EatCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.VultureEatButton;

    public override DeadBody GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestDeadBody(Distance);
    }

    protected override void OnClick()
    {
        var p = PlayerControl.LocalPlayer;
        if (p == null || Target == null) return;

        VultureRpcs.RpcCleanBody(p, Target.ParentId);
        VultureRpcs.RpcIncrementBodyCount(1);

        var notif = Helpers.CreateAndShowNotification(
            $"You have {TownOfExtraColours.VultureRoleColour.ToTextColor()}eaten</color> a body and are now at {TownOfUsColors.Neutral.ToTextColor()}{VultureRole.DeadBodiesEaten}/{OptionGroupSingleton<VultureRoleOptions>.Instance.EatenBodiesNeeded}</color> bodies!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VultureEatButton.LoadAsset());
        notif.AdjustNotification();
        
        VultureRpcs.RpcCheckWin(PlayerControl.LocalPlayer);
    }
}