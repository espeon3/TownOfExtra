using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Crewmate.Investigative;

namespace TownOfExtra.Options.Roles;

public sealed class JournalistRoleOptions : AbstractOptionGroup<JournalistRole>
{
    public override string GroupName => "Journalist";
    
    [ModdedNumberOption("Interview Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float InterviewCooldown { get; set; } = 30f;
    [ModdedToggleOption("Can speak while controlled")]
    public bool CanSpeakWhileControlled { get; set; } = true;
}