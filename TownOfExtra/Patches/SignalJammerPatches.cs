using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using UnityEngine;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
public static class SjStartMeetingPatch
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (__instance.HasModifier<SignalJammedModifier>())
        {
            __instance.RpcSendNotification(
                $"Your meeting signals are {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
                "SignalJammerRoleIcon",
                "ImpRoleIcon",
                200,
                flashColour: Palette.ImpostorRed
            );
            
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
        if (__instance.HasModifier<SignalJammedModifier>())
        {
            __instance.RpcSendNotification(
                $"Your meeting signals are {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
                "SignalJammerRoleIcon",
                "ImpRoleIcon",
                200,
                flashColour: Palette.ImpostorRed
            );

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