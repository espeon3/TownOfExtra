using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

// this is coded so wierdly idk if theres stuff for this like NoCd/Clickable/AllowActive their probably is but im dumb alr 💔
public sealed class ChiefRecruitButton : TownOfUsKillRoleButton<ChiefRole, PlayerControl>, IKillButton
{
    public override string Name => "Recruit";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.ChiefRoleColour;
    public override float Cooldown => NoCd ? 0f : OptionGroupSingleton<ChiefRoleOptions>.Instance.RecruitCooldown;
    public override float EffectDuration => AllowActive ? 3f : 0f;
    public override bool IsEffectCancellable() => true;

    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ChiefRecruitButton;
    public override int MaxUses => (int)OptionGroupSingleton<ChiefRoleOptions>.Instance.RecruitUses;
    public PlayerControl Recruit;
    public bool NoCd;
    public bool AllowActive = true;

    public override bool CanClick()
    {
        return GetTarget() != null && Timer <= 0;
    }

    public override PlayerControl GetTarget()
    {
        if (Recruit != null)
        {
            return null;
        }
        
        if (!OptionGroupSingleton<ChiefRoleOptions>.Instance.CanRecruitLoverTeammate && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
                x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    protected override void OnClick()
    {
        AllowActive = true;
        NoCd = false;
        Recruit = null;
        var sheriffAlive = false;

        if (Target == null) return;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.GetTownOfUsRole() is SheriffRole)
            {
                sheriffAlive = true;
            }
        }

        if (sheriffAlive)
        {
            var c = Palette.ImpostorRed.ToTextColor();
            var notif = Helpers.CreateAndShowNotification(
                $"{c}There is already a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color>{c} in the game!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.ChiefRoleIcon.LoadAsset());
            notif.AdjustNotification();

            if (MaxUses != 0)
            {
                IncreaseUses();
            }

            NoCd = true;
            AllowActive = false;
            return;
        }
        Recruit = Target;

        var recruitingnotif = Helpers.CreateAndShowNotification(
            $"{Recruit.Data.PlayerName} will be {TownOfExtraColours.ChiefRoleColour.ToTextColor()}recruited</color> in 3 seconds!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.ChiefRoleIcon.LoadAsset());
        recruitingnotif.AdjustNotification();
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (Recruit != null)
        {
            OverrideName("Recruiting...");
        }
        else
        {
            OverrideName("Recruit");
        }
    }

    public override void OnEffectEnd()
    {
        if (Recruit == null) return;

        if (Recruit.IsCrewmate())
        {
            Recruit.RpcChangeRole(RoleId.Get<SheriffRole>());
            ChiefRpcs.RpcNotifyChiefRecruited(Recruit);
        }

        var notif = Helpers.CreateAndShowNotification(
            $"You have recruited {Recruit.Data.PlayerName} and they are now a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.ChiefRoleIcon.LoadAsset());
        notif.AdjustNotification();
        Recruit = null;
    }
}