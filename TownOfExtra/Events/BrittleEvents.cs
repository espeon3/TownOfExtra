using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Hud;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Networking;

namespace TownOfExtra.Events;

public static class BrittleEvents
{
    [RegisterEvent]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent e)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return;
        }

        var button = e.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;

        if (target == null || !button.CanClick())
        {
            return;
        }

        BrittleRpcs.RpcTriggerBrittleModifier(target);
    }

    [RegisterEvent]
    public static void KillButtonClickEventHandler(BeforeMurderEvent e)
    {
        BrittleRpcs.RpcTriggerBrittleModifier(e.Target);
    }

    [RegisterEvent]
    public static void GameEndEventHandler(GameEndEvent e)
    {
        BrittleModifier.Interactions = new Dictionary<PlayerControl, int>();
    }
}