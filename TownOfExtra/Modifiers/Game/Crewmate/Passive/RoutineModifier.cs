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

public class RoutineModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Routine";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => "You gain speed boosts from tasks!";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.RoutineModifierIcon;
    public Color ModifierColor => Palette.CrewmateBlue;
    public override Color FreeplayFileColor => Palette.CrewmateBlue;

    public override string GetDescription()
    {
        return "You gain a short speed boost from tasks!";
    }

    public string GetAdvancedDescription()
    {
        return "When you complete a task, you gain a small speed boost temporarily.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.RoutineChance;
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