using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Options;
using TownOfExtra.Roles.Impostor.Support;
using TownOfUs.Buttons;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class FreezerFreezeButton : TownOfUsRoleButton<FreezerRole>
{
    public override string Name => "Freeze";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.FreezerRoleColour;
    public override float Cooldown => OptionGroupSingleton<FreezerRoleOptions>.Instance.FreezeCooldown;
    public override float EffectDuration => OptionGroupSingleton<FreezerRoleOptions>.Instance.FreezeDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.Placeholder;

    protected override void OnClick()
    {
        OverrideName("Freeze Active");
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.IsDead) continue;
            if (player.Data.Role is HaunterRole or SpectreRole) continue;

            if (player.Data.Role.IsImpostor)
            {
                player.AddModifier<ImpostorFreezeModifier>();
            }
            else
            {
                player.AddModifier<FreezeModifier>();
            }
        }
    }

    public override void OnEffectEnd()
    {
        OverrideName("Freeze");
    }
}