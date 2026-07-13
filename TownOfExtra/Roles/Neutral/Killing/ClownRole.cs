using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Neutral.Killing;

public sealed class ClownRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public string RoleName => "Clown";
    public string RoleDescription => "Clown around with your jack-in-the-boxes";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.ClownRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExTrickster;
    public RoleBehaviour CrewVariant =>
        RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<PlumberRole>());

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }
        ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = $"{TownOfExtraColours.ClownRoleColour.ToTextColor()}Be the last killer alive, at all costs.</color>\n{TownOfExtraColours.ClownRoleColour.ToTextColor()}Fake Tasks:</color>";
        orCreateTask.name = "NeutralRoleText";
    }
    
    public string GetAdvancedDescription()
    {
        return
            "The Clown is a Neutral Killing role which can place 3 jack-in-the-boxes which when placed, act as 3 linked vents, only usable by the Clown." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.ClownRoleIcon,
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
                new("Place", "Place a jack-in-the-box.", TownOfExtraAssets.ClownPlaceButton)
            };
        }
    }
    
    public bool WinConditionMet()
    {
        var clownAmount = CustomRoleUtils.GetActiveRolesOfType<ClownRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > clownAmount)
        {
            return false;
        }

        return clownAmount >= Helpers.GetAlivePlayers().Count - clownAmount;
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
