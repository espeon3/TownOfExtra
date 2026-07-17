using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class CannibalRoleOptions : AbstractOptionGroup<CannibalRole>
{
    public override string GroupName => "Cannibal";

    [ModdedNumberOption("Cannibal Eat Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float SwallowCooldown { get; set; } = 25f;

    [ModdedNumberOption("Max Players in Cannibal's Stomach", 1f, 10f, 1f, MiraNumberSuffixes.None)]
    public float MaxSwallowed { get; set; } = 3f;

    [ModdedToggleOption("Can Use Map while swallowed")]
    public bool CanUseMapWhileSwallowed { get; set; } = false;

    [ModdedToggleOption("Can Eat Through Shields")]
    public bool CanSwallowThroughShields { get; set; } = false;

    [ModdedToggleOption("Cannibal can Vent")]
    public bool CanVent { get; set; } = false;
}