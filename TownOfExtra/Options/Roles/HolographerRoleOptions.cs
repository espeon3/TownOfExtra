using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Concealing;

namespace TownOfExtra.Options.Roles;

public sealed class HolographerRoleOptions : AbstractOptionGroup<HolographerRole>
{
    public override string GroupName => "Holographer";

    [ModdedNumberOption("Holograph Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HolographCooldown { get; set; } = 35f;

    [ModdedNumberOption("Hologram Duration", 2.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float HologramDuration { get; set; } = 15f;
    
    [ModdedEnumOption("Can holograph", typeof(CanHolographOptions),
        ["Everyone", "Dead"])]
    public CanHolographOptions CanHolograph { get; set; } = CanHolographOptions.Dead;
}

public enum CanHolographOptions
{
    Everyone,
    Dead
}