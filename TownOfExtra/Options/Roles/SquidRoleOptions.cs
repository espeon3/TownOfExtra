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
    
    [ModdedNumberOption("Spill Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SpillCooldown { get; set; } = 22.5f;
    [ModdedNumberOption("Ink Duration", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float InkDuration { get; set; } = 30f;
    [ModdedToggleOption("Ink affects squid")]
    public bool InkAffectsSquid { get; set; } = true;
    
    [ModdedNumberOption("Vision Multiplier (1=off)", 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float VisionDebuffMultiplier { get; set; } = 0.25f;
    [ModdedNumberOption("Speed Multiplier (1=off)", 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float SpeedDebuffMultiplier { get; set; } = 0.35f;
    [ModdedToggleOption("Button Block")]
    public bool ButtonBlockDebuff { get; set; } = true;
    
    [ModdedNumberOption("Debuff Durations", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float DebuffDurations { get; set; } = 20f;
    
    [ModdedToggleOption("Can Vent")]
    public bool CanVent  { get; set; } = true;
}