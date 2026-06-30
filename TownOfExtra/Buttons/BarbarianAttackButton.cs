using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class BarbarianAttackButton : TownOfUsKillRoleButton<BarbarianRole, PlayerControl>, IKillButton
{
    public override string Name => "Attack";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.BarbarianRoleColour;
    public override float Cooldown => 0f;
    public override bool ZeroIsInfinite => false;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.BarbarianAttackButton;

    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSpecialMurder(Target, MeetingCheck.OutsideMeeting, causeOfDeath: "Slain");
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