using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Networking;

namespace TownOfExtra.Events;

public class EraserEvents
{
    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (!AmongUsClient.Instance.AmHost) return;
        
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PendingEraseModifier>() && !p.Data.IsDead && !p.Data.Disconnected)
            {
                p.RpcSetRole(RoleTypes.Crewmate);
                EraserRpcs.RpcNotifyErased(p);
                p.RpcRemoveModifier<PendingEraseModifier>();
                p.RpcAddModifier<ErasedModifier>();
            }
        }
    }
}