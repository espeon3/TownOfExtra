using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options;

public sealed class FreezerRoleOptions : AbstractOptionGroup<FreezerRole>
{
    public override string GroupName => "Freezer";
    
    [ModdedNumberOption("Freeze Cooldown", 10f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float FreezeCooldown { get; set; } = 45f;
    [ModdedNumberOption("Freeze Duration", 5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float FreezeDuration { get; set; } = 15f;
    [ModdedToggleOption("Alert Impostors")]
    public bool AlertImpostors { get; set; } = true;
}