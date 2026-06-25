using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Concealing;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class CannibalRoleOptions : AbstractOptionGroup<CannibalRole>
{
    public override string GroupName => "Cannibal";
    
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 35f;
    [ModdedToggleOption("Revive eaten players if cannibal dies")]
    public bool ReviveIfDeadCannibal { get; set; } = false;
}