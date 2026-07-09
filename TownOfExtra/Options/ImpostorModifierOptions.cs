using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Options;
using UnityEngine;

namespace TownOfExtra.Options;

public sealed class ImpostorModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Impostor Modifiers";
    public override Func<bool> GroupVisible => () => OptionGroupSingleton<RoleOptions>.Instance.IsClassicRoleAssignment;
    public override Color GroupColor => TownOfUsColors.Impostor;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 2;

    /*----------------------
            SHOCKWAVE
    ----------------------*/

    [ModdedNumberOption("Shockwave Amount", 0, 5)]
    public float ShockwaveAmount { get; set; } = 0;

    public ModdedNumberOption ShockwaveChance { get; } =
        new("Shockwave Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    public ModdedNumberOption ShockwaveRadius { get; } =
        new("Shockwave Radius", 1f, 0.25f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    public ModdedNumberOption ShockwaveCooldown { get; } =
        new("Shockwave Cooldown", 25f, 2.5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    public ModdedNumberOption ShockwaveUses { get; } =
        new("Shockwave Uses", 1f, 1f, 5f, 1f, MiraNumberSuffixes.None)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    public ModdedNumberOption ShockwaveEffectDuration { get; } =
        new("Shockwave effect durations", 10f, 5f, 15f, 1f, MiraNumberSuffixes.Seconds)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    public ModdedNumberOption ShockwaveVisionDebuffMultiplier { get; } =
        new("Shockwave Vision Multiplier", 0.05f, 0f, 0.25f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };

    public ModdedNumberOption ShockwaveSpeedDebuffMultiplier { get; } =
        new("Shockwave Speed Multiplier (1=off)", 0.35f, 0f, 1f, 0.05f, MiraNumberSuffixes.Multiplier, "0.00")
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveAmount > 0
        };
    
    /*----------------------
            REBIRTH
    ----------------------*/

    [ModdedNumberOption("Rebirth Amount", 0, 5)]
    public float RebirthAmount { get; set; } = 0;

    public ModdedNumberOption RebirthChance { get; } =
        new("Rebirth Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.RebirthAmount > 0
        };
    
    /*----------------------
         EMBRITTLEMENT
    ----------------------*/

    [ModdedNumberOption("Embrittlement Amount", 0, 5)]
    public float EmbrittlementAmount { get; set; } = 0;

    public ModdedNumberOption EmbrittlementChance { get; } =
        new("Embrittlement Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.EmbrittlementAmount > 0
        };
    
    /*----------------------
            BLOODLUST
    ----------------------*/

    [ModdedNumberOption("Bloodlust Amount", 0, 5)]
    public float BloodlustAmount { get; set; } = 0;

    public ModdedNumberOption BloodlustChance { get; } =
        new("Bloodlust Chance", 50f, 0f, 100f, 10f, MiraNumberSuffixes.Percent)
        {
            Visible = () => OptionGroupSingleton<ImpostorModifierOptions>.Instance.BloodlustAmount > 0
        };
}