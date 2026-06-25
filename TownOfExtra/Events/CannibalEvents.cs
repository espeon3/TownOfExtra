using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.GameOptions;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class CannibalEvents
{
    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        if (!AmongUsClient.Instance.AmHost) return;
    
        if (CannibalRole.EatenPlayers.Count == 0) return;
        if (!OptionGroupSingleton<CannibalRoleOptions>.Instance.ReviveIfDeadCannibal) return;
    
        PlayerControl cannibal = GetCannibal();
    
        if (cannibal == null || !cannibal.Data.IsDead)
        {
            return;
        }

        foreach (byte victimId in CannibalRole.EatenPlayers)
        {
            PlayerControl victim = GameData.Instance.GetPlayerById(victimId)?.Object;
        
            if (victim == null) continue;

            CannibalRpcs.RpcReviveCannibalVictims(MiscUtils.PlayerById(victimId));
            CannibalRpcs.RpcNotifyCannibalDead(GameData.Instance.GetPlayerById(victimId));
        }

        CannibalRole.EatenPlayers.Clear();
    }
    
    [RegisterEvent]
    public static void OnMeetingStart(StartMeetingEvent e)
    {
        if (!AmongUsClient.Instance.AmHost) return;

        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is CannibalRole)
            {
                CannibalRole.CannibalId = p.PlayerId;
                return;
            }
        }
    }

    public static PlayerControl GetCannibal()
    {
        if (CannibalRole.CannibalId == null) return null;
        return GameData.Instance.GetPlayerById(CannibalRole.CannibalId.Value)?.Object;
    }
}