using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public class SquidRoleOptions : AbstractOptionGroup<SquidRole>
{
    public override string GroupName => "Squid";
    
    [ModdedNumberOption("Kill Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 22.5f;
}