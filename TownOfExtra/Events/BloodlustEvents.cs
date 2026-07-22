using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Map;
using MiraAPI.GameOptions;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;

namespace TownOfExtra.Events;

public class BloodlustEvents
{
    [RegisterEvent]
    public static void UpdateSystemEventHandler(UpdateSystemEvent e)
    {
        if (e.Player.Data.Role is BloodlustRole && !OptionGroupSingleton<BloodlustRoleOptions>.Instance.CanSabotage)
            e.Cancel();
    }
}