using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class SerialKillerKillButton : TownOfUsKillRoleButton<SerialKillerRole, PlayerControl>, IDiseaseableButton, IKillButton
{
    public override string Name => "Kill";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => Cooldown - OptionGroupSingleton<SerialKillerRoleOptions>.Instance.LowerKillCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PhAttack;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSpecialMurder(Target, MeetingCheck.OutsideMeeting);
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