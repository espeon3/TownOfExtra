using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Buttons;
using TownOfExtra.Networking;
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

        bool isFake = false;
        foreach (var body in TricksterRole.SpawnedBodies)
        {
            if (body == e.Body) isFake = true;
            else isFake = false;
        }

        if (isFake)
        {
            e.Cancel();
            
            if (e.Reporter.IsRole<TricksterRole>())
            {
                var c = Palette.ImpostorRed.ToTextColor();
                Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                var othernotif = Helpers.CreateAndShowNotification(
                    $"{c}You cannot report your own {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>{c}!",
                    Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                othernotif.AdjustNotification();
                
                e.Body.Reported = false;
                return;
            }

            if (e.Reporter.IsLover())
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player == null) continue;

                    if (player.IsRole<TricksterRole>() && player.IsLover())
                    {
                        var c = Palette.ImpostorRed.ToTextColor();
                        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                        var othernotif = Helpers.CreateAndShowNotification(
                            $"{c}You cannot report your lover's {TownOfExtraColours.TricksterRoleColour.ToTextColor()}fake bodies</color>{c}!",
                            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
                        othernotif.AdjustNotification();
                        
                        e.Body.Reported = false;
                        return;
                    }
                }
            }
            
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.TricksterRoleColour));
            var notif = Helpers.CreateAndShowNotification(
                $"Something about that body felt {TownOfExtraColours.TricksterRoleColour.ToTextColor()}wrong</color>...",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterRoleIcon.LoadAsset());
            notif.AdjustNotification();
            
            TricksterRpcs.RpcNotifyTrickster(PlayerControl.LocalPlayer);
            TricksterRpcs.RpcDestroyFakeBodies(PlayerControl.LocalPlayer);
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