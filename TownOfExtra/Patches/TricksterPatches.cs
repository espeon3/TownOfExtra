using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
public static class ReportDeadBodyPatch
{
    public static bool Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
    {
        if (__instance.Data.IsDead) return false;
        
        TricksterRole.SpawnedBodies.RemoveAll(b => b == null);
        bool isFake = TricksterRole.SpawnedBodies.Any(b =>
            b.ParentId == target.PlayerId
        );

        if (isFake)
        {
            if (__instance.IsRole<TricksterRole>())
            {
                BodyManager.ClearFakeBodies(target);
                
                Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                var othernotif = Helpers.CreateAndShowNotification(
                    $"You cannot report your own {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>!",
                    Palette.ImpostorRed, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                othernotif.AdjustNotification();
                return false;
            }

            if (__instance.IsLover())
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player == null) continue;

                    if (player.IsRole<TricksterRole>() && player.IsLover())
                    {
                        BodyManager.ClearFakeBodies(target);
                        
                        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                        var othernotif = Helpers.CreateAndShowNotification(
                            $"You cannot report your lover's {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>!",
                            Palette.ImpostorRed, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                        othernotif.AdjustNotification();
                        return false;
                    }
                }
            }
            
            TricksterRole.FakeBodiesReported++;
            
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.TricksterRoleColour));
            var notif = Helpers.CreateAndShowNotification(
                $"Something about that body felt {TownOfExtraColours.TricksterRoleColour.ToTextColor()}wrong</color>...",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
            notif.AdjustNotification();

            BodyManager.ClearFakeBodies(target);

            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
public static class StartGamePatch
{
    public static bool Prefix()
    {
        TricksterRole.FakeBodiesReported = 0;
        TricksterRole.SpawnedBodies = new List<DeadBody>();
        TricksterRole.SampledColourId = 0;
        TricksterRole.HasSampledColour = false;
        return true;
    }
}

public static class BodyManager
{
    public static void ClearFakeBodies(NetworkedPlayerInfo target)
    {
        foreach (var body in 
                 TricksterRole.SpawnedBodies
                     .Where(b => b != null && b.ParentId == target.PlayerId)
                     .ToList())
        {
            Object.Destroy(body.gameObject);
        }
        TricksterRole.SpawnedBodies.RemoveAll(b => b == null || b.ParentId == target.PlayerId);
    }
}