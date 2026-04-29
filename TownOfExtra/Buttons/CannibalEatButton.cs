using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfExtra.Roles.Impostor.Concealing;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Options;
using TownOfUs.Options.Maps;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class CannibalEatButton : TownOfUsKillRoleButton<CannibalRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Eat";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.CannibalRoleColour;
    public override float Cooldown => OptionGroupSingleton<CannibalRoleOptions>.Instance.KillCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.CannibalEatButton;

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
        
        CannibalRole.EatenPlayers.Add(Target.PlayerId);
        
        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target,
            createDeadBody: false,
            causeOfDeath: "Cannibalised"
        );
    }
}