using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Killing;

public sealed class StrikerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnlovable
{
    public string RoleName => "Striker";
    public string RoleDescription => OptionGroupSingleton<StrikerRoleOptions>.Instance.IntroBlurb == StrikerIntroBlurb.Normal ? "Airstrike players by locating their roles!" : "This your adress?";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public bool IsUnlovable => true;

    public string GetAdvancedDescription()
    {
        return
            $"The Striker is an Impostor Killing role that can guess players ingame, causing them to die. They can also locate players inside meetings, giving a hint of {OptionGroupSingleton<StrikerRoleOptions>.Instance.LocateRoleAmount} roles that they could be." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.StrikerRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Locate (Meeting)", "Locate a player, giving you a list of roles they could be.", TownOfExtraAssets.StrikerLocateButton),
                new("Notes", "Open your notepad, listing all of your located players.", TouChatAssets.NormalChatHover),
                new("Strike", "Strike a player, if you correctly guess their role they will die.", TownOfExtraAssets.StrikerStrikeButton)
            };
        }
    }
    
    private MeetingMenu _meetingMenu;
    public static int UsesLeft = (int)OptionGroupSingleton<StrikerRoleOptions>.Instance.LocateUses;
    public static int UsesThisRound;
    public static Dictionary<PlayerControl, string> Messages = new Dictionary<PlayerControl, string>();

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        if (!Player.AmOwner) return;

        _meetingMenu = new MeetingMenu(this, OnClick, abilityType: MeetingAbilityType.Click, activeSprite: TownOfExtraAssets.StrikerLocateButton, exemption: IsExempt, position: new Vector3(-0.40f, 0f, -3f));
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);

        if (Player.AmOwner)
        {
            _meetingMenu.Dispose();
            _meetingMenu = null!;
        }
    }

    public override void OnMeetingStart()
    {
        RoleBehaviourStubs.OnMeetingStart(this);

        if (!Player.AmOwner) return;

        UsesThisRound = 0;
        _meetingMenu.GenButtons(MeetingHud.Instance, usable: !Player.HasDied() && UsesLeft > 0 && UsesThisRound < OptionGroupSingleton<StrikerRoleOptions>.Instance.LocatesPerMeeting);
    }

    public override void OnVotingComplete()
    {
        RoleBehaviourStubs.OnVotingComplete(this);

        if (Player.AmOwner) _meetingMenu.HideButtons();
    }
    
    public void OnClick(PlayerVoteArea voteArea, MeetingHud meeting)
    {
        var target = GameData.Instance.GetPlayerById(voteArea.TargetPlayerId).Object;

        if (UsesLeft > 0)
        {
            if (UsesThisRound < OptionGroupSingleton<StrikerRoleOptions>.Instance.LocatesPerMeeting)
            {
                if (!Messages.ContainsKey(target))
                {
                    UsesLeft--;
                    UsesThisRound++;

                    var allRoles = MiscUtils.GetPotentialRoles().Where(x => !x.IsImpostor() &&
                                                                            x.Role != target.Data.Role.Role
                    );

                    var selectedRoles = allRoles
                        .OrderBy(_ => Guid.NewGuid())
                        .Take((int)OptionGroupSingleton<StrikerRoleOptions>.Instance.LocateRoleAmount - 1)
                        .ToList();

                    selectedRoles.Add(target.Data.Role);
                    selectedRoles = selectedRoles.OrderBy(_ => Guid.NewGuid()).ToList();

                    var title = $"{Palette.ImpostorRed.ToTextColor()}Locate Result</color>";
                    var roleListText =
                        string.Join(", ", selectedRoles.Select(role => MiscUtils.GetHyperlinkText(role)));
                    var msg = $"{target.Data.PlayerName} is one of the following roles:\n{roleListText}";
                    Messages.Add(target, msg);
                    MiscUtils.AddFakeChat(Player.Data, title, msg, false, true);
                    if (!HudManager.Instance.Chat.IsOpenOrOpening) HudManager.Instance.Chat.Toggle();

                    Player.RpcSendNotification(
                        $"You have been {Palette.AcceptedGreen.ToTextColor()}given a hint</color> for {target.Data.PlayerName}'s role!",
                        "StrikerLocateButton",
                        356,
                        Palette.AcceptedGreen
                    );
                }
                else
                {
                    Player.RpcSendNotification(
                        $"You have {Palette.ImpostorRed.ToTextColor()}already located</color> {target.Data.PlayerName}!",
                        "StrikerLocateButton",
                        356,
                        Palette.ImpostorRed
                    );
                }
            }
            else
            {
                Player.RpcSendNotification(
                    $"You have already hit the {Palette.ImpostorRed.ToTextColor()}max uses</color> for this meeting!!",
                    "StrikerLocateButton",
                    356,
                    Palette.ImpostorRed
                );
            }
        }
        else
        {
            Player.RpcSendNotification(
                $"You have already hit the {Palette.ImpostorRed.ToTextColor()}max uses</color> per game!",
                "StrikerLocateButton",
                356,
                Palette.ImpostorRed
            );
        }
    }

    public bool IsExempt(PlayerVoteArea voteArea)
    {
        if (voteArea.AmDead || voteArea.TargetPlayerId == Player.PlayerId) return true;

        var target = voteArea.GetPlayer();
        if (target == null) return true;

        var genOpt = OptionGroupSingleton<GeneralOptions>.Instance;
        if (genOpt.FFAImpostorMode || 
            (Player.IsLover() && OptionGroupSingleton<LoversOptions>.Instance.LoverKillTeammates) ||
            !Messages.ContainsKey(target))
            return false;

        return target.IsImpostor();
    }
}