using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class BarbarianRoleOptions : AbstractOptionGroup<BarbarianRole>
{
    public override string GroupName => "Barbarian";

    [ModdedNumberOption("Target Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TargetCooldown { get; set; } = 30f;
    [ModdedNumberOption("Target Duration", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TargetDuration { get; set; } = 45f;
    
    [ModdedToggleOption("Can Vent")]
    public bool CanVent  { get; set; } = true;
}