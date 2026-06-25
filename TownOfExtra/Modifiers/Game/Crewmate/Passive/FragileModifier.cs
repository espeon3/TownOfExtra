using System.Collections.Generic;
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

public class FragileModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Fragile";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => "You could die when interacted with";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.FragileModifierIcon;
    public Color ModifierColor => TownOfExtraColours.FragileModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.FragileModifierColour;
    
    public static Dictionary<PlayerControl, int> Interactions = new Dictionary<PlayerControl, int>();

    public override string GetDescription()
    {
        return $"You die if you are interacted with {OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileMaxInteractions.Value} times.\n<b>Interactions: {Interactions[PlayerControl.LocalPlayer]}/{OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileMaxInteractions.Value}</b>";
    }

    public string GetAdvancedDescription()
    {
        return $"When you are interacted with, you add one to your total interactions. When they reach {OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileMaxInteractions.Value}, you die.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.FragileChance;
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