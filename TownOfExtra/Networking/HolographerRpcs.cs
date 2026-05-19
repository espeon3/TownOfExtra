using System.Collections;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Networking;

public class HolographerRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.HolographerSyncFakePlayer)]
    public static void RpcSpawnFakePlayer(PlayerControl target, Vector3 vector)
    {
        var fakePlayer = new FakePlayer(target, vector);
        Coroutines.Start(DestroyFakePlayer(fakePlayer));
    }
    
    private static IEnumerator DestroyFakePlayer(FakePlayer fakePlayer)
    {
        float duration = OptionGroupSingleton<HolographerRoleOptions>.Instance.HologramDuration;
        yield return new WaitForSeconds(duration);

        if (fakePlayer != null) fakePlayer.Destroy();
    }
}