using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class SerialKillerRoleOptions : AbstractOptionGroup<SerialKillerRole>
{
    public override string GroupName => "SerialKiller";

    [ModdedNumberOption("Lesser Kill Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float LowerKillCooldown { get; set; } = 30f;

    [ModdedToggleOption("Can Vent")]
    public bool CanVent  { get; set; } = true;
}