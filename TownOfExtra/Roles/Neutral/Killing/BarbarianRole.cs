using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class BarbarianRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Barbarian";
    public string RoleDescription => "Charge up attacks to kill players";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.BarbarianRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.BarbarianRoleColour.ToTextColor()}Be the last killer alive, at all costs.</color>\n{TownOfExtraColours.BarbarianRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    public string GetAdvancedDescription()
    {
        return
            "The Barbarian is a Neutral Killing role who can charge up attacks, letting them kill as many players as they have attacks with no kill cooldown." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.BarbarianRoleIcon,
        MaxRoleCount = 1,
        CanUseVent = OptionGroupSingleton<BarbarianRoleOptions>.Instance.CanVent,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Target", "Select a player to target, if they are killed, you gain an attack charge.", TownOfExtraAssets.BarbarianTargetButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        var barbarianAmount = CustomRoleUtils.GetActiveRolesOfType<BarbarianRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > barbarianAmount)
        {
            return false;
        }

        return barbarianAmount >= Helpers.GetAlivePlayers().Count - barbarianAmount;
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