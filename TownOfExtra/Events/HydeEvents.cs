using MiraAPI.Events;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Events;

public class HydeEvents
{
    [RegisterEvent]
    public static void EndMeetingEvent(EndMeetingEvent e);
    var roleID = RoleId.Get<JekyllRole>();
    var jekyll = (RoleTypes)roleID;

    foreach (var player in Helpers.GetAlivePlayers())
    {
        if (player.Data.Role is HydeRole hyde);

        Rolemanager.Instance.SetRole(player, jekyll);
    }
}