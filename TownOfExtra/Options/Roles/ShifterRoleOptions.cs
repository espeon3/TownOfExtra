using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Outlier;

namespace TownOfExtra.Options.Roles;

public sealed class ShifterRoleOptions : AbstractOptionGroup<ShifterRole>
{
    public override string GroupName => "Shifter";

    [ModdedNumberOption("Shift Cooldown", 5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ShiftCooldown { get; set; } = 20f;
    
    [ModdedToggleOption("Shift Modifiers")]
    public bool ShiftModifiers { get; set; } = true;
}