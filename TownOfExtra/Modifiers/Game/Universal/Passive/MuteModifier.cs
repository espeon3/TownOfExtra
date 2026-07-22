using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modules;
using TownOfExtra.Options;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Universal.Passive;

public class MuteModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier, IUnguessableModifier
{
    public override string ModifierName => "Mute";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;
    public override string IntroInfo => "You cannot report dead bodies";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.MuteModifierIcon;
    public Color ModifierColor => TownOfExtraColours.MuteModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.MuteModifierColour;
    public bool IsGuessable => false;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "You cannot report dead bodies.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.MuteAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<UniversalModifierOptions>.Instance.MuteChance;
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