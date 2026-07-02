using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Options;
using UnityEngine;

namespace TownOfExtra.Options;

public sealed class CrewmateModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Crewmate Modifiers";
    public override Func<bool> GroupVisible => () => OptionGroupSingleton<RoleOptions>.Instance.IsClassicRoleAssignment;
    public override Color GroupColor => Palette.CrewmateRoleHeaderBlue;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 2;

    /*----------------------
         HEAVY WORKLOAD
    ----------------------*/

    [ModdedNumberOption("Heavy Workload Amount", 0, 5)]
    public float HeavyWorkloadAmount { get; set; } = 0;

    public ModdedNumberOption HeavyWorkloadChance { get; } =
        new("Heavy Workload Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount > 0
        };

    public ModdedNumberOption HeavyWorkloadExtraCommonTasks { get; } =
        new("Extra Common Tasks", 1f, 0f, 2f, 1f, MiraNumberSuffixes.None)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount > 0
        };

    public ModdedNumberOption HeavyWorkloadExtraLongTasks { get; } =
        new("Extra Long Tasks", 1f, 0f, 2f, 1f, MiraNumberSuffixes.None)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount > 0
        };

    public ModdedNumberOption HeavyWorkloadExtraShortTasks { get; } =
        new("Extra Short Tasks", 2f, 0f, 3f, 1f, MiraNumberSuffixes.None)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount > 0
        };

    /*----------------------
             ROUTINE
    ----------------------*/

    [ModdedNumberOption("Routine Amount", 0, 5)]
    public float RoutineAmount { get; set; } = 0;

    public ModdedNumberOption RoutineChance { get; } =
        new("Routine Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineAmount > 0
        };

    public ModdedNumberOption RoutineSpeedBoost { get; } =
        new("Speed Boost", 1.5f, 1.25f, 2f, 0.25f, MiraNumberSuffixes.Multiplier)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineAmount > 0
        };

    public ModdedNumberOption RoutineSpeedBoostDuration { get; } =
        new("Speed Boost Duration", 5f, 5f, 20f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineAmount > 0
        };
    
    /*----------------------
             BRITTLE
    ----------------------*/

    [ModdedNumberOption("Brittle Amount", 0, 5)]
    public float BrittleAmount { get; set; } = 0;

    public ModdedNumberOption BrittleChance { get; } =
        new("Brittle Chance", 30f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleAmount > 0
        };
    public ModdedNumberOption BrittleMaxInteractions { get; } =
        new("Max Interactions", 5f, 1f, 20f, 1f, MiraNumberSuffixes.None)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleAmount > 0
        };
    
    /*----------------------
           OBSERVANT
   ----------------------*/

    [ModdedNumberOption("Observant Amount", 0, 5)]
    public float ObservantAmount { get; set; } = 0;

    public ModdedNumberOption ObservantChance { get; } =
        new("Observant Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.ObservantAmount > 0
        };
    
    /*----------------------
            CLUMSY
   ----------------------*/

    [ModdedNumberOption("Clumsy Amount", 0, 5)]
    public float ClumsyAmount { get; set; } = 0;

    public ModdedNumberOption ClumsyChance { get; } =
        new("Clumsy Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsyAmount > 0
        };
    
    public ModdedNumberOption ClumsySabotageChance { get; } =
        new("Sabotage Chance", 70, 10f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsyAmount > 0
        };
}