using System.Collections;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Networking;
using TownOfUs;
using TownOfUs.Modifiers;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class ObservantEvents
{
    [RegisterEvent]
    public static void OnMeetingDeath(AfterMurderEvent e)
    {
        if (!MeetingHud.Instance) return;
        if (!PlayerControl.LocalPlayer.HasModifier<ObservantModifier>()) return;

        Coroutines.Start(CoWaitForDeathHandler(e.Target, e.Source));
    }

    private static IEnumerator CoWaitForDeathHandler(PlayerControl target, PlayerControl source)
    {
        while (DeathHandlerModifier.IsAltCoroutineRunning || DeathHandlerModifier.IsCoroutineRunning)
        {
            yield return null;
        }

        if (!target.TryGetModifier<DeathHandlerModifier>(out var deathHandler)) yield break;
        
        var cod = deathHandler.CauseOfDeath == "Killed" ? "Guessed" : deathHandler.CauseOfDeath;

        var title = $"{TownOfExtraColours.ObservantModifierColour.ToTextColor()}Observations</color>";
        var startTxt =
            cod == "Misguessed" 
                ? $"{target.Data.PlayerName} has" 
                : $"{target.Data.PlayerName} has been";
        var endTxt = 
            source.Data.Role is VigilanteRole
                ? $"{TownOfUsColors.Vigilante.ToTextColor()}<b>{cod}</b></color>"
                : $"<b>{cod}</b>";
        var msg = $"{startTxt} {endTxt}!";
        
        MiscUtils.AddFakeChat(PlayerControl.LocalPlayer.Data, title, msg, false, true);
        PlayerControl.LocalPlayer.RpcSendNotification(
            msg,
            "ObservantModifierIcon",
            "CrewModIcon",
            200
            );

        if (!HudManager.Instance.Chat.IsOpenOrOpening) HudManager.Instance.Chat.Toggle();
    }
}