using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Support;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ObstructorObstructButton : TownOfUsRoleButton<ObstructorRole, PlayerControl>, IAftermathablePlayerButton
{
    public override string Name => "Obstruct";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<ObstructorRoleOptions>.Instance.ObstructCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ObstructorObstructButton;
    public override bool ShouldPauseInVent => false;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    public void AftermathHandler()
    {
        PlayerControl.LocalPlayer.RpcAddModifier<ObstructorObstructedModifier>(PlayerControl.LocalPlayer.PlayerId);
    }
    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"The next time {Target.Data.PlayerName} attempts to use any ability, their abilities {Palette.ImpostorRed.ToTextColor()}will be disabled</color> for {OptionGroupSingleton<ObstructorRoleOptions>.Instance.ObstructDuration}s!",
            "ObstructorObstructButton",
            "ImpButton"
        );

        Target.RpcAddModifier<ObstructorObstructedModifier>(PlayerControl.LocalPlayer.PlayerId);
    }
}