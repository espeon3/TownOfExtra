using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Crewmate.Power;

namespace TownOfExtra.Options.Roles;

public sealed class ChiefRoleOptions : AbstractOptionGroup<ChiefRole>
{
    public override string GroupName => "Chief";
    
    [ModdedNumberOption("Recruit Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float RecruitCooldown { get; set; } = 30f;
    [ModdedNumberOption("Max Recruit Uses", 0f, 30f, zeroInfinity:true)]
    public float RecruitUses { get; set; } = 2f;
    [ModdedToggleOption("Can Recruit Lover Teammate")]
    public bool CanRecruitLoverTeammate { get; set; } = true;
    [ModdedNumberOption("Shoot Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ShootCooldown { get; set; } = 30f;
    [ModdedNumberOption("Max Shoot Uses", 0f, 30f, zeroInfinity:true)]
    public float ShootUses { get; set; } = 3f;
    [ModdedToggleOption("Spread egotism")]
    public bool SpreadEgotism { get; set; } = false;
}