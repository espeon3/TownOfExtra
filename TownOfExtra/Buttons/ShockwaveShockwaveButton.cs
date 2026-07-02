using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Modifiers.Game.Impostor.Utility;
using TownOfExtra.Networking;
using TownOfExtra.Options;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class DisperseButton : TownOfUsButton
{
    public override string Name => "Shockwave";
    public override BaseKeybind Keybind => Keybinds.ModifierAction;
    public override Color TextOutlineColor => TownOfExtraColours.ShockwaveModifierColour;
    public override float Cooldown => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveCooldown.Value;
    public override int MaxUses => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveUses;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ShockwaveShockwaveButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return PlayerControl.LocalPlayer &&
               PlayerControl.LocalPlayer.HasModifier<ShockwaveModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    protected override void OnClick()
    {
        var radius = OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveRadius.Value;
        var shockwavedPlayers =
            Helpers.GetClosestPlayers(PlayerControl.LocalPlayer, radius * ShipStatus.Instance.MaxLightRadius);
        shockwavedPlayers.RemoveAll(p => p.Data.IsDead || p.Data.Disconnected);

        foreach (var player in shockwavedPlayers)
        {
            player.RpcAddModifier<ShockwavedModifier>();
        }

        
        PlayerControl.LocalPlayer.RpcSendNotification(
            $"You have {TownOfExtraColours.ShockwaveModifierColour.ToTextColor()}shockwaved</color> {shockwavedPlayers.Count} nearby players!",
            "ShockwaveShockwaveButton",
            "ImpModButton"
        );
    }
}