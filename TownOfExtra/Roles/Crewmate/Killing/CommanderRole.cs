using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Killing;

public sealed class CommanderRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Commander";
    public string RoleDescription => "Enact vengeance for your brawlers";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CommanderRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateKilling;
    public DoomableType DoomHintType => DoomableType.Hunter;
    
    public string GetAdvancedDescription()
    {
        return
            "The Commander is a Crewmate Killing role who can command players into becoming brawlers. Once a brawler dies, the Commander gains a kill charge to try and avenge the killers." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.CommanderRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Avenge", "Kill a player. You will not be told if they are a crewmate or a killer.", TownOfExtraAssets.AttackPh),
            };
        }
    }
}