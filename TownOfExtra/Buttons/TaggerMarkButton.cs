using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Options;
using TownOfUs.Options.Maps;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class TaggerMarkButton : TownOfUsKillRoleButton<TaggerRole, PlayerControl>, IKillButton
{
    public override string Name => "Mark";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<UnnamedRoleOptions>.Instance.MarkCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.TaggerMarkButton;

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

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(includePostors, Distance);
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        var target = GetTarget();
        if (target == null || !TaggerRole.MarkedPlayers.Contains(target))
        {
            OverrideName("Mark");
            OverrideSprite(TownOfExtraAssets.TaggerMarkButton.LoadAsset());
        }
        else
        {
            OverrideName("Eliminate");
            OverrideSprite(TouAssets.KillSprite.LoadAsset());
        }
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        if (TaggerRole.MarkedPlayers.Contains(Target))
        {
            PlayerControl.LocalPlayer.RpcSpecialMurder(
                Target, MeetingCheck.OutsideMeeting
            );
            TaggerRole.MarkedPlayers.Remove(Target);
        }
        else
        {
            TaggerRole.MarkedPlayers.Add(Target);
        }
    }
}