using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Outlier;

public sealed class ShifterRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnguessable
{
    public string RoleName => "Shifter";
    public string RoleDescription => "Shift roles with another player!";
    public string RoleLongDescription => RoleDescription;
    public bool IsGuessable => false;
    public RoleBehaviour AppearAs => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<ShifterRole>());
    public Color RoleColor => TownOfExtraColours.ShifterRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralOutlier;
    public DoomableType DoomHintType => DoomableType.Trickster;
    
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.ShifterRoleColour.ToTextColor()}Shift your role with another player!</color>\n{TownOfExtraColours.ShifterRoleColour.ToTextColor()}Optional Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }

    public string GetAdvancedDescription()
    {
        return
            "The Shifter is a Neutral Outlier role that can switch their role with another player, applying after the next meeting.\n" +
            "<b>This role is unguessable!</b>" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.ShifterRoleIcon,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Shift", "Shift your role with another player.", TownOfExtraAssets.ShifterShiftButton)
            };
        }
    }
}