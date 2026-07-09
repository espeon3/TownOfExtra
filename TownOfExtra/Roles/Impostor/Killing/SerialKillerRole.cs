using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Killing;

public sealed class SerialKillerRole: ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant, IUnlovable
{
    public string Rolename => "Serial Killer";
    public string RoleDescription => "Unleash your bloodlust on others!";
    public string RoleLongDescription => RoleDescription;
    public color RoleColor => palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    public bool IsUnlovable = true;
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<SheriffRole>());

public string GetAdvancedDescription()
{
    return
          "The Serial Killer is an Impostor Killing role that has a lower kill cooldown than others."  +
          MiscUtils.AppendOptionsText(GetType());

}

public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(This)
{
    Icon = TownOfExtraAssets.KnifeThrowerRoleIcon,
    CanUseVent = OptionGroupSingleton<SerialKillerRoleOptions>.Instance.CanVent
};
}