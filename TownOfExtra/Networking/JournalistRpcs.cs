using System.Linq;
using MiraAPI.Modifiers;
using Reactor.Networking.Attributes;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public class JournalistRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.SendJournalistChat)]
    public static void RpcSendJournalistChat(PlayerControl sender, string text)
    {
        var p = PlayerControl.LocalPlayer;
        var interviewee = ModifierUtils.GetPlayersWithModifier<InterviewModifier>().FirstOrDefault();
        var displayPlayer = interviewee != null ? interviewee : sender;
        
        if (p.Data.Role is not JournalistRole && !p.HasModifier<InterviewModifier>()) return;

        string title = sender.Data.Role is JournalistRole
            ? $"{TownOfExtraColours.JournalistRoleColour.ToTextColor()}Journalist</color>"
            : $"{TownOfExtraColours.JournalistRoleColour.ToTextColor()}{sender.Data.PlayerName} (Interviewee)</color>";

        MiscUtils.AddTeamChat(displayPlayer.Data, title, text, blackoutText: false, bubbleType: BubbleType.Other, onLeft: !sender.AmOwner);
    }
}