using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class BloodlustRoleOptions : AbstractOptionGroup<BloodlustRole>
{
    public override string GroupName => "Bloodlust";

    [ModdedNumberOption("Kill Cooldown Reduction", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float LowerKillCooldown { get; set; } = 10f;

    [ModdedToggleOption("Can Vent")]
    public bool CanVent { get; set; } = false;
    
    [ModdedToggleOption("Can Sabotage")]
    public bool CanSabotage { get; set; } = false;
}