using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Networking;
using TownOfExtra.Options;
using UnityEngine;

namespace TownOfExtra.Events;

public class ClumsyEvents
{
    private static readonly Dictionary<SystemTypes, SystemTypes> SkeldSabos = new()
    {
        [SystemTypes.Reactor] = SystemTypes.Reactor,
        [SystemTypes.LifeSupp] = SystemTypes.LifeSupp,
        [SystemTypes.Admin] = SystemTypes.LifeSupp,
        [SystemTypes.Comms] = SystemTypes.Comms,
        [SystemTypes.Electrical] = SystemTypes.Electrical,
    };

    private static readonly Dictionary<SystemTypes, SystemTypes> MiraSabos = new()
    {
        [SystemTypes.Reactor] = SystemTypes.Reactor,
        [SystemTypes.Greenhouse] = SystemTypes.LifeSupp,
        [SystemTypes.Comms] = SystemTypes.Comms,
        [SystemTypes.Office] = SystemTypes.Electrical,
    };

    private static readonly Dictionary<SystemTypes, SystemTypes> PolusSabos = new()
    {
        [SystemTypes.Laboratory] = SystemTypes.Reactor,
        [SystemTypes.Comms] = SystemTypes.Comms,
        [SystemTypes.Electrical] = SystemTypes.Electrical,
    };

    private static readonly Dictionary<SystemTypes, SystemTypes> AirshipSabos = new()
    {
        [SystemTypes.GapRoom] = SystemTypes.Reactor,
        [SystemTypes.Comms] = SystemTypes.Comms,
        [SystemTypes.ViewingDeck] = SystemTypes.Electrical,
        [SystemTypes.CargoBay] = SystemTypes.Electrical,
    };

    private static readonly Dictionary<SystemTypes, SystemTypes> FungleSabos = new()
    {
        [SystemTypes.Reactor] = SystemTypes.Reactor,
        [SystemTypes.Comms] = SystemTypes.Comms,
        [SystemTypes.Lookout] = SystemTypes.Comms,
        [SystemTypes.Greenhouse] = SystemTypes.MushroomMixupSabotage,
        [SystemTypes.Laboratory] = SystemTypes.MushroomMixupSabotage,
    };

    [RegisterEvent]
    public static void OnTaskComplete(CompleteTaskEvent e)
    {
        if (e.Player != PlayerControl.LocalPlayer) return;
        if (!PlayerControl.LocalPlayer.HasModifier<ClumsyModifier>()) return;
        if (Random.Range(0f, 100f) > OptionGroupSingleton<CrewmateModifierOptions>.Instance.ClumsySabotageChance.Value) return;
        if (RoomTracker.Instance.LastRoom == null) return;
        
        var saboStatus = ShipStatus.Instance.Systems[SystemTypes.Sabotage].TryCast<SabotageSystemType>();
        if (saboStatus == null) return;
        if (saboStatus.AnyActive) return;

        var maps = (MapNames)GameManager.Instance.LogicOptions.MapId switch
        {
            MapNames.Skeld => SkeldSabos,
            MapNames.MiraHQ => MiraSabos,
            MapNames.Polus => PolusSabos,
            MapNames.Airship => AirshipSabos,
            MapNames.Fungle => FungleSabos,
            _ => SkeldSabos,
        };

        if (!maps.TryGetValue(RoomTracker.Instance.LastRoom.RoomId, out var saboType)) return;
        
        ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, (byte)saboType);
        
        PlayerControl.LocalPlayer.RpcSendNotification(
            $"You tripped and accidentally set off a sabotage in {RoomTracker.Instance.LastRoom.RoomId.ToString()}!",
            "ClumsyModifierIcon",
            "CrewModIcon",
            200,
            TownOfExtraColours.ClumsyModifierColour
        );
    }
}