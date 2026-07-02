using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Options.Roles;

public sealed class CannibalRoleOptions : AbstractOptionGroup<CannibalRole>
{
    public override string GroupName => "Cannibal";
    
    [ModdedNumberOption("Kill Cooldown", 7.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;
    [ModdedToggleOption("Increase kcd per kill")]
    public bool IncreaseKcdOnKillOrNot { get; set; } = true;
    public ModdedNumberOption CdIncreaseOnKill { get; } =
        new("Cd Increase", 7.5f, 2.5f, 240f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<CannibalRoleOptions>.Instance.IncreaseKcdOnKillOrNot
        };
    
    
    [ModdedToggleOption("Revive eaten players if cannibal dies")]
    public bool ReviveIfDeadCannibal { get; set; } = false;
    [ModdedToggleOption("Can Vent")]
    public bool CanVent { get; set; } = true;
}