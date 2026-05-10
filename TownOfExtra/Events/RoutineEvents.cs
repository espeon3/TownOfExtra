using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;

namespace TownOfExtra.Events;

public class RoutineEvents
{
    [RegisterEvent]
    public static void CompleteTaskEventHandler(CompleteTaskEvent e)
    {
        PlayerControl p = e.Player;
        
        if (p.AmOwner && p.HasModifier<RoutineModifier>())
        {
            if (p.HasModifier<RoutineSpeedModifier>())
            {
                var speedModifier = p.GetModifier<RoutineSpeedModifier>();
                if (speedModifier != null)
                {
                    speedModifier.ResetTimer();
                    speedModifier.StartTimer();
                }
            }
            else
            {
                p.RpcAddModifier<RoutineSpeedModifier>();
            }
        }
    }
}