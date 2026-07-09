using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;

namespace TownOfExtra.Events;

public class EmbrittlementEvents
{
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        if (!AmongUsClient.Instance.AmHost) return;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (!p.HasModifier<WaitingOnBrittleModifier>()) continue;
            
            p.RpcRemoveModifier<WaitingOnBrittleModifier>();
            p.RpcAddModifier<BrittleModifier>();
        }
    }
}