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

public sealed class BloodlustKillButton : TownOfUsKillRoleButton<BloodlustRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Kill";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => game.Killcooldown - OptionGroupSingleton<BloodlustOptions>.Instance.ReducedKillCooldown
    public override LoadableAsset<Sprite> Sprite => TouAssets.KillSprite;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

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

    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target, MeetingCheck.OutsideMeeting
        );
    }
}