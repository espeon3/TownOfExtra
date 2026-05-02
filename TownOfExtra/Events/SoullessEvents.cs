using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfUs.Modules;

namespace TownOfExtra.Events;

public static class SoulLessEvents
{
    private static readonly HashSet<byte> SoullessKills = new();
    
    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        var target = @event.Target;
        var source = @event.Source;

        if (SoullessKills.Contains(target.PlayerId))
        {
            SoullessKills.Remove(target.PlayerId);
            return;
        }

        if (target.HasModifier<SoullessModifier>() && !MeetingHud.Instance)
        {
            @event.Cancel();

            SoullessKills.Add(target.PlayerId);
            source.RpcCustomMurder(target, MeetingCheck.OutsideMeeting, createDeadBody: false);
        }
    }
    
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        var target = @event.Target;

        if (target.HasModifier<SoullessModifier>() && !MeetingHud.Instance)
        {
            _ = new FakePlayer(target);
        }
    }
}