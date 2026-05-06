using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class UnnamedRoleOptions : AbstractOptionGroup<TaggerRole>
{
    public override string GroupName => "Unnamed";
    
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 40f;
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float MarkCooldown { get; set; } = 20f;
}