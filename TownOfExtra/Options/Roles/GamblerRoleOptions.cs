using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Support;

namespace TownOfExtra.Options.Roles;

public sealed class GamblerRoleOptions : AbstractOptionGroup<GamblerRole>
{
    public override string GroupName => "Gambler";
    
    [ModdedToggleOption("Can get same modifier twice in a row?")]
    public bool TwiceInARow { get; set; } = true;
    
    [ModdedToggleOption("Longer Cooldown")]
    public bool LongerCooldownEnabled { get; set; } = true;
    public ModdedNumberOption CooldownBoost { get; } =
        new("Cooldown Boost", 5f, 2.5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.LongerCooldownEnabled
        };
    [ModdedToggleOption("Shorter Cooldown")]
    public bool ShorterCooldownEnabled { get; set; } = true;
    public ModdedNumberOption CooldownNerf { get; } =
        new("Cooldown Nerf", 5f, 2.5f, 30f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.ShorterCooldownEnabled
        };
    [ModdedToggleOption("Nothing")]
    public bool NothingEnabled { get; set; } = true;
    [ModdedToggleOption("Viper Body")]
    public bool ViperBodyEnabled { get; set; } = true;
    public ModdedNumberOption ViperBodyDissolveDuration { get; } =
        new("Dissolve Duration", 10f, 5f, 20f, 5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.ViperBodyEnabled
        };
    [ModdedToggleOption("Teleport Back")]
    public bool TeleportBackEnabled { get; set; } = true;
    public ModdedNumberOption TeleportBackDelay { get; } =
        new("Delay", 5f, 2.5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.TeleportBackEnabled
        };
    [ModdedToggleOption("No Body")]
    public bool NoBodyEnabled { get; set; } = true;
    [ModdedToggleOption("Self Report")]
    public bool SelfReportEnabled { get; set; } = true;
    public ModdedToggleOption SelfReportIgnoreCelebrity { get; } =
        new("Ignore Celebrity", true)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.SelfReportEnabled
        };
    [ModdedToggleOption("Invisibility")]
    public bool InvisibilityEnabled { get; set; } = true;
    public ModdedNumberOption InvisibilityDuration { get; } =
        new("Duration", 7.5f, 2.5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<GamblerRoleOptions>.Instance.InvisibilityEnabled
        };
}