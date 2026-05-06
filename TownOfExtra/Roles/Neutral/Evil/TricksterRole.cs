using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Evil;

public sealed class TricksterRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnlovable
{
    public string RoleName => "Trickster";
    public string RoleDescription => "Trick the crewmates!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.TricksterRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Death;
    public bool IsUnlovable => true;

    public static int SampledColourId;
    public static List<DeadBody> SpawnedBodies = new List<DeadBody>();
    public static int FakeBodiesReported;
    public static bool HasSampledColour = false;
    
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfUsColors.Neutral.ToTextColor()}{TouLocale.GetParsed("NeutralOutlierTaskHeader")}</color>";
        orCreateTask.name = "NeutralRoleText";
    }

    public string GetAdvancedDescription()
    {
        return
            "The Trickster is a Neutral Evil role that wins by tricking enough players into reporting fake bodies. They can sample a player and create a fake dead body of them, and when reported, will give the trickster +1 report." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.TricksterRoleIcon,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Sample", "Sample a player's colour to make a fake body of.", TownOfExtraAssets.Placeholder),
                new("Spawn Body", "Spawn a fake dead body of the sampled colour.", TownOfExtraAssets.Placeholder)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        if (Player.HasDied())
        {
            return false;
        }

        return FakeBodiesReported >= 3;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}