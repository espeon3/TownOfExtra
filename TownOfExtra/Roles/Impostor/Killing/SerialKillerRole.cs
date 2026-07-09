using System.Collections.Generic;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Killing;

public sealed class SerialKillerRole: ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant, IUnlovable
{
    public string RoleName => "Serial Killer";
    public string RoleDescription => "Unleash your bloodlust on others!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public bool IsUnlovable => true;
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<SheriffRole>());

public string GetAdvancedDescription()
{
    return
          "The Serial Killer is an Impostor Killing role that has a lower kill cooldown than others."  +
          MiscUtils.AppendOptionsText(GetType());

}

public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
{
    Icon = TownOfExtraAssets.KnifeThrowerRoleIcon,
    CanUseVent = OptionGroupSingleton<SerialKillerRoleOptions>.Instance.CanVent
};
}
