using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;

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

                if (p.IsCrewmate())
                {
                    p.RpcRemoveModifier<ImitatorCacheModifier>();
                    p.RpcSetRole(RoleTypes.Crewmate, true);
                }
                else if (p.IsNeutral())
                {
                    if (OptionGroupSingleton<EraserRoleOptions>.Instance.ErasedNeutralRole == ErasedNeutralRole.Amnesiac)
                    {
                        p.RpcChangeRole(RoleId.Get<AmnesiacRole>());
                    }
                    else
                    {
                        p.RpcChangeRole(RoleId.Get<SurvivorRole>());
                    }
                }

                p.RpcSendNotification(
                    $"Your role has been erased by the {Palette.ImpostorRed.ToTextColor()}eraser</color>!",
                    "EraserRoleIcon",
                    "ImpRoleIcon",
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