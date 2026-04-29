using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
public static class SjStartMeetingPatch
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (__instance.AmOwner && __instance.HasModifier<SignalJammedModifier>())
        {
            SignalJammerPatches.SendJammedWarning();
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
public static class SjReportDeadBodyPatch
{
    public static bool Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
    {
        if (__instance.AmOwner && __instance.HasModifier<SignalJammedModifier>())
        {
            SignalJammerPatches.SendJammedWarning();

            if (target != null)
            {
                var body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                if (body != null)
                {
                    body.Reported = false;
                }
            }
            return false;
        }
        return true;
    }
}

public class SignalJammerPatches
{
    public static void SendJammedWarning()
    {
        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
        var notif = Helpers.CreateAndShowNotification(
            $"Your meeting signals are {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SignalJammerJamButton.LoadAsset());
        notif.AdjustNotification();
    }
}