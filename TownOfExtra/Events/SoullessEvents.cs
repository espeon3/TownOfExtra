using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfUs.Modules;
using UnityEngine;

namespace TownOfExtra.Events;

public static class SoullessEvents
{
    [RegisterEvent(10000)]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        var target = e.Target;
        if (!target.HasModifier<SoullessModifier>()) return;

        if (e.DeadBody != null) Object.Destroy(e.DeadBody.gameObject);
        _ = new FakePlayer(target);
    }
}