using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Impostor.Power;

namespace TownOfExtra.Events;

public class EraserEvents
{
    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PendingEraseModifier>() && !p.Data.IsDead && !p.Data.Disconnected)
            {
                EraserRole.ErasedPlayerRoles.Add(p, p.Data.Role.Role);
                
                if (!AmongUsClient.Instance.AmHost) continue;
                
                p.RpcSetRole(RoleTypes.Crewmate, true);
                EraserRpcs.RpcNotifyErased(p);
                p.RpcRemoveModifier<PendingEraseModifier>();
            }
        }
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        EraserRole.ErasedPlayerRoles.Remove(e.Target);
    }
}