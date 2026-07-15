using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfExtra.Modules;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Power;

public sealed class DreamCasterRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Dream Caster";
    public string RoleDescription => "Make players fall into a state of lucid dreaming.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    public DoomableType DoomHintType => DoomableType.Protective;

    public string GetAdvancedDescription()
    {
        return
            "The Dream Caster is an Impostor Power role that can cast lucid dreams onto players. Being in a lucid dream disables their abilities & prevents them from dying to any cause." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.DreamCasterRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Lucid Dream", "Force the player into a state of lucid dreaming. While in this state, they cannot use abilities or be killed in any way.", TownOfExtraAssets.DreamCasterCastButton)
            };
        }
    }
}