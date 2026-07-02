using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Events;

public class BarbarianEvents
{
    [RegisterEvent]
    public static void OnTargetDeath(AfterMurderEvent e)
    {
        if (e.Source.Data.Role is BarbarianRole) return;
        if (!e.Target.HasModifier<BarbarianTargetModifier>()) return;
        
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is not BarbarianRole) continue;
            
            BarbarianRpcs.RpcNotifyBarbarianOfTargetDeath(p, e.Target.Data.PlayerName);
        }
    }
}