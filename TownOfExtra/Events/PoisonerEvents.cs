using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;

namespace TownOfExtra.Events;

public class PoisonerEvents
{
    [RegisterEvent]
    public static void MeetingStartEvent(StartMeetingEvent e)
    {
        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PoisonedModifier>())
            {
                var modifier = p.GetModifier<PoisonedModifier>();
                if (modifier == null)
                {
                    return;
                }
                
                modifier.StopTimer();
            }
        }
    }
}