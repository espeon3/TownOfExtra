using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class ShadowWalkerRoleOptions : AbstractOptionGroup<ShadowWalkerRole>
{
    public override string GroupName => "Shadow Walker";

    [ModdedNumberOption("Kill Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 30f;
    [ModdedNumberOption("Invis duration on kill", 0f, 10f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float InvisDurOnKill { get; set; } = 3f;
    [ModdedToggleOption("Give speed multiplier on regular kill")]
    public bool SpeedMultiOnRegKill { get; set; } = true;
    
    [ModdedNumberOption("Enshroud Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EnshroudCooldown { get; set; } = 30f;
    [ModdedNumberOption("Enshroud Duration", 10f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EnshroudDuration { get; set; } = 15f;
    [ModdedNumberOption("Enshroud Kill Cooldown", 2.5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EnshroudKillCooldown { get; set; } = 5f;
    [ModdedNumberOption("Cooldown increase per enshrouded kill", 0f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CdIncreasePerEnshroudedKill { get; set; } = 3f;
    [ModdedNumberOption("Enshroud Speed Multiplier", 1f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float EnshroudSpeedMultiplier { get; set; } = 1.5f;
}