using System.Collections.Generic;
using System.Linq;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Impostor.Passive;
using TownOfExtra.Networking;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class RebirthEvents
{
    public static List<PlayerControl> Exclusions = new List<PlayerControl>();
    
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        if (!PlayerControl.LocalPlayer.HasModifier<RebirthModifier>()) return;
        if (RebirthModifier.Used) return;

        List<PlayerControl> deadImps = new List<PlayerControl>();
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.IsImpostor() && p.Data.IsDead && !Exclusions.Contains(p) && p != PlayerControl.LocalPlayer) deadImps.Add(p);
        }

        PlayerControl chosen = deadImps.FirstOrDefault();
        if (chosen == null) return;
        Exclusions.Add(chosen);
        RebirthRpcs.RpcSendRebirthPopup(PlayerControl.LocalPlayer, chosen);
    }

    [RegisterEvent]
    public static void OnGameStart(IntroEndEvent e)
    {
        RebirthModifier.Used = false;
        Exclusions = new List<PlayerControl>();
    }
}
