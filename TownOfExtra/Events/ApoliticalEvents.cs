using System.Linq;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfExtra.Options;

namespace TownOfExtra.Events;

public class ApoliticalEvents
{
    [RegisterEvent]
    public static void OnProcessVotes(ProcessVotesEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<ApoliticalModifier>()) return;
        ApoliticalModifier.CdIncrease +=
            (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.ApoliticalCdIncrease *
            e.Votes.Count(v => v.Suspect == PlayerControl.LocalPlayer.PlayerId);
    }

    [RegisterEvent]
    public static void OnMiraButtonPress(MiraButtonClickEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<ApoliticalModifier>()) return;

        var btn = e.Button;
        if (btn.Timer !> 0) return;
        if (btn.Cooldown !>= 0 || btn.EffectActive) return;
        btn.SetTimer(btn.Timer + ApoliticalModifier.CdIncrease);
    }

    [RegisterEvent]
    public static void AfterMurder(AfterMurderEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<ApoliticalModifier>()) return;
        
        e.Source.SetKillTimer(e.Source.killTimer + ApoliticalModifier.CdIncrease);
    }

    [RegisterEvent]
    public static void OnMeetingStart(StartMeetingEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<ApoliticalModifier>()) return;
        
        ApoliticalModifier.CdIncrease = 0;
    }

    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<ApoliticalModifier>()) return;
        
        foreach (var btn in CustomButtonManager.Buttons)
        {
            if (btn.Timer !> 0) return;
            if (btn.Cooldown !>= 0 || btn.EffectActive) return;
            btn.SetTimer(btn.Timer + ApoliticalModifier.CdIncrease);
        }

        PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.LocalPlayer.killTimer + ApoliticalModifier.CdIncrease);
    }
}