using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfExtra.Options.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Evil;

public sealed class TricksterRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Trickster";
    public string RoleDescription => "Spawn fake bodies to trick the crew";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.TricksterRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Death;

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
        orCreateTask.Text = $"{TownOfExtraColours.TricksterRoleColour.ToTextColor()}Get {OptionGroupSingleton<TricksterRoleOptions>.Instance.ReportsNeeded} of your fake bodies reported!</color>\n{TownOfExtraColours.TricksterRoleColour.ToTextColor()}Optional Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Fakes Reported: {FakeBodiesReported}/{OptionGroupSingleton<TricksterRoleOptions>.Instance.ReportsNeeded}</color>");

        return stringB;
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
                new("Sample", "Sample a player's colour to make a fake body of.", TownOfExtraAssets.TricksterSampleButton),
                new("Spawn Body", "Spawn a fake dead body of the sampled colour.", TownOfExtraAssets.TricksterPlaceButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        if (Player.HasDied())
        {
            return false;
        }

        return FakeBodiesReported >= OptionGroupSingleton<TricksterRoleOptions>.Instance.ReportsNeeded;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}