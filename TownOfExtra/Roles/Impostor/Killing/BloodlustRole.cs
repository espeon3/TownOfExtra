using MiraAPI.GameOptions;
using MiraAPI.Roles;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Killing;

public sealed class BloodlustRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnlovable
{
    public string RoleName => "Bloodlust";
    public string RoleDescription => "Unleash your bloodlust on others!";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorKilling;
    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExFearmonger;
    public bool IsUnlovable => true;

    public string GetAdvancedDescription()
    {
        return
            "Bloodlust is an Impostor Killing role that has a lower kill cooldown than others, in exchange for being blocked from venting & sabotaging." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.BloodlustRoleIcon,
        CanUseVent = OptionGroupSingleton<BloodlustRoleOptions>.Instance.CanVent,
        CanUseSabotage =  OptionGroupSingleton<BloodlustRoleOptions>.Instance.CanSabotage,
        UseVanillaKillButton = false
    };
}