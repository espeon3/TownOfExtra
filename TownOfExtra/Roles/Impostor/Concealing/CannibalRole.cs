using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Concealing;

public sealed class CannibalRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Cannibal";
    public string RoleDescription => "Leave no traces of the crew!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => TownOfExtraColours.CannibalRoleColour;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Death;
    
    public static List<byte> EatenPlayers = new List<byte>();
    public static byte? CannibalId = null;

    public string GetAdvancedDescription()
    {
        return
            "The Cannibal is an Impostor Concealing role that leaves no dead bodies when killing." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        UseVanillaKillButton = false,
        MaxRoleCount = 1,
        Icon = TownOfExtraAssets.CannibalRoleIcon
    };
    
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new("Cannibalise", "Kill a player with no dead body left behind.", TownOfExtraAssets.Placeholder)
            };
        }
    }
    
    public static void SendRevivedMessage()
    {
        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Medic));
        var notif = Helpers.CreateAndShowNotification(
            $"You have been {TownOfUsColors.Medic.ToTextColor()}revived</color> as the {TownOfExtraColours.CannibalRoleColour.ToTextColor()}cannibal</color> has {Palette.ImpostorRed.ToTextColor()}died</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TouCrewAssets.MedicSprite.LoadAsset());
        notif.AdjustNotification();
    }
}