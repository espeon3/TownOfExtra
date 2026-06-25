using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Crewmate.Passive;

public class ClumsyModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Clumsy";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => "You occasionally start sabotages when doing tasks";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.ClumsyModifierIcon;
    public Color ModifierColor => TownOfExtraColours.ClumsyModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.ClumsyModifierColour;

    public override string GetDescription() => IntroInfo;

    public string GetAdvancedDescription()
    {
        return
            $"Whenever you complete a task, there is a {OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsySabotageChance.Value}% chance for a sabotage to be called in your current room (if there is one for that room).\n\n" +
            $"<b>{TownOfUsColors.Vigilante.ToTextColor()}Rooms:</color></b>\n" +
            "<b>Skeld:</b>\n" +
            "Reactor = Reactor\n" +
            "O2/Admin = O2\n" +
            "Comms = Comms\n" +
            "Electrical = Lights\n" +
            "<b>Mira HQ:</b>\n" +
            "Reactor = Reactor\n" +
            "Greenhouse = O2\n" +
            "Comms = Comms\n" +
            "Office = Lights\n" +
            "<b>Polus</b>\n" +
            "Laboratory = Reactor\n" +
            "Comms = Comms\n" +
            "Electrical = Lights\n" +
            "<b>Airship:</b>\n" +
            "Gap Room = Reactor\n" +
            "Comms = Comms\n" +
            "Viewing Deck/Cargo Bay = Lights\n" +
            "<b>Fungle:</b>\n" +
            "Reactor = Reactor\n" +
            "Comms/Lookout = Comms\n" +
            "Greenhouse = Mushroom Mixup\n" +
            "Laboratory = Mushroom Mixup\n";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsyAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsyChance;
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