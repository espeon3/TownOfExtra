using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Evil;

public sealed class PoltergeistRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Poltergeist";
    public string RoleDescription => "Use their fear of the abnormal against them!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.PoltergeistRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public DoomableType DoomHintType => DoomableType.Trickster;
    
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}Possess {OptionGroupSingleton<PoltergeistRoleOptions>.Instance.WinPossesses} player{((int)OptionGroupSingleton<PoltergeistRoleOptions>.Instance.WinPossesses != 1 ? "s" : "")} to win!</color>\n{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        List<PlayerControl> scaredPlayers = new List<PlayerControl>();
        List<PlayerControl> possessedPlayers = new List<PlayerControl>();
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<ScaredModifier>())
            {
                scaredPlayers.Add(p);
            }
            
            if (p.HasModifier<PossessedModifier>())
            {
                possessedPlayers.Add(p);
            }
        }
        
        var stringB = ITownOfUsRole.SetNewTabText(this);

        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}Scared Players:</color></b>");
        foreach (var p in scaredPlayers)
        {
            stringB.Append(TownOfUsPlugin.Culture, $"\n{p.Data.PlayerName}");
        }
        
        stringB.Append(TownOfUsPlugin.Culture, $"\n<b>{TownOfExtraColours.PossessedColour.ToTextColor()}Possessed Players:</color></b>");
        foreach (var p in possessedPlayers)
        {
            stringB.Append(TownOfUsPlugin.Culture, $"\n{p.Data.PlayerName}");
        }

        return stringB;
    }

    public string GetAdvancedDescription()
    {
        return
            "The Poltergeist is a Neutral Evil role that can scare players, making their soul vulnerable to be possessed. Being scared gives you permanently slightly lower vision, and being possessed gives you much lower vision & slightly lower speed, even if the poltergeist dies." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.PoltergeistRoleIcon,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Scare", "Scare a player, making them vulnerable to being possessed.", TownOfExtraAssets.PoltergeistScareButton),
                new("Possess", "Possess a scared player.", TownOfExtraAssets.PoltergeistPossessButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        if (Player.HasDied())
        {
            return false;
        }
        
        int possessedCount = 0;
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PossessedModifier>())
            {
                possessedCount++;
            }
        }

        return possessedCount >= OptionGroupSingleton<PoltergeistRoleOptions>.Instance.WinPossesses;
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