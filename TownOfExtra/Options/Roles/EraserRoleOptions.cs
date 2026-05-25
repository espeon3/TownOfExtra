using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Power;

namespace TownOfExtra.Options.Roles;

public sealed class EraserRoleOptions : AbstractOptionGroup<EraserRole>
{
    public override string GroupName => "Eraser";
    
    [ModdedNumberOption("Erase Cooldown", 10f, 300f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EraseCooldown { get; set; } = 40f;
    [ModdedNumberOption("Erase Uses", 0f, 30f, zeroInfinity:true)]
    public float EraseUses { get; set; } = 3f;
    /*[ModdedToggleOption("Cleric can unerase")]
    public bool CanUnerase { get; set; } = true;*/
}