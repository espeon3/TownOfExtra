using MiraAPI.Events;
using TownOfExtra.Roles.Crewmate.Power;

namespace TownOfExtra.Events;

public class JekyllEvents
{
    [RegisterEvent]
    public static void EndMeetingEvent(EndMeetingEvent e);
    var roleID = RoleId.Get<HydeRole>();
    var hyde = (RoleTypes)roleID;

    foreach (var player in Helpers.GetAlivePlayers())
    {
        if (player.Data.Role is JekyllRole jekyll);

        Rolemanager.Instance.SetRole(player, hyde);
    }
}