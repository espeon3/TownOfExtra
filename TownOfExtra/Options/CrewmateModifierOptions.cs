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

    [ModdedNumberOption("Heavy Workload Amount", 0, 5)]
    public float HeavyWorkloadAmount { get; set; } = 0;
    public ModdedNumberOption HeavyWorkloadChance { get; } =
        new("Heavy Workload Chance", 50f, 0, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount > 0
        };
    [ModdedNumberOption("Heavy Workload Extra Common Tasks", 0, 2)]
    public float HeavyWorkloadExtraCommonTasks { get; set; } = 1;
    [ModdedNumberOption("Heavy Workload Extra Long Tasks", 0, 2)]
    public float HeavyWorkloadExtraLongTasks { get; set; } = 1;
    [ModdedNumberOption("Heavy Workload Extra Shots Tasks", 0, 3)]
    public float HeavyWorkloadExtraShortTasks { get; set; } = 2;
}