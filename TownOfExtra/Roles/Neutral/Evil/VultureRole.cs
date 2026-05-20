using System;
using System.Collections.Generic;
using System.Text;
using TownOfExtra.Options.Roles;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Evil;

public sealed class VultureRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Vulture";
    public string RoleDescription => "Eat the bodies of dead crewmates!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.VultureRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Death;
    public static int DeadBodiesEaten;

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.VultureRoleColour.ToTextColor()}Eat {OptionGroupSingleton<VultureRoleOptions>.Instance.EatenBodiesNeeded} dead bodies to win!</color>\n{TownOfExtraColours.VultureRoleColour.ToTextColor()}Optional Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Bodies Eaten: {Color.white.ToTextColor()}{DeadBodiesEaten}/{OptionGroupSingleton<VultureRoleOptions>.Instance.EatenBodiesNeeded}</color>");

        return stringB;
    }
    
    public string GetAdvancedDescription()
    {
        return
            "The Vulture is a Neutral Evil role that has to eat a specific number of dead bodies to win." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.VultureRoleIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Eat", "Eat a dead body.", TownOfExtraAssets.VultureEatButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        if (Player.HasDied())
        {
            return false;
        }

        return DeadBodiesEaten >= OptionGroupSingleton<VultureRoleOptions>.Instance.EatenBodiesNeeded;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}