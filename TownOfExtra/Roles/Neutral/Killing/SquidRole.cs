using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class SquidRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Squid";
    public string RoleDescription => "Spill ink to slip up players";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.SquidRoleColour;
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
        orCreateTask.Text = $"{TownOfExtraColours.SquidRoleColour.ToTextColor()}Be the last killer alive, at all costs.</color>\n{TownOfExtraColours.SquidRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    public string GetAdvancedDescription()
    {
        return
            "The Squid is a Neutral Killing role who can spill ink on the ground, causing 3 debuffs to players who walk in them.\n\n" +
            $"<size=105%>{TownOfUsColors.Vigilante.ToTextColor()}Debuffs:</color></size>\n" +
            "> Ink splatters onscreen\n" +
            "> Slower speed\n" +
            "> Block role buttons" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.SquidRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Enshroud", "Become invisible with a speed boost for X seconds, with the duration decreasing after each kill.", TownOfExtraAssets.MiscPh)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        var shadowWalkerAmount = CustomRoleUtils.GetActiveRolesOfType<ShadowWalkerRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > shadowWalkerAmount)
        {
            return false;
        }

        return shadowWalkerAmount >= Helpers.GetAlivePlayers().Count - shadowWalkerAmount;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}