using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
public class RtCompleteTaskPatch
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (__instance.AmOwner && __instance.HasModifier<RoutineModifier>())
        {
            if (__instance.HasModifier<RoutineSpeedModifier>())
            {
                var speedModifier = __instance.GetModifier<RoutineSpeedModifier>();
                if (speedModifier != null)
                {
                    speedModifier.ResetTimer();
                    speedModifier.StartTimer();
                }
            }
            else
            {
                __instance.RpcAddModifier<RoutineSpeedModifier>();
            }
        }

        return true;
    }
}