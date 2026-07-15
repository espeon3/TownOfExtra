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
    public static void RpcSpawnFakePlayer(PlayerControl target, float x, float y, float z)
    {
        var vector = new Vector3(x, y, x);
        var fakePlayer = new CustomLocFakePlayer(target, vector);
        Coroutines.Start(DestroyFakePlayer(fakePlayer));
    }
    
    private static IEnumerator DestroyFakePlayer(CustomLocFakePlayer customLocFakePlayer)
    {
        float duration = OptionGroupSingleton<HolographerRoleOptions>.Instance.HologramDuration;
        yield return new WaitForSeconds(duration);

        if (customLocFakePlayer != null) customLocFakePlayer.Destroy();
    }
}