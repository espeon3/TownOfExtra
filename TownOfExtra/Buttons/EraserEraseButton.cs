using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Buttons;
using TownOfUs.Options;
using TownOfUs.Options.Maps;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class EraserEraseButton : TownOfUsKillRoleButton<EraserRole, PlayerControl>, IKillButton
{
    public override string Name => "Erase";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<EraserRoleOptions>.Instance.EraseCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.EraserEraseButton;
    public override int MaxUses => (int)OptionGroupSingleton<EraserRoleOptions>.Instance.EraseUses;

    public override PlayerControl GetTarget()
    {
        var genOpt = OptionGroupSingleton<GeneralOptions>.Instance;
        var saboOpt = OptionGroupSingleton<AdvancedSabotageOptions>.Instance;
        var closePlayer = PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);

        var includePostors = genOpt.FFAImpostorMode ||
                             (PlayerControl.LocalPlayer.IsLover() &&
                              OptionGroupSingleton<LoversOptions>.Instance.LoverKillTeammates) ||
                             (saboOpt.KillDuringCamoComms &&
                              closePlayer?.GetAppearanceType() == TownOfUsAppearances.Camouflage);
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(includePostors, Distance, false,
                x => !x.IsLover() && !x.HasModifier<PendingEraseModifier>());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(includePostors, Distance, predicate:x => !x.HasModifier<PendingEraseModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        
        Target.RpcAddModifier<PendingEraseModifier>();
    }
}