using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
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
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExFearmonger;
    
    public static List<byte> EatenPlayers = new List<byte>();
    public static byte? CannibalId = null;

    public string GetAdvancedDescription()
    {
        return
            "The Cannibal is a Neutral Killing role that leaves no dead bodies when killing as it devours the player whole." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.CannibalRoleIcon,
        CanUseVent = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanVent,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
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
    
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>()!;
        return console == null || console.AllowImpostor;
    }
}