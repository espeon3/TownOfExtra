using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class StrikerRoleOptions : AbstractOptionGroup<StrikerRole>
{
    public override string GroupName => "Striker";
    
    [ModdedNumberOption("# of Locate uses per game", 1, 10)]
    public float LocateUses { get; set; } = 5;
    [ModdedNumberOption("# of Locate uses per meeting", 1, 5)]
    public float LocatesPerMeeting { get; set; } = 1;
    [ModdedNumberOption("# of roles shown on Locate", 2, 8)]
    public float LocateRoleAmount { get; set; } = 5;
    
    [ModdedNumberOption("Strike Cooldown", 0f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float StrikeCooldown { get; set; } = 10f;
    
    [ModdedNumberOption("Kill Delay", 0f, 30f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ImpendingDoomDuration { get; set; } = 10f;
    [ModdedToggleOption("Announce Doomed")]
    public bool AnnounceDoomed { get; set; } = true;
    public ModdedToggleOption ShowDoomedTimer { get; } =
        new("Show Doomed Timer", false)
        {
            Visible = () => OptionGroupSingleton<StrikerRoleOptions>.Instance.AnnounceDoomed
        };
    
    [ModdedEnumOption("Intro Blurb", typeof(StrikerIntroBlurb),
        ["Normal", "Nerd candy's special blurb"])]
    public StrikerIntroBlurb IntroBlurb { get; set; } = StrikerIntroBlurb.Normal;

    [ModdedToggleOption("Share Assassin Strike Settings")]
    public bool ShareAssassinSettings { get; set; } = false;

    public ModdedToggleOption StrikerGuessCrewmate { get; } =
        new("Striker Can Guess \"Crewmate\"", false)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessInvest { get; } =
        new("Striker Can Guess Crew Investigative Roles", false)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessNeutralBenign { get; } =
        new("Striker Can Guess Neutral Benign Roles", true)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessNeutralEvil { get; } =
        new("Striker Can Guess Neutral Evil Roles", true)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessNeutralKilling { get; } =
        new("Striker Can Guess Neutral Killing Roles", true)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessNeutralOutlier { get; } =
        new("Striker Can Guess Neutral Outlier Roles", true)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };

    public ModdedToggleOption StrikerGuessImpostors { get; } =
        new("Striker Can Guess Impostor Roles", true)
        {
            Visible = () => !OptionGroupSingleton<StrikerRoleOptions>.Instance.ShareAssassinSettings
        };
}

public enum StrikerIntroBlurb
{
    Normal,
    NerdCandy,
}