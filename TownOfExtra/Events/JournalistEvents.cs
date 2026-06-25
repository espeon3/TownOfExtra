using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Crewmate.Power;

namespace TownOfExtra.Events;

public class JournalistEvents
{
    [RegisterEvent]
    public static void RoundStartHandler(RoundStartEvent e)
    {
        var p = PlayerControl.LocalPlayer;

        ModifierUtils.GetActiveModifiers<InterviewModifier>().Do(x => x.OnRoundStart());
        CustomRoleUtils.GetActiveRolesOfType<JournalistRole>().Do(x => x.OnRoundStart());

        if (p.HasModifier<InterviewModifier>())
        {
            p.RpcSendNotification(
                $"You are being {TownOfExtraColours.JournalistRoleColour.ToTextColor()}interviewed</color> by the {TownOfExtraColours.JournalistRoleColour.ToTextColor()}journalist</color>, talk to them in the chat!",
                "JournalistInterviewButton",
                "CrewButton",
                flashColour: TownOfExtraColours.JournalistRoleColour
            );
        }
    
        if (!AmongUsClient.Instance.AmHost) return;
    
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (!player.TryGetModifier<InterviewModifier>(out var mod)) continue;

            if (mod.Active) player.RpcRemoveModifier<InterviewModifier>();
            else mod.Active = true;
        }
    }
}