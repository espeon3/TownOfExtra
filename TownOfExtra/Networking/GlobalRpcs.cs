using TownOfUs.Utilities;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public static class GlobalRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.SendNotification)]
    public static void RpcSendNotification(this PlayerControl p, string msg, Sprite sprite, Color? flashColour = null)
    {
        if (PlayerControl.LocalPlayer != p || p == null) return;

        if (flashColour != null)
        {
            Coroutines.Start(MiscUtils.CoFlash((Color)flashColour));
        }

        var notif = Helpers.CreateAndShowNotification(
            msg,
            Color.white, new Vector3(0f, 1f, -20f), spr: sprite);
        notif.AdjustNotification();
    }
}