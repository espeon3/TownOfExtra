/*
 * Divani Mods - https://github.com/DivaniNL/TownOfUsMiraDivaniModsAddOn | Divani - https://github.com/DivaniNL
 * For the code for making cleric's cleansing work on TouEx roles
 */

using System;
using System.Collections.Generic;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;

namespace TownOfExtra.Utilities;

public static class NegativeEffects
{
    private static readonly List<Action<PlayerControl>> Removers =
    [
        // general
        Remover<ErasedModifier>(),
        Remover<PoisonedModifier>(),
        Remover<SwitchedModifier>(),
        // dream caster
        Remover<LucidDreamingModifier>(),
        Remover<WaitingOnLcdModifier>(),
        // poltergeist
        Remover<ScaredModifier>(),
        Remover<PossessedModifier>(),
    ];

    public static void CleanseAll(PlayerControl player)
    {
        if (player == null)
        {
            return;
        }

        foreach (var remove in Removers)
        {
            remove(player);
        }
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
}