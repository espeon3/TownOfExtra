using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class ShifterEvents
{
    [RegisterEvent(1)]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        if (!AmongUsClient.Instance.AmHost) return;
        
        var shifter = GetShifter();

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<ShiftedModifier>())
            {
                if (shifter == null || p == null || p.Data.IsDead || shifter.Data.IsDead)
                {
                    if (shifter != null)
                    {
                        shifter.RpcSendNotification(
                            $"Your {TownOfExtraColours.ShifterRoleColour.ToTextColor()}shifter</color> target is no longer alive!",
                            "ShifterRoleIcon",
                            "NeutRoleIcon",
                            flashColour: TownOfExtraColours.ShifterRoleColour
                        );
                    }

                    return;
                }
                if (shifter.HasModifier<ErasedModifier>() || shifter.HasModifier<PendingEraseModifier>() || p.HasModifier<ErasedModifier>() || p.HasModifier<PendingEraseModifier>())
                {
                    if (shifter != null)
                    {
                        shifter.RpcSendNotification(
                            $"You or your {TownOfExtraColours.ShifterRoleColour.ToTextColor()}shifter</color> target's role was erased!",
                            "ShifterRoleIcon",
                            "NeutRoleIcon",
                            flashColour: TownOfExtraColours.ShifterRoleColour
                        );
                    }

                    return;
                }

                var pRole = p.Data.Role.Role;

                p.RpcRemoveModifier<ImitatorCacheModifier>();
                p.RpcChangeRole(RoleId.Get<ShifterRole>());
                shifter.RpcSetRole(pRole, true);
                p.RpcRemoveModifier<ShiftedModifier>();

                p.RpcSendNotification(
                    $"Your role has been shifted with the {TownOfExtraColours.ShifterRoleColour.ToTextColor()}shifter</color>!",
                    "ShifterRoleIcon",
                    "NeutRoleIcon",
                    flashColour: TownOfExtraColours.ShifterRoleColour
                );
                shifter.RpcSendNotification(
                    $"You have {TownOfExtraColours.ShifterRoleColour.ToTextColor()}shifted</color> your role with {p.name}!",
                    "ShifterRoleIcon",
                    "NeutRoleIcon",
                    flashColour: TownOfExtraColours.ShifterRoleColour
                );
            }
        }
    }

    public static PlayerControl GetShifter()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is ShifterRole)
            {
                return p;
            }
        }

        return null;
    }
}