using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using TownOfExtra.Roles.Impostor.Power;

namespace TownOfExtra.Options.Roles;

public sealed class VinculatorRoleOptions : AbstractOptionGroup<VinculatorRole>, IOptionable
{
    public override string GroupName => "Vinculator";
    
    [ModdedNumberOption("Empower Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float EmpowerCooldown { get; set; } = 30f;
    [ModdedNumberOption("# of Empowers", 0f, 5f, zeroInfinity:true)]
    public float EmpowerUses { get; set; } = 1f;
    [ModdedNumberOption("# of Kills for new Empower", 0f, 5f)]
    public float EmpowerKillsForNew { get; set; } = 2f;
    
    [ModdedNumberOption("Chain Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float ChainCooldown { get; set; } = 20f;
    [ModdedNumberOption("# of Chains", 0f, 5f, zeroInfinity:true)]
    public float ChainUses { get; set; } = 1f;
    [ModdedNumberOption("# of Kills for new Chain", 0f, 5f)]
    public float ChainKillsForNew { get; set; } = 2f;
}