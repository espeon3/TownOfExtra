using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfExtra.Modules;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Power;

public sealed class ConjurerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Conjurer";
    public string RoleDescription => "Conjure up rocks to block players paths";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExTrickster;

    public string GetAdvancedDescription()
    {
        return
            "The Conjurer is an Impostor Power role that can conjure to summon a rotatable rock wherever they click, blocking players from passing and killing any it lands on.\n\n" +
            $"<b>{TownOfUsColors.Vigilante.ToTextColor()}Controls:</color></b>\n" +
            "<b>PC | Mobile [Touches]</b>\n" +
            "LMB | 1 = Drop\n" +
            "RMB | 2 = Rotate\n" +
            "ESC | 3 = Cancel" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.ConjurerRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Conjure", "Summon a rock wherever you click, blocking players from passing and killing any it lands on. Left click to place, right  click to rotate, esc to cancel.", TownOfExtraAssets.ConjurerConjureButton)
            };
        }
    }
}