using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Modules;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Investigative;

public sealed class JournalistRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Journalist";
    public string RoleDescription => "Interview players to gain their information";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.JournalistRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;
    public DoomableType DoomHintType => DoomableType.Insight;
    
    public string GetAdvancedDescription()
    {
        return
            "The Journalist is a Crewmate Investigative role who can interview players, giving them an ingame chat until the next meeting starts. The journalist is anonymous to the interviewee." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.JournalistRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Interview", "Interview a player, letting them send you one piece of information from the game.", TownOfExtraAssets.JournalistInterviewButton),
            };
        }
    }
    
    public void OnRoundStart()
    {
        if (!Player.AmOwner)
        {
            return;
        }

        if (ModifierUtils.GetActiveModifiers<InterviewModifier>().FirstOrDefault() == null)
        {
            return;
        }

        HudManager.Instance.Chat.SetVisible(true);
    }
}