using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfExtra.Roles.Impostor.Concealing;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class CannibalKillButton : TownOfUsKillRoleButton<CannibalRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Cannibalise";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.CannibalRoleColour;
    public override float Cooldown => OptionGroupSingleton<CannibalRoleOptions>.Instance.KillCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.Placeholder;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(
            true,
            Distance,
            predicate: plr =>
                plr != null &&
                plr != PlayerControl.LocalPlayer &&
                !plr.HasDied()
                );
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target,
            createDeadBody: false,
            causeOfDeath: "Cannibalised"
        );
    }
}