using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Power;

public sealed class ChiefRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Chief";
    public string RoleDescription => "Recruit players into sheriffs!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.ChiefRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Relentless;
    
    public string GetAdvancedDescription()
    {
        return
            "The Chief is a Crewmate Power role that can recruit players, causing them to become a sheriff!" +
            MiscUtils.AppendOptionsText(GetType());
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