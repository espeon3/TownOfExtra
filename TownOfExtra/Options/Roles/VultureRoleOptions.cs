using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Evil;

namespace TownOfExtra.Options.Roles;

public sealed class VultureRoleOptions : AbstractOptionGroup<VultureRole>
{
    public override string GroupName => "Vulture";

    [ModdedNumberOption("Eat Cooldown", 0f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EatCooldown { get; set; } = 30f;

    [ModdedNumberOption("# of Bodies Eaten to Win", 1f, 15f)]
    public float EatenBodiesNeeded { get; set; } = 3f;
    
    [ModdedToggleOption("Turn into Amnesiac when cannot win")]
    public bool TurnIntoAmne { get; set; } = true;
}