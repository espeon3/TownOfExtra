using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Concealing;

public sealed class HolographerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Holographer";
    public string RoleDescription => "Spawn holographic decoys of other players!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Trickster;

    public string GetAdvancedDescription()
    {
        return
            "The Holographer is an Impostor Concealing role that can spawn a hologram of a dead player, tricking others into thinking they are alive.\n\n" +
            $"<b>{TownOfUsColors.Vigilante.ToTextColor()}Controls:</color></b>\n" +
            "<b>PC | Mobile [Touches]</b>\n" +
            "LMB | 1 = Drop\n" +
            "RMB | 2 = Change Target\n" +
            "ESC | 3 = Cancel" +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.HolographerRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Holograph", "Spawn a hologram of a dead player, tricking others into thinking they are alive.", TownOfExtraAssets.Placeholder)
            };
        }
    }
}