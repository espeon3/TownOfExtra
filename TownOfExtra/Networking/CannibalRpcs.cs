using Reactor.Networking.Attributes;
using TownOfExtra.Events;
using TownOfExtra.Roles.Impostor.Concealing;
using TownOfUs;
using TownOfUs.Modules;

namespace TownOfExtra.Networking;

public class CannibalRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.NotifyCannibalDead)]
    public static void RpcNotifyCannibalDead(NetworkedPlayerInfo player)
    {
        if (PlayerControl.LocalPlayer.PlayerId != player.PlayerId) return;
        CannibalRole.SendRevivedMessage();
    }
    
    [MethodRpc((uint)TownOfExtraRpcs.ReviveCannibalVictims)]
    public static void RpcReviveCannibalVictims(PlayerControl sender, byte victimId)
    {
        PlayerControl victim = GameData.Instance.GetPlayerById(victimId)?.Object;
        if (victim == null) return;

        PlayerControl cannibal = CannibalEvents.GetCannibal();

        ReviveUtilities.RevivePlayer(
            reviver: cannibal,
            revived: victim,
            position: victim.transform.position,
            roleWhenAlive: victim.GetRoleWhenAlive(),
            flashColor: TownOfUsColors.Medic,
            revivedOwnerNotificationText: null,
            reviverOwnerNotificationText: null);
    }
}