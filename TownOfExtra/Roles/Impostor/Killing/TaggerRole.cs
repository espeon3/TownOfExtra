using System.Collections.Generic;
using System.Linq;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Killing;

public sealed class TaggerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Tagger";
    public string RoleDescription => "Mark and kill the crew!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => DoomableType.Relentless;

    public static List<PlayerControl> MarkedPlayers = new List<PlayerControl>();

    public string GetAdvancedDescription()
    {
        return
            "The Tagger is an Impostor Killing role that can mark & kill players, marking a marked player who is already marked will kill them, allowing you to perform double kills solo." +
            MiscUtils.AppendOptionsText(GetType());
    }
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var stringB = ITownOfUsRole.SetNewTabText(this);

        var allMarked = PlayerControl.AllPlayerControls.ToArray().Where(x =>
            !x.HasDied() && MarkedPlayers.Contains(x)).ToList();

        if (allMarked.HasAny())
        {
            stringB.Append(TownOfUsPlugin.Culture, $"\n<b>Marked Players:</b>");
            foreach (var plr in allMarked)
            {
                stringB.Append(TownOfUsPlugin.Culture,
                    $"\n{Color.white.ToTextColor()}{plr.Data.PlayerName}</color>");
            }
        }

        return stringB;
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        MaxRoleCount = 1,
        UseVanillaKillButton = false,
        Icon = TownOfExtraAssets.TaggerRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Mark", "Mark a player, allowing you to eliminate them later.", TownOfExtraAssets.Placeholder),
                new("Eliminate", "Eliminate a marked player.", TouAssets.KillSprite)
            };
        }
    }
}