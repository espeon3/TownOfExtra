using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Networking;

namespace TownOfExtra.Events;

public static class SoullessEvents
{
    public static Dictionary<PlayerControl, bool> NeedFakePlayer = new Dictionary<PlayerControl, bool>();
    
    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent e)
    {
        var target = e.Target;

        if (NeedFakePlayer.ContainsKey(target)) return;
        if (!target.HasModifier<SoullessModifier>()) return;
        if (target.HasModifier<BaseShieldModifier>()) return;
        
        NeedFakePlayer.Add(target, true);
        e.Source.RpcSpecialMurder(target, createDeadBody: false);
        target.RpcRemoveModifier<SoullessModifier>();
        e.Cancel();
    }
    
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        var target = e.Target;

        if (!NeedFakePlayer.ContainsKey(target)) return;
        _ = new FakePlayer(target);
        NeedFakePlayer.Remove(target);
    }
}