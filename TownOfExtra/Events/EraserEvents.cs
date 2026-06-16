using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Modifiers.Crewmate;

namespace TownOfExtra.Events;

public class EraserEvents
{
    [RegisterEvent(1005)]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PendingEraseModifier>() && !p.Data.IsDead && !p.Data.Disconnected)
            {
                EraserRole.ErasedPlayerRoles.Add(p, p.Data.Role.Role);
                
                if (!AmongUsClient.Instance.AmHost) continue;
                
                p.RpcRemoveModifier<ImitatorCacheModifier>();
                p.RpcSetRole(RoleTypes.Crewmate, true);
                
                p.RpcSendNotification(
                    $"Your role has been erased by the {Palette.ImpostorRed.ToTextColor()}eraser</color>!",
                    "EraserRoleIcon",
                    200,
                    Palette.ImpostorRed
                );
                
                p.RpcRemoveModifier<PendingEraseModifier>();
                p.RpcAddModifier<ErasedModifier>();
            }
        }
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        EraserRole.ErasedPlayerRoles.Remove(e.Target);
    }
}