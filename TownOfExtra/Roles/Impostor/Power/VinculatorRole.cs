using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Power;

public sealed class VinculatorRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Vinculator";
    public string RoleDescription => "Intertwine the fates of the crew!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    
    public string GetAdvancedDescription()
    {
        return
            "The Vinculator is an Impostor Power role that can intertwine the fates of players with deadly chains and empower fellow impostors by instantly refreshing their abilities!\n" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.VinculatorRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Chain", "Link two players together. If one is ejected, both die.", TownOfExtraAssets.VinculatorChainButton),
                new("Empower", "Refresh a teammate’s cooldowns instantly.", TownOfExtraAssets.VinculatorEmpowerButton)
            };
        }
    }
}