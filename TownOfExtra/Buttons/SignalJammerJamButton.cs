using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Concealing;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class SignalJammerJamButton : TownOfUsRoleButton<SignalJammerRole>
{
    public override string Name => "Jam Signals";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<SignalJammerRoleOptions>.Instance.JamCooldown;
    public override float EffectDuration => OptionGroupSingleton<SignalJammerRoleOptions>.Instance.JamDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.SignalJammerJamButton;

    protected override void OnClick()
    {
        OverrideName("Jamming Signals...");
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.Disconnected) continue;
            player.RpcAddModifier<SignalJammedModifier>();
        }
    }

    public override void OnEffectEnd()
    {
        OverrideName("Jam Signals");
    }
}