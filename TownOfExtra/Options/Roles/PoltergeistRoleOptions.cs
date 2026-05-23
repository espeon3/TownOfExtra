using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Outlier;

namespace TownOfExtra.Options.Roles;

public sealed class PoltergeistRoleOptions : AbstractOptionGroup<PoltergeistRole>
{
    public override string GroupName => "Poltergeist";
    
    [ModdedNumberOption("Scare Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ScareCooldown { get; set; } = 25f;
    
    [ModdedNumberOption("Possess Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PossessCooldown { get; set; } = 30f;
    
    [ModdedNumberOption("# of Possesses to win", 1f, 15f)]
    public float WinPossesses { get; set; } = 5f;
}