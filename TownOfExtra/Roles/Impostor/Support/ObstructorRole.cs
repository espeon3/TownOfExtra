using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Support;

public sealed class ObstructorRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Obstructor";
    public string RoleDescription => "Obstruct the crew's abilities";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Default;

    public string GetAdvancedDescription()
    {
        return
            "The Obstructor is an Impostor Support role that can obstruct a player, disabling all buttons, sabotage consoles & tasks, as well as the map & admin table, the next time they try to use any." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.ObstructorRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Obstruct", "Obstruct a player, disabling all buttons, sabotage consoles & tasks, as well as the map & admin table, the next time they try to use any.", TownOfExtraAssets.ObstructorObstructButton)
            };
        }
    }
}