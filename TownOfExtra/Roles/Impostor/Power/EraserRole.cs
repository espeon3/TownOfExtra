using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfExtra.Modules;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Power;

public sealed class EraserRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Eraser";
    public string RoleDescription => "Erase the roles of others";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExTrickster;
    public static Dictionary<PlayerControl, RoleTypes> ErasedPlayerRoles = new Dictionary<PlayerControl, RoleTypes>();
    
    public string GetAdvancedDescription()
    {
        return
            "The Eraser is an Impostor Power role that can erase player's roles, causing them to become a plain crewmate next meeting!" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.EraserRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Erase", "Erase a player's role, turning them into a plain crewmate next meeting.", TownOfExtraAssets.EraserEraseButton)
            };
        }
    }
}