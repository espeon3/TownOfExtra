using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options.Roles;

public sealed class EmbrittlementRoleOptions : AbstractOptionGroup<EmbrittlementRole>
{
    public override string GroupName => "Embrittlement";

    [ModdedNumberOption("Brittle Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BrittleCooldown { get; set; } = 30f;
    [ModdedNumberOption("Brittle Duration", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float BrittleDuration { get; set; } = 45f;
}