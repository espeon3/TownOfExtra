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

public class BrittleModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Brittle";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => $"You die when when interacted with {OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleMaxInteractions.Value} times";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.BrittleModifierIcon;
    public Color ModifierColor => TownOfExtraColours.BrittleModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.BrittleModifierColour;
    
    public static Dictionary<PlayerControl, int> Interactions = new Dictionary<PlayerControl, int>();

    public override string GetDescription()
    {
        return $"You die if you are interacted with {OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleMaxInteractions.Value} times.\n<b>Interactions: {Interactions[PlayerControl.LocalPlayer]}/{OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleMaxInteractions.Value}</b>";
    }

    public string GetAdvancedDescription()
    {
        return $"When you are interacted with, you add one to your total interactions. When they reach {OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleMaxInteractions.Value}, you die.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleChance;
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