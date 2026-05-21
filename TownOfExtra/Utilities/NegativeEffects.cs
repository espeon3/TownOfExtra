using System;
using System.Collections.Generic;
using System.Linq;
using MiraAPI.Modifiers;

namespace TownOfExtra.Utilities;

public static class NegativeEffects
{
    private static readonly List<Action<PlayerControl, ModifierComponent>> Removers =
    [
        // general
        Remover<Modifiers.ErasedModifier>(),
        Remover<Modifiers.PoisonedModifier>(),
        Remover<Modifiers.SwitchedModifier>(),
        // lucid dreams
        Remover<Modifiers.LucidDreamingModifier>(),
        Remover<Modifiers.WaitingOnLcdModifier>(),
    ];

    public static void CleanseAll(PlayerControl player)
    {
        if (player == null)
        {
            return;
        }

        var comp = player.GetModifierComponent();
        if (comp == null)
        {
            return;
        }

        foreach (var remove in Removers)
        {
            remove(player, comp);
        }
    }

    private static Action<PlayerControl, ModifierComponent> Remover<T>()
        where T : BaseModifier
    {
        return (player, comp) =>
        {
            foreach (var modifier in player.GetModifiers<T>().ToArray())
            {
                comp.RemoveModifier(modifier);
            }
        };
    }
}