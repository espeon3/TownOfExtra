using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfUs.Modifiers;

namespace TownOfExtra.Patches;

[HarmonyPatch]
public static class ObstructorPatches
{
    [HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
    [HarmonyPatch(typeof(UseButton), nameof(UseButton.DoClick))]
    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.DoClick))]
    [HarmonyPriority(Priority.First)]
    [HarmonyPrefix]
    public static bool ObstructorObstructedSabotageButtonPatch(ActionButton __instance)
    {
        if (PlayerControl.LocalPlayer.HasModifier<ObstructorObstructedModifier>())
        {
            if (!PlayerControl.LocalPlayer.GetModifier<ObstructorObstructedModifier>()!.ShouldHideObstructed)
            {
                return false;
            }
            ObstructorRpcs.RpcTriggerObstruct(PlayerControl.LocalPlayer, false);
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.ToggleMapVisible))]
    [HarmonyPrefix]
    public static bool ObstructorObstructedToggleMapVisiblePatch(HudManager __instance)
    {
        if (PlayerControl.LocalPlayer.HasModifier<ObstructorObstructedModifier>() &&
            !PlayerControl.LocalPlayer.GetModifier<ObstructorObstructedModifier>()!.ShouldHideObstructed)
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.GetModifiers<DisabledModifier>().Any(x => !x.CanOpenMap))
        {
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
    [HarmonyPrefix]
    public static bool ObstructorConsoleCanUsePrefix(Console __instance, NetworkedPlayerInfo pc, ref bool canUse, ref bool couldUse, ref float __result)
    {
        var playerControl = pc?.Object;
        if (playerControl == null) return true;
        if (!playerControl.HasModifier<ObstructorObstructedModifier>()) return true;
        if (playerControl.GetModifier<ObstructorObstructedModifier>()!.ShouldHideObstructed) return true;

        canUse = false;
        couldUse = false;
        __result = float.MaxValue;
        return false;
    }

    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    [HarmonyPrefix]
    public static bool ObstructorMinigameBeginPrefix(Minigame __instance, PlayerTask task)
    {
        var localPlayer = PlayerControl.LocalPlayer;
        if (localPlayer == null) return true;
        if (!localPlayer.HasModifier<ObstructorObstructedModifier>()) return true;

        var mod = localPlayer.GetModifier<ObstructorObstructedModifier>()!;
        if (mod.ShouldHideObstructed)
        {
            ObstructorRpcs.RpcTriggerObstruct(localPlayer, false);
        }

        __instance.Close();
        return false;
    }
}