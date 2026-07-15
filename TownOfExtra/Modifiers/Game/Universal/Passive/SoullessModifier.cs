using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modules;
using TownOfExtra.Options;
using TownOfUs;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Universal.Passive;

public class SoullessModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier, IUnguessableModifier
{
    public override string ModifierName => "Soulless";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;
    public override string IntroInfo => "Your soul is collected upon death";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.SoullessModifierIcon;
    public Color ModifierColor => TownOfUsColors.SoulCollector;
    public override Color FreeplayFileColor => TownOfUsColors.SoulCollector;
    public bool IsGuessable => false;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "When you die, you will leave behind a soulless body, as if your soul was reaped by the soul collector.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.SoullessAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<UniversalModifierOptions>.Instance.SoullessChance;
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