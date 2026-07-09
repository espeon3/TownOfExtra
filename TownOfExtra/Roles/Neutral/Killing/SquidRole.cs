using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class SquidRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "Squid";
    public string RoleDescription => "Spill ink to slow down players";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.SquidRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;
    public RoleBehaviour CrewVariant =>
        RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<ClericRole>());

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
            "The Squid is a Neutral Killing role who can spill ink on the ground, causing the following debuffs to players who walk in them.\n\n" +
            $"<size=105%><b>{TownOfUsColors.Vigilante.ToTextColor()}Debuffs:</color></b></size>\n" +
            "> Reduced vision\n" +
            "> Slower speed\n" +
            "> Block role abilities" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.SquidRoleIcon,
        CanUseVent = OptionGroupSingleton<SquidRoleOptions>.Instance.CanVent,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Spill", "Spill ink on the ground, causing 3 debuffs to the next player who walks in it.", TownOfExtraAssets.SquidSpillButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        var SquidAmount = CustomRoleUtils.GetActiveRolesOfType<SquidRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > SquidAmount)
        {
            return false;
        }

        return SquidAmount >= Helpers.GetAlivePlayers().Count - SquidAmount;
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