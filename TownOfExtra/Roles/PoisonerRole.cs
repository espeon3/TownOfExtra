using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles;

public sealed class PoisonerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Poisoner";
    public string RoleDescription => "Infect the ship with a deadly poison!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.PoisonerRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Fearmonger;

    public string GetAdvancedDescription()
    {
        return
            "The Poisoner is an Impostor Killing role that can poison players, making their screen become green, before dying." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.PoisonerRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Poison", "Poison a player causing them to die later in the game.", TownOfExtraAssets.PoisonerRoleIcon)
            };
        }
    }
}