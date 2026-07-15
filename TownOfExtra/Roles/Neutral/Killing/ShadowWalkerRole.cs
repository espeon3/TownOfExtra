using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

public sealed class ShadowWalkerRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Shadow Walker";
    public string RoleDescription => "Pounce from the shadows";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.ShadowWalkerRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => DoomableType.Fearmonger;

    public static int CdIncrease = 0;
    public static bool Enshrouded = false;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.ShadowWalkerRoleColour.ToTextColor()}Be the last killer alive, at all costs.</color>\n{TownOfExtraColours.ShadowWalkerRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    public string GetAdvancedDescription()
    {
        return
            $"The Shadow Walker is a Neutral Killing role that gains invisibility for {OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.InvisDurOnKill} second{((int)OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.InvisDurOnKill != 1 ? "s" : "")} after killing. Additionally, the Shadow Walker can Enshroud to become invisible with a speed boost, but each kill while enshrouded increases your permanent kill cooldown." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Cooldown Increase: {CdIncrease}s</b>");

        return stringB;
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.ShadowWalkerRoleIcon,
        CanUseVent = OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.CanVent,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Enshroud", "Become invisible with a speed boost for X seconds, with the duration decreasing after each kill.", TownOfExtraAssets.ShadowWalkerEnshroudButton)
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