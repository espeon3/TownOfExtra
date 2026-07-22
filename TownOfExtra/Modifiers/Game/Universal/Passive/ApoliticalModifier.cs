using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modules;
using TownOfExtra.Options;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Universal.Passive;

public class ApoliticalModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier, IUnguessableModifier
{
    public override string ModifierName => "Apolitical";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;
    public override string IntroInfo => "Your cooldowns increase for each vote you gain";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ApoliticalModifierIcon;
    public Color ModifierColor => TownOfExtraColours.ApoliticalModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.ApoliticalModifierColour;
    public bool IsGuessable => false;

    public static int CdIncrease = 0;

    public override string GetDescription()
    {
        return $"Your cooldowns increase for each vote you gain.\n<b>Cooldown Increase: {CdIncrease}s</b>";
    }

    public string GetAdvancedDescription()
    {
        return $"Your role's button cooldowns increases by {OptionGroupSingleton<UniversalModifierOptions>.Instance.ApoliticalCdIncrease.Value} for each vote you gain. (Until the next meeting)";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.ApoliticalAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<UniversalModifierOptions>.Instance.ApoliticalChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead)
            return false;

        return true;
    }
}