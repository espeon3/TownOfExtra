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

namespace TownOfExtra.Modifiers.Game.Impostor.Utility;

public class EmbrittlementModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Embrittlement";
    public override ModifierFaction FactionType => ModifierFaction.ImpostorUtility;
    public override string IntroInfo => "You can make someone brittle";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.EmbrittlementModifierIcon;
    public Color ModifierColor => TownOfExtraColours.EmbrittlementModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.EmbrittlementModifierColour;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "You can give one person the Brittle modifier, applies after the next meeting.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.EmbrittlementAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<ImpostorModifierOptions>.Instance.EmbrittlementChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || !player.IsImpostor())
            return false;

        return true;
    }
}