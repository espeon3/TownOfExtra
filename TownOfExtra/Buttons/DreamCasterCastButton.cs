using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
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

public sealed class DreamCasterCastButton : TownOfUsRoleButton<DreamCasterRole, PlayerControl>
{
    public override string Name => "Lucid Dream";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.DreamCasterCastButton;

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
                x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(includePostors, Distance, false,
            x => !x.HasModifier<LucidDreamingModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<WaitingOnLcdModifier>()) p.RpcRemoveModifier<WaitingOnLcdModifier>();
            if (p.HasModifier<LucidDreamingModifier>()) p.RpcRemoveModifier<LucidDreamingModifier>();
        }
        
        Target.RpcAddModifier<WaitingOnLcdModifier>();
    }
}