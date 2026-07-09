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
    public override string IntroInfo => "You can set make others Brittle!";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.EmbrittlementRoleIcon;
    public Color ModifierColor => Pallette.ImpostorRed;
    public override Color FreeplayFileColor => Pallette.ImpostorRed;

    public override string GetDescription()
    {
        return $"You can make others be Brittle!";
    }

    public string GetAdvancedDescription()
    {
        return $"You may give others Brittle for {OptionGroupSingleton<ImpostorModifierOptions>.Instance.BrittleDuration.Value} seconds.";
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
        if (player == null || player.Data == null || player.Data.IsDead || !role.IsImpostor())
            return false;

        return true;
    }
}