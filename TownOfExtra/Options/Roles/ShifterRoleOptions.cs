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
    
    [ModdedEnumOption("Shift Method (NOT IMPLEMENTED YET, WILL BE IN FULL RELEASE)", typeof(ShifterShiftMethod),
        ["Next Round", "Timer"])]
    public ShifterShiftMethod ShiftMethod { get; set; } = ShifterShiftMethod.NextRound;
}

public enum ShifterShiftMethod
{
    NextRound,
    Timer,
}