using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using Reactor.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfUs;
using TownOfUs.Modules;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class ChiefEvents
{
    [RegisterEvent]
    public static void OnGameStart(IntroEndEvent e)
    {
        ChiefRole.Recruits.Clear();
        ChiefRole.ShotPlayers.Clear();
    }

    [RegisterEvent]
    public static void AfterKillEventHandler(AfterMurderEvent e)
    {
        if (e.Source.Data.Role is not ChiefRole) return;

        var t = e.Target;
        var p = e.Source;
        
        string c = TownOfUsColors.Crewmate.ToTextColor();
        
        if (t.IsCrewmate())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Crewmate));
        }
        else if (t.IsNeutral())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Neutral));
            c = TownOfUsColors.Neutral.ToTextColor();
        }
        else if (t.IsImpostor())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor));
            c = TownOfUsColors.Impostor.ToTextColor();
        }
        
        p.RpcSendNotification(
            $"{c}You have shot {t.Data.PlayerName} and their role was {TownOfExtraColours.GetRoleColour(t.GetRoleWhenAlive().NiceName).ToTextColor()}{t.GetRoleWhenAlive().NiceName}{c}!",
            "ChiefShootButton",
            "CrewButton"
        );

        ChiefRole.ShotPlayers.Add(t);
    }
}