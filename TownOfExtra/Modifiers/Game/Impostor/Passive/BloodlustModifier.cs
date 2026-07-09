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

namespace TownOfExtra.Modifiers.Game.Impostor.Passive;

public class BloodlustModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Bloodlust";
    public override ModifierFaction FactionType => ModifierFaction.ImpostorPassive;
    public override string IntroInfo => "You have a lower kcd, at a cost";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.BloodlustModifierIcon;
    public Color ModifierColor => Palette.ImpostorRed;
    public override Color FreeplayFileColor => Palette.ImpostorRed;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return "Your kill cooldown is shorter, but you cant vent nor sabotage.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ImpostorModifierOptions>.Instance.BloodlustAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<ImpostorModifierOptions>.Instance.BloodlustChance;
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