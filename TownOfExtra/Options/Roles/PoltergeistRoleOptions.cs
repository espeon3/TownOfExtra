using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Evil;

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
    
    [ModdedNumberOption("Vision Multiplier (1=off)", 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float ScaredVisDebuffMulti { get; set; } = 0.80f;

    [ModdedNumberOption("Possessed Multiplier (1=off)", 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float PossessedVisDebuffMulti { get; set; } = 0.65f;
    [ModdedNumberOption("Speed Multiplier (1=off)", 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float SpeedDebuffMultiplier { get; set; } = 0.80f;
}