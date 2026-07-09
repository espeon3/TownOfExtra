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

namespace TownOfExtra.Roles.Impostor.Support;

public sealed class EmbrittlementRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Embrittlement";
    public string RoleDescription => "Make players Brittle.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.EmbrittlementRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Relentless;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.ImpostorRoleColour.ToTextColor()}Kill all who oppose you.</color>\n{TownOfExtraColours.ImpostorRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    public string GetAdvancedDescription()
    {
        return
            "The Embrittlement is an Impostor Support role that may make other players Brittle." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.EmbrittlementRoleIcon,
        MaxRoleCount = 1,
        CanUseVent = true
        GhostRole = false
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Brittle", "Select a player to brittle, they will gain the Brittle modifier.", TownOfExtraAssets.EmbrittleBrittleButton)
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