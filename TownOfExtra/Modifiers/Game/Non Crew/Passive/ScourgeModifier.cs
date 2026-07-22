using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Non_Crew.Passive;

public class ScourgeModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Scourge";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;
    public override string IntroInfo => "The crew can't task win while you're alive";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ScourgeModifierIcon;
    public Color ModifierColor => TownOfExtraColours.ScourgeModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.ScourgeModifierColour;

    public override string GetDescription()
    {
        return "The crew cannot win on tasks, while you are alive.";
    }

    public string GetAdvancedDescription()
    {
        return "The crew cannot win on tasks, while you are alive.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<NonCrewModifierOptions>.Instance.ScourgeAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<NonCrewModifierOptions>.Instance.ScourgeChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || (!player.IsImpostor() && !player.IsNeutral()))
            return false;

        return true;
    }
}