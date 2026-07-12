using System.Collections.Generic;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Power;

public sealed class ChiefRole : CrewmateRole, ITouCrewRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Chief";
    public string RoleDescription => $"Recruit players and shoot {Palette.ImpostorRed.ToTextColor()}evildoers</color>";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.ChiefRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Hunter;
    public bool IsPowerCrew => false;

    public static List<PlayerControl> Recruits = new List<PlayerControl>();
    public static List<PlayerControl> ShotPlayers = new List<PlayerControl>();
    
    public string GetAdvancedDescription()
    {
        return
            "The Chief is a Crewmate Power role who successes players , converting them into loyal sheriffs, aswell as being able to shoot players with a limited amount of shots, killing and revealing their roles.\nWhen they are an Egotist, they will also fuel their recruits with egotism and make them a fellow Egotist." +
            MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);
        
        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Recruits:</b>");
        foreach (var p in Recruits)
        {
            if (p.Data.IsDead)
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{Palette.ImpostorRed.ToTextColor()}{p.Data.PlayerName}</color>");
            }
            else if (p.Data.Disconnected)
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{TownOfUsColors.Neutral.ToTextColor()}{p.Data.PlayerName}</color>");
            }
            else
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{Palette.AcceptedGreen.ToTextColor()}{p.Data.PlayerName}</color>");
            }
        }
        
        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Shot Players:</b>");
        foreach (var p in ShotPlayers)
        {
            var r = p.GetRoleWhenAlive();
            if (r.IsImpostor)
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{Palette.ImpostorRed.ToTextColor()}{p.Data.PlayerName}:</color> {TownOfExtraColours.GetRoleColour(r.NiceName).ToTextColor()}{r.NiceName}</color>");
            }
            else if (r.IsNeutral())
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{TownOfUsColors.Neutral.ToTextColor()}{p.Data.PlayerName}:</color> {TownOfExtraColours.GetRoleColour(r.NiceName).ToTextColor()}{r.NiceName}</color>");
            }
            else
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{Palette.CrewmateBlue.ToTextColor()}{p.Data.PlayerName}:</color> {TownOfExtraColours.GetRoleColour(r.NiceName).ToTextColor()}{r.NiceName}</color>");
            }
        }
        
        return stringB;
    }
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.ChiefRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Recruit", "Recruit a player, turning them into a sheriff.", TownOfExtraAssets.ChiefRecruitButton),
                new("Shoot", "Shoot a player, killing them. You will be notified of the killed player's role.", TownOfExtraAssets.ChiefShootButton)
            };
        }
    }
}