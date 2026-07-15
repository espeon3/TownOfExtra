using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfUs.Utilities;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerRoleTextExtensions), nameof(PlayerRoleTextExtensions.UpdateStatusSymbols), typeof(string), typeof(PlayerControl), typeof(DataVisibility))]
public static class PlayerRoleTextExtensionsPatch
{
    public static void Postfix(ref string __result, PlayerControl player)
    {
        if (player.TryGetModifier<YouthlingModifier>(out var youthling))
        {
            __result += $"{TownOfExtraColours.YouthlingModifierColour.ToTextColor()} ({youthling.Age})</color>";
        }
    }
}