using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Concealing;

public sealed class CannibalRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Cannibal";
    public string RoleDescription => "Leave no traces of the crew!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CannibalRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Death;

    public string GetAdvancedDescription()
    {
        return
            "The Cannibal is an Impostor Concealing role that leaves no dead bodies when killing." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        UseVanillaKillButton = false,
        Icon = TownOfExtraAssets.CannibalRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Cannibalise", "Kill a player with no dead body left behind.", TownOfExtraAssets.Placeholder)
            };
        }
    }
}