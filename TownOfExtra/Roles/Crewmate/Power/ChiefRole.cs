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
    public string RoleDescription => "Recruit players and shoot <color=#FF0000>evildoers</color>";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.ChiefRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => DoomableType.Hunter;
    
    public string GetAdvancedDescription()
    {
        return
            "The Chief is a Crewmate Power who successes role that can recruit players, converting them into sheriffs, aswell as being able to shoot players with a limited amount of shots, killing and revealing their roles." +
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