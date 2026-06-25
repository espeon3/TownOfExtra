using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class CannibalRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Cannibal";
    public string RoleDescription => "Leave no traces of the crew!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CannibalRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Death;
    
    public static List<byte> EatenPlayers = new List<byte>();
    public static byte? CannibalId = null;

    public string GetAdvancedDescription()
    {
        return
            "The Cannibal is a Neutral Killing role that leaves no dead bodies when killing." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.CannibalRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Eat", "Kill a player with no dead body left behind.", TouAssets.KillSprite)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        var cannibalCount = CustomRoleUtils.GetActiveRolesOfType<CannibalRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > cannibalCount)
        {
            return false;
        }

        return cannibalCount >= Helpers.GetAlivePlayers().Count - cannibalCount;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}