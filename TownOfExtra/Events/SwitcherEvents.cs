using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class SwitcherEvents
{
    [RegisterEvent(1)]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        if (!AmongUsClient.Instance.AmHost) return;
        
        var switcher = GetSwitcher();

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<SwitchedModifier>())
            {
                if (switcher == null || p == null || p.Data.IsDead || switcher.Data.IsDead)
                {
                    if (switcher != null)
                    {
                        switcher.RpcSendNotification(
                            $"Your {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color> target is no longer alive!",
                            "SwitcherRoleIcon",
                            356,
                            TownOfExtraColours.SwitcherRoleColour
                        );
                    }

                    return;
                }
                if (switcher.HasModifier<ErasedModifier>() || switcher.HasModifier<PendingEraseModifier>() || p.HasModifier<ErasedModifier>() || p.HasModifier<PendingEraseModifier>())
                {
                    if (switcher != null)
                    {
                        switcher.RpcSendNotification(
                            $"You or your {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color> target's role was erased!",
                            "SwitcherRoleIcon",
                            356,
                            TownOfExtraColours.SwitcherRoleColour
                        );
                    }

                    return;
                }

                var pRole = p.Data.Role.Role;

                p.RpcRemoveModifier<ImitatorCacheModifier>();
                p.RpcChangeRole(RoleId.Get<SwitcherRole>());
                switcher.RpcSetRole(pRole, true);
                p.RpcRemoveModifier<SwitchedModifier>();

                p.RpcSendNotification(
                    $"Your role has been switched with the {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color>!",
                    "SwitcherRoleIcon",
                    356,
                    TownOfExtraColours.SwitcherRoleColour
                );
                switcher.RpcSendNotification(
                    $"You have {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switched</color> your role with {p.name}!",
                    "SwitcherRoleIcon",
                    356,
                    TownOfExtraColours.SwitcherRoleColour
                );
            }
        }
    }

    public static PlayerControl GetSwitcher()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is SwitcherRole)
            {
                return p;
            }
        }

        return null;
    }
}