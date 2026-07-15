using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Options;
using UnityEngine;

namespace TownOfExtra.Options;

public sealed class NonCrewModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Non Crew Modifiers";
    public override Func<bool> GroupVisible => () => OptionGroupSingleton<RoleOptions>.Instance.IsClassicRoleAssignment;
    public override Color GroupColor => TownOfUsColors.Neutral;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 2;

    /*----------------------
            SCOURGE
    ----------------------*/

    [ModdedNumberOption("Scourge Amount", 0, 5)]
    public float ScourgeAmount { get; set; } = 0;

    public ModdedNumberOption ScourgeChance { get; } =
        new("Scourge Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<NonCrewModifierOptions>.Instance.ScourgeAmount > 0
        };
}