using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Usables;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Events;

public static class ClownEvents
{
    [RegisterEvent]
    public static void PlayerCanUseEventHandler(PlayerCanUseEvent @event)
    {
        if (!@event.IsVent)
        {
            return;
        }

        var vent = @event.Usable.TryCast<Vent>();

        if (vent == null)
        {
            return;
        }

        if (vent.name.Contains("ClownJackInTheBox") && PlayerControl.LocalPlayer.Data.Role is not ClownRole)
        {
            @event.Cancel();
        }
        if (PlayerControl.LocalPlayer.Data.Role is ClownRole && !vent.name.Contains("ClownJackInTheBox"))
        {
            @event.Cancel();
        }
    }
}