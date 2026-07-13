using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public class ClownRoleOptions : AbstractOptionGroup<ClownRole>
{
    public override string GroupName => "Clown";

    [ModdedNumberOption("Place Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float PlaceCooldown { get; set; } = 20f;
    
    [ModdedNumberOption("Place Delay", 0f, 10f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float PlaceDelay { get; set; } = 3f;
    
    [ModdedNumberOption("Max # of Jack-in-the-boxes", 0f, 30f, 5f, zeroInfinity: true)]
    public float MaxJackInTheBoxes { get; set; } = 3f;
}