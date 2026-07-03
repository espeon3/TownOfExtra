using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options.Roles;

public sealed class ObstructorRoleOptions : AbstractOptionGroup<ObstructorRole>
{
    public override string GroupName => "Obstructor";

    [ModdedNumberOption("Obstruct Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ObstructCooldown { get; set; } = 25f;

    [ModdedNumberOption("Obstruct Duration", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ObstructDuration { get; set; } = 15f;
}