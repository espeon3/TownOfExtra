using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options;

public sealed class SignalJammerRoleOptions : AbstractOptionGroup<SignalJammerRole>
{
    public override string GroupName => "Signal Jammer";
    
    [ModdedNumberOption("Jam Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float JamCooldown { get; set; } = 35f;
    [ModdedNumberOption("Jam Duration", 10f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float JamDuration { get; set; } = 15f;
}