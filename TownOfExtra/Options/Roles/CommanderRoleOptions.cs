using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Crewmate.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class CommanderRoleOptions : AbstractOptionGroup<CommanderRole>
{
    public override string GroupName => "Commander";

    [ModdedNumberOption("Command Uses", 1f, 5f, 1f, MiraNumberSuffixes.Seconds)]
    public float CommandUses { get; set; } = 3f;
    [ModdedNumberOption("Command Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CommandCooldown { get; set; } = 30f;
    
    [ModdedNumberOption("Avenge Cooldown", 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AvengeCooldown { get; set; } = 30f;
}