using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Networking.Global;
using TownOfExtra.Roles.Crewmate.Killing;

namespace TownOfExtra.Events;

public class CommanderEvents
{
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        var t = e.Target;

        if (t.HasModifier<BrawlerModifier>())
        {
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.Data.Role is not CommanderRole) continue;
                
                p.RpcIncreaseAvengeUses();
                
                var clr = TownOfExtraColours.CommanderRoleColour.ToTextColor();
                p.RpcSendNotification(
                    $"One of your {clr}brawlers</color>, {t.Data.PlayerName}, has been killed. You can now {clr}avenge them</color>!",
                    "CommanderAvengeButton",
                    "CrewButton",
                    flashColour: TownOfExtraColours.CommanderRoleColour
                );
            }
        }
    }
}