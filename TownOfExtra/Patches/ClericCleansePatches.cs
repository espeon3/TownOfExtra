/*
 * Divani Mods - https://github.com/DivaniNL/TownOfUsMiraDivaniModsAddOn | Divani - https://github.com/DivaniNL
 * For the code for making cleric's cleansing work on TouEx roles
 */

using System;
using System.Collections.Generic;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfUs.Buttons.Crewmate;

namespace TownOfExtra.Patches;

public static class ClericCleansePatches
{
    private static readonly List<Action<PlayerControl>> Removers =
    [
        // general
        Remover<PoisonedModifier>(),
        Remover<SwitchedModifier>(),
        Remover<ImpendingDoomModifierv>(),
        // dream caster
        Remover<LucidDreamingModifier>(),
        Remover<WaitingOnLcdModifier>(),
        // poltergeist
        Remover<ScaredModifier>(),
        Remover<PossessedModifier>(),
        // eraser
        Remover<PendingEraseModifier>(),
    ];

    public static void CleanseAll(PlayerControl p)
    {
        if (p == null) return;
        
        /*if (EraserRole.ErasedPlayerRoles.ContainsKey(p) && OptionGroupSingleton<EraserRoleOptions>.Instance.CanUnerase)
        {
            p.RpcSetRole(EraserRole.ErasedPlayerRoles[p], true);
            EraserRole.ErasedPlayerRoles.Remove(p);
            p.RpcSendNotification(
                $"Your role has been restored by a {TownOfUsColors.Cleric}Cleric</color>!",
                TouCrewAssets.CleanseSprite.LoadAsset()
            );
        }*/

        foreach (var remove in Removers) remove(p);
    }

    private static Action<PlayerControl> Remover<T>()
        where T : BaseModifier
    {
        return player =>
        {
            if (player.HasModifier<T>())
            {
                player.RpcRemoveModifier<T>();
            }
        };
    }
    
    [HarmonyPatch(typeof(ClericCleanseButton), "OnClick")]
    private static class OnClickPatch
    {
        private static void Postfix(ClericCleanseButton __instance)
        {
            CleanseAll(__instance.Target);
        }
    }
}