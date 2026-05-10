using System.Collections.Generic;
using System.Linq;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Buttons;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Events;

public class TricksterEvents
{
    [RegisterEvent]
    public static void ReportDeadBodyEventHandler(ReportBodyEvent e)
    {
        if (e.Reporter.Data.IsDead) return;
        if (e.Target == null || e.Body == null) return;

        TricksterRole.SpawnedBodies.RemoveAll(b => b == null);
        bool isFake = TricksterRole.SpawnedBodies.Any(b =>
            b.ParentId == e.Target.PlayerId
        );

        if (isFake)
        {
            if (e.Reporter.IsRole<TricksterRole>())
            {
                BodyManager.ClearFakeBodies(e.Target);
                
                Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                var othernotif = Helpers.CreateAndShowNotification(
                    $"You cannot report your own {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>!",
                    Palette.ImpostorRed, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                othernotif.AdjustNotification();
                e.Cancel();
            }

            if (e.Reporter.IsLover())
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player == null) continue;

                    if (player.IsRole<TricksterRole>() && player.IsLover())
                    {
                        BodyManager.ClearFakeBodies(e.Target);
                        
                        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                        var othernotif = Helpers.CreateAndShowNotification(
                            $"You cannot report your lover's {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>!",
                            Palette.ImpostorRed, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                        othernotif.AdjustNotification();
                        e.Cancel();
                    }
                }
            }
            
            TricksterRole.FakeBodiesReported++;
            
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.TricksterRoleColour));
            var notif = Helpers.CreateAndShowNotification(
                $"Something about that body felt {TownOfExtraColours.TricksterRoleColour.ToTextColor()}wrong</color>...",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
            notif.AdjustNotification();

            BodyManager.ClearFakeBodies(e.Target);

            e.Cancel();
        }
    }

    [RegisterEvent]
    public static void StartGameEventHandler(IntroBeginEvent e)
    {
        TricksterRole.FakeBodiesReported = 0;
        TricksterRole.SpawnedBodies = new List<DeadBody>();
        TricksterRole.SampledColourId = 0;
        TricksterRole.HasSampledColour = false;
        TricksterPlaceButton.BodyPlaced = false;
    }

    [RegisterEvent]
    public static void MeetingStartEventHandler(StartMeetingEvent e)
    {
        TricksterRole.SpawnedBodies = new List<DeadBody>();
        TricksterRole.SampledColourId = 0;
        TricksterRole.HasSampledColour = false;
    }
}

public static class BodyManager
{
    public static void ClearFakeBodies(NetworkedPlayerInfo target)
    {
        if (target.Object == null) return;
        
        var bodies = TricksterRole.SpawnedBodies
            .Where(b => b != null && b.ParentId == target.PlayerId)
            .ToList();
        foreach (var body in bodies)
        {
            TricksterRpcs.RpcDestroyFakeBodies(target.Object, body.ParentId);
        }

        TricksterPlaceButton.Instance.Timer = OptionGroupSingleton<TricksterRoleOptions>.Instance.PlaceCooldown;
        TricksterPlaceButton.BodyPlaced = false;
    }
}