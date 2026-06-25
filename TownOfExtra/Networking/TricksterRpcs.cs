using System.Collections.Generic;
using System.Linq;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
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
    public static void RpcNotifyTrickster(PlayerControl sender)
    {
        PlayerControl p = PlayerControl.LocalPlayer;
        if (p == null) return;

        TricksterRole.FakeBodiesReported++;
        TricksterPlaceButton.BodyPlaced = false;

        if (PlayerControl.LocalPlayer.GetTownOfUsRole() is TricksterRole)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.TricksterRoleColour));

            string ttc = TownOfExtraColours.TricksterRoleColour.ToTextColor();
            int reports = TricksterRole.FakeBodiesReported;
            int reportsNeeded = (int)OptionGroupSingleton<TricksterRoleOptions>.Instance.ReportsNeeded;

            p.RpcSendNotification($"One of your {ttc}fake bodies</color> has been found! {ttc}{reports}/{reportsNeeded}</color>",
                "TricksterRoleIcon",
                "NeutRoleIcon",
                200,
                TownOfExtraColours.TricksterRoleColour
            );
            
            CustomButtonSingleton<TricksterPlaceButton>.Instance.Timer = OptionGroupSingleton<TricksterRoleOptions>.Instance.PlaceCooldown;
            
        }
        
        if (AmongUsClient.Instance.AmHost)
        {
            List<NetworkedPlayerInfo> winners = new List<NetworkedPlayerInfo>();
            foreach (var trickster in CustomRoleUtils.GetActiveRolesOfType<TricksterRole>().Where(t => t.WinConditionMet()))
            {
                winners.Add(trickster.Player.Data);
            }

            CustomGameOver.Trigger<NeutralGameOver>(winners);
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
    public static void RpcDestroyFakeBodies(PlayerControl sender)
    {
        foreach (var body in TricksterRole.SpawnedBodies)
        {
            TricksterRole.SpawnedBodies.Remove(body);
            Object.Destroy(body.gameObject);
        }
    }
}