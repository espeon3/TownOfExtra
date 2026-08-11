using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfExtra.Modules;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Crewmate.Power;

public sealed class JekyllRole : CrewmateRole, ITownOfUsRole, IWikiDiscoverable, IDoomable, IGuessable
{
    public string RoleName => "Jekyll";
    public string RoleDescription => "Finish your tasks until you transform.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CommanderRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmatePower;
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExTrickster;
    
    public string GetAdvancedDescription()
    {
        return
            "The Jekyll and Hyde are an abomination of two roles, one being a regular crewmate and the other a dangerous beast; an Impostor. Every emergency meeting you aren't exiled transforms you into the other role." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        IconTmp = MiraAPI.Utilities.Assets.TmpSpriteUtils.CreateSpriteAsset(TownOfExtraAssets.JeknHydIcon.LoadAsset(), "ToEx.Role.Crewmate.Jekyll", 1.45f),
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.JeknHydIcon
    };

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Medicine", "Drink medicine, immediatly transforming into the Hyde.", TownOfExtraAssets.JekyllMedicineButton),
            };
        }
    }
}