using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class MurdererRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "Cannibal";
    public string RoleDescription => "Kill everyone!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CannibalRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Death;
// Todo: Add Crew Variant
    public static List<byte> KilledPlayers = new List<byte>();
    public static byte? MurdererId = null;

    public string GetAdvancedDescription()
    {
        return
            "The Murderer is a Neutral Killing role that is a standard Neutral Killer, having to kill everyone to win while being able to sabotage or vent too." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.MurdererRoleIcon,
        CanUseVent = OptionGroupSingleton<MurdererRoleOptions>.Instance.CanVent,
        CanUseSabotage = OptionGroupSingleton<MurdererRoleOptions>.Instance.CanSabotage
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Murder", "Kill a player.", TouAssets.KillSprite)
            };
        }
    }

    public bool WinConditionMet()
    {
        var cannibalCount = CustomRoleUtils.GetActiveRolesOfType<MurdererRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > murdererCount)
        {
            return false;
        }

        return cannibalCount >= Helpers.GetAlivePlayers().Count - murdererCount;
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