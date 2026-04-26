using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Options;
using TownOfExtra.Roles;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class PoisonerPoisonButton : TownOfUsKillRoleButton<PoisonerRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Poison";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.PoisonerRoleColour;
    public override float Cooldown => OptionGroupSingleton<PoisonerRoleOptions>.Instance.PoisonCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PoisonerRoleIcon;

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
                !plr.HasDied() &&
                !plr.HasModifier<PoisonedModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        Target.RpcAddModifier<PoisonedModifier>(PlayerControl.LocalPlayer);
        SetTimer(Cooldown);

        var notif = Helpers.CreateAndShowNotification(
            $"You poisoned {TownOfExtraColours.PoisonerRoleColour.ToTextColor()}{Target.Data.PlayerName}</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.PoisonerRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
}