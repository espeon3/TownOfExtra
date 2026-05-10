using System.Collections.Generic;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Buttons;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.GameOver;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class TricksterRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.NotifyTrickster)]
    public static void RpcNotifyTrickster(PlayerControl targetPlayer)
    {
        if (targetPlayer == null) return;

        if (PlayerControl.LocalPlayer == targetPlayer)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.TricksterRoleColour));
            
            string ttc = TownOfExtraColours.TricksterRoleColour.ToTextColor();
            int reports = TricksterRole.FakeBodiesReported;
            int reportsNeeded = (int)OptionGroupSingleton<TricksterRoleOptions>.Instance.ReportsNeeded;
            
            var notif = Helpers.CreateAndShowNotification(
                $"One of your {ttc}fake bodies</color> has been found! {ttc}{reports}/{reportsNeeded}</color>",
                Color.white,
                new Vector3(0f, 1f, -20f)
            );
            notif.AdjustNotification();

            if (reports == reportsNeeded)
            {
                List<NetworkedPlayerInfo> winners = new List<NetworkedPlayerInfo>();
                winners.Add(PlayerControl.LocalPlayer.Data);
                CustomGameOver.Trigger<NeutralGameOver>(winners);
            }
        }
    }
    
    [MethodRpc((uint)TownOfExtraRpcs.PlaceFakeBody)]
    public static void RpcPlaceFakeBody(PlayerControl sender, byte colorId, byte parentId)
    {
        var body = TricksterPlaceButton.CreateDeadBody(sender.transform.position, colorId, parentId, sender);
        if (body != null)
        {
            TricksterRole.SpawnedBodies.Add(body);
        }
    }
    
    [MethodRpc((uint)TownOfExtraRpcs.DestroyFakeBodies)]
    public static void RpcDestroyFakeBodies(PlayerControl sender, byte playerId)
    {
        foreach (var body in Object.FindObjectsOfType<DeadBody>())
        {
            if (body.ParentId == playerId)
            {
                TricksterRole.SpawnedBodies.Remove(body);
                Object.Destroy(body.gameObject);
                return;
            }
        }
    }
}