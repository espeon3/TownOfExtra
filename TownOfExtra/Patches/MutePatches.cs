using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfExtra.Networking.Global;
using UnityEngine;

namespace TownOfExtra.Patches;

public class MutePatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
    public static class ReportDeadBodyPatch
    {
        public static bool Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
        {
            if (__instance.HasModifier<MuteModifier>())
            {
                __instance.RpcSendNotification(
                    $"You are {TownOfExtraColours.MuteModifierColour.ToTextColor()}mute</color>, you cannot report {Palette.ImpostorRed.ToTextColor()}dead bodies</color>!",
                    "MuteModifierIcon",
                    "UniModIcon",
                    200,
                    flashColour: TownOfExtraColours.MuteModifierColour
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
}