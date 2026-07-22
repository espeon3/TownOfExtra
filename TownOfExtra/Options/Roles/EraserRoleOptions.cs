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
    [ModdedToggleOption("Can be assassin?")]
    public bool CanBeAssassin { get; set; } = true;
    
    [ModdedEnumOption("Erased neutral's roles", typeof(ErasedNeutralRole),
        ["Amnesiac", "Survivor"])]
    public ErasedNeutralRole ErasedNeutralRole { get; set; } = ErasedNeutralRole.Survivor;
}

public enum ErasedNeutralRole
{
    Amnesiac,
    Survivor,
}