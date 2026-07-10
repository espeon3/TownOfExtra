using System.Linq;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Roles.Crewmate;
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
            if (p.HasModifier<WaitingOnShiftModifier>())
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

                if (shifter.HasModifier<ErasedModifier>() || shifter.HasModifier<PendingEraseModifier>() ||
                    p.HasModifier<ErasedModifier>() || p.HasModifier<PendingEraseModifier>())
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

                if (!p.HasModifier<ImitatorCacheModifier>()) shifter.RpcSetRole(pRole, true);
                else shifter.RpcChangeRole(RoleId.Get<ImitatorRole>());

                p.RpcRemoveModifier<ImitatorCacheModifier>();
                p.RpcRemoveModifier<WaitingOnShiftModifier>();
                p.AddModifier<PreviouslyShiftedModifier>();
                p.RpcChangeRole(RoleId.Get<ShifterRole>());

                if (OptionGroupSingleton<ShifterRoleOptions>.Instance.ShiftModifiers)
                {
                    var pGameMods = p.GetModifiers<TouGameModifier>().ToList();
                    var shifterGameMods = shifter.GetModifiers<TouGameModifier>().ToList();
                    var pAllianceMods = p.GetModifiers<AllianceGameModifier>().ToList();
                    var shifterAllianceMods = shifter.GetModifiers<AllianceGameModifier>().ToList();

                    PlayerControl pLover = p.TryGetModifier<LoverModifier>(out var plvr) ? plvr.OtherLover : null;
                    PlayerControl shifterLover = shifter.TryGetModifier<LoverModifier>(out var slvr) ? slvr.OtherLover : null;

                    foreach (var mod in pGameMods) p.RpcRemoveModifier(mod.GetType());
                    foreach (var mod in shifterGameMods) shifter.RpcRemoveModifier(mod.GetType());
                    foreach (var mod in pAllianceMods) p.RpcRemoveModifier(mod.GetType());
                    foreach (var mod in shifterAllianceMods) shifter.RpcRemoveModifier(mod.GetType());

                    foreach (var mod in pGameMods) shifter.RpcAddModifier(mod.GetType());
                    foreach (var mod in shifterGameMods) p.RpcAddModifier(mod.GetType());
                    foreach (var mod in pAllianceMods.Where(m => m is not LoverModifier)) shifter.RpcAddModifier(mod.GetType());
                    foreach (var mod in shifterAllianceMods.Where(m => m is not LoverModifier)) p.RpcAddModifier(mod.GetType());

                    if (pAllianceMods.Any(m => m is LoverModifier)) Loverify(newLover: shifter, otherLover: pLover);
                    if (shifterAllianceMods.Any(m => m is LoverModifier))  Loverify(newLover: p, otherLover: shifterLover);
                }

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
    
    private static void Loverify(PlayerControl newLover, PlayerControl otherLover)
    {
        var loverMod = newLover.AddModifier<LoverModifier>();
        if (loverMod == null) return;

        loverMod.OtherLover = otherLover;

        otherLover.TryGetModifier<LoverModifier>(out var otherLoverMod);
        if (otherLoverMod == null) return;
        
        otherLoverMod.OtherLover = newLover;
    }
}