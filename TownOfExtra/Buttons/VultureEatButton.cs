using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Buttons;
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
    }
}