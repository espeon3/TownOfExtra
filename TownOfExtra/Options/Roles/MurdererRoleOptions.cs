using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class BarbarianRoleOptions : AbstractOptionGroup<BarbarianRole>
{
    public override string GroupName => "Barbarian";

    [ModdedNumberOption("Murder Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MurderCooldown { get; set; } = 20f;
    [ModdedNumberOption("Can Sabotage")]
    public bool CanSabotage { get; set; } = false;

    [ModdedToggleOption("Can Vent")]
    public bool CanVent  { get; set; } = false;
}