using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options;

public sealed class PoisonerRoleOptions : AbstractOptionGroup<PoisonerRole>
{
    public override string GroupName => "Poisoner";
    
    [ModdedNumberOption("Poison Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PoisonCooldown { get; set; } = 25f;
    [ModdedNumberOption("Poison Length", 5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PoisonLength { get; set; } = 15f;
    [ModdedToggleOption("Show poison to all")]
    public bool ShowPoison { get; set; } = true;
}