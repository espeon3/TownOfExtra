using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Options;
using UnityEngine;

namespace TownOfExtra.Options;

public sealed class UniversalModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Universal Modifiers";
    public override Func<bool> GroupVisible => () => OptionGroupSingleton<RoleOptions>.Instance.IsClassicRoleAssignment;
    public override Color GroupColor => TownOfUsColors.Neutral;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 2;

    /*----------------------
            SOULLESS
    ----------------------*/

    [ModdedNumberOption("Soulless Amount", 0, 5)]
    public float SoullessAmount { get; set; } = 0;

    public ModdedNumberOption SoullessChance { get; } =
        new("Soulless Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<UniversalModifierOptions>.Instance.SoullessAmount > 0
        };
}