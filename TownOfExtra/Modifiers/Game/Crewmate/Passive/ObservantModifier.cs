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

namespace TownOfExtra.Modifiers.Game.Crewmate.Passive;

public class ObservantModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Observant";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => "Get more info when meeting deaths happen";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ObservantModifierIcon;
    public Color ModifierColor => TownOfExtraColours.ObservantModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.ObservantModifierColour;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "When a player gets guessed or killed within a meeting due to anything, you will be notified of the exact reason.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ObservantAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.ObservantChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || !role.IsCrewmate())
            return false;

        return true;
    }
}