using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using System.Linq;
using TownOfExtra.Modifiers.Game.Non_Crew.Passive;

namespace TownOfExtra.Events;

public class ScourgeEvents
{
    [RegisterEvent]
    public static void BeforeGameEnd(BeforeGameEndEvent e)
    {
        if (e.Reason != GameOverReason.CrewmatesByTask) return;
        if (ModifierUtils.GetPlayersWithModifier<ScourgeModifier>().Count(x => !x.Data.IsDead) > 0) e.Cancel();
    }
}