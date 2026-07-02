using Reactor.Networking.Attributes;
using TownOfExtra.Events;
using TownOfUs;
using TownOfUs.Modules;

namespace TownOfExtra.Networking;

public class CannibalRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.CannibalNotifyDead)]
    public static void RpcNotifyCannibalDead(NetworkedPlayerInfo player)
    {
        if (PlayerControl.LocalPlayer.PlayerId != player.PlayerId) return;
        PlayerControl.LocalPlayer.RpcSendNotification(
            $"You have been {TownOfUsColors.Medic.ToTextColor()}revived</color> as the {TownOfExtraColours.CannibalRoleColour.ToTextColor()}cannibal</color> has {Palette.ImpostorRed.ToTextColor()}died</color>!",
            "CannibalRoleIcon",
            "NeutRoleIcon"
        );
    }
    
    [MethodRpc((uint)TownOfExtraRpcs.CannibalReviveVictims)]
    public static void RpcReviveCannibalVictims(PlayerControl p)
    {
        if (p == null) return;

        PlayerControl cannibal = CannibalEvents.GetCannibal();

        ReviveUtilities.RevivePlayer(
            reviver: cannibal,
            revived: p,
            position: p.transform.position,
            roleWhenAlive: p.GetRoleWhenAlive(),
            flashColor: TownOfExtraColours.CannibalRoleColour,
            revivedOwnerNotificationText: null,
            reviverOwnerNotificationText: null);
    }
}