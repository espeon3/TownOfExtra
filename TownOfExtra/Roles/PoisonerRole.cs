using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfExtra.Options;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles;

public sealed class PoisonerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Poisoner";
    public string RoleLongDescription => "Infect the ship with a deadly poison!";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor => TownOfExtraColours.PoisonerRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Fearmonger;

    public string GetAdvancedDescription()
    {
        return
            "The Poisoner is a Neutral Killing role that wins by being the last killer alive. They can poison players, making their screen become progressively greener, before dying." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanUseVent = OptionGroupSingleton<PoisonerRoleOptions>.Instance.CanVent,
        Icon = TownOfExtraAssets.PoisonerRoleIcon,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
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