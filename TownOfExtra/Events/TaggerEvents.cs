using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Events;

public class TaggerEvents
{
    [RegisterEvent]
    public static void GameEndEventHandler(GameEndEvent e)
    {
        TaggerRole.MarkedPlayers = new List<PlayerControl>();
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        TaggerRole.MarkedPlayers.Remove(e.Target);
    }
}