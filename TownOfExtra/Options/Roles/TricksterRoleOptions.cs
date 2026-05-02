using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Evil;

namespace TownOfExtra.Options;

public sealed class TricksterRoleOptions : AbstractOptionGroup<TricksterRole>
{
    public override string GroupName => "Trickster";
    
    [ModdedNumberOption("Sample Cooldown", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SampleCooldown { get; set; } = 10f;
    [ModdedNumberOption("Place Cooldown", 10f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PlaceCooldown { get; set; } = 32.5f;
    [ModdedNumberOption("Reports needed to win", 2f, 15f)]
    public float ReportsNeeded { get; set; } = 5f;
}