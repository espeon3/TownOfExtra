using TownOfUs.Utilities;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class GamblerRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.GamblerNotifyEffect)]
    public static void RpcNotifyEffect(PlayerControl p, string msg)
    {
        if (PlayerControl.LocalPlayer != p || p == null) return;
        
        Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
        var notif = Helpers.CreateAndShowNotification(
            msg,
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.GamblerRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
}