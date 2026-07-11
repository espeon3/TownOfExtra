using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Achievements;
using TownOfExtra.Networking;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ChiefRecruitButton : TownOfUsRoleButton<ChiefRole, PlayerControl>
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
        return
            GetTarget() != null &&
            Timer <= 0 &&
            (MaxUses == 0 || UsesLeft > 0);
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
                x => !x.IsLover() && !ChiefRole.Recruits.Contains(x));
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, predicate:
            x => !ChiefRole.Recruits.Contains(x));
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
            PlayerControl.LocalPlayer.RpcSendNotification(
                $"{Palette.ImpostorRed.ToTextColor()}There is already a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color> {Palette.ImpostorRed.ToTextColor()}in the game!",
                "ChiefRecruitButton",
                "CrewButton"
            );

            if (MaxUses != 0)
            {
                IncreaseUses();
            }

            NoCd = true;
            AllowActive = false;
            return;
        }

        Recruit = Target;

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"{Recruit.Data.PlayerName} will be {TownOfExtraColours.ChiefRoleColour.ToTextColor()}recruited</color> in 3 seconds!",
            "ChiefRecruitButton",
            "CrewButton"
        );
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

        if (Recruit.Data.IsDead || Recruit.Data.Disconnected)
        {
            if (MaxUses != 0)
            {
                IncreaseUses();
                PlayerControl.LocalPlayer.RpcSendNotification(
                    $"{Palette.ImpostorRed.ToTextColor()}Your recruit is no longer alive. Your charge has been refunded!</color>",
                    "ChiefRecruitButton",
                    "CrewButton"
                );
            }
            else
            {
                PlayerControl.LocalPlayer.RpcSendNotification(
                    $"{Palette.ImpostorRed.ToTextColor()}Your recruit is no longer alive!</color>",
                    "ChiefRecruitButton",
                    "CrewButton"
                );
            }

            Recruit = null;
            return;
        }

        if (Recruit.IsCrewmate())
        {
            Recruit.RpcRemoveModifier<ImitatorCacheModifier>();
            Recruit.RpcChangeRole(RoleId.Get<SheriffRole>());

            Recruit.RpcSendNotification(
                $"You have been recruited by the {TownOfExtraColours.ChiefRoleColour.ToTextColor()}chief</color>, you are now a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color>!",
                "ChiefRecruitButton",
                "CrewButton",
                flashColour: TownOfUsColors.Sheriff
            );

            if (PlayerControl.LocalPlayer.HasModifier<EgotistModifier>() &&
                OptionGroupSingleton<ChiefRoleOptions>.Instance.SpreadEgotism)
            {
                Recruit.RpcAddModifier<EgotistModifier>();

                Recruit.RpcSendNotification(
                    $"You feel a {TownOfUsColors.Egotist.ToTextColor()}dark presence</color> taking over... You are now an {TownOfUsColors.Egotist.ToTextColor()}egotist</color>.",
                    "ChiefRecruitButton",
                    "CrewButton"
                );
            }
        }

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"You have recruited {Recruit.Data.PlayerName} and they are now a {TownOfUsColors.Sheriff.ToTextColor()}sheriff</color>!",
            "ChiefRecruitButton",
            "CrewButton"
        );
        
        PlayerControl.LocalPlayer.RpcAwardAchievement(AApi.GetInstance()?.ChiefUseRecruit);

        ChiefRole.Recruits.Add(Recruit);
        Recruit = null;
    }
}