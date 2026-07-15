using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
//todo: using TownOfExtra.Achievements;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
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
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<FreezerRoleOptions>.Instance.FreezeCooldown;
    public override float EffectDuration => OptionGroupSingleton<FreezerRoleOptions>.Instance.FreezeDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.FreezerFreezeButton;

    protected override void OnClick()
    {
        OverrideName("Freeze Active");
        
        //todo: PlayerControl.LocalPlayer.RpcAwardAchievement(AApi.GetInstance()?.FreezerUseAbility);
        
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.IsDead || player.Data.Role is HaunterRole or SpectreRole) continue;

            if (player.Data.Role.IsImpostor)
            {
                player.RpcAddModifier<ImpostorFreezeModifier>();
            }
            else
            {
                player.RpcAddModifier<FreezeModifier>();
            }
        }
    }

    public override void OnEffectEnd()
    {
        OverrideName("Freeze");
    }
}