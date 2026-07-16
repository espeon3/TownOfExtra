using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using TownOfExtra.Modules;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public class CannibalRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.CannibalSwallow, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcCannibalSwallow(PlayerControl Cannibal, byte victimId)
    {
        if (Cannibal == null) return;

        var victim = MiscUtils.PlayerById(victimId);
        if (victim == null || victim.HasDied()) return;

        CannibalSystem.SwallowPlayer(Cannibal.PlayerId, victimId);
        if (victim.AmOwner)
        {
            CannibalSystem.ShowSwallowedNotification();
        }
    }

    [MethodRpc((uint)TownOfExtraRpcs.CannibalDigest, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcCannibalDigest(PlayerControl Cannibal)
    {
        if (Cannibal == null) return;

        CannibalSystem.DigestAll(Cannibal.PlayerId);
    }

    [MethodRpc((uint)TownOfExtraRpcs.CannibalRelease, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcCannibalRelease(PlayerControl Cannibal)
    {
        if (Cannibal == null) return;

        CannibalSystem.ReleaseAll(Cannibal.PlayerId);
    }
}