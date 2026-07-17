using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Events;
using MiraAPI.Modifiers;
using TownOfUs.Events;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers;
using TownOfUs.Modules.Localization;
using TownOfUs.Utilities;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfExtra.Modules;
using TownOfExtra.Networking;

namespace TownOfExtra.Events;

public static class CannibalEvents
{
    [RegisterEvent]
    public static void StartMeetingEventHandler(StartMeetingEvent @event)
    {
        CannibalSystem.StopSpectatingCannibal();
        CannibalSystem.HideSwallowedNotification();

        // Digest all swallowed players locally on every client to ensure correct state immediately.
        // If we are the host, calling CannibalSystem.DigestAll will also properly sync the death to other clients.
        DigestAllCannibals();

        var local = PlayerControl.LocalPlayer;
        if (local != null && local.Data?.Role is CannibalRole)
        {
            var swallowed = CannibalSystem.GetSwallowedByCannibal(local.PlayerId);
            if (swallowed.Count > 0)
            {
                CannibalRpcs.RpcCannibalDigest(local);
            }
        }
    }

    private static void DigestAllCannibals()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player == null) continue;
            if (CannibalSystem.GetSwallowedByCannibal(player.PlayerId).Count > 0)
            {
                CannibalSystem.DigestAll(player.PlayerId);
            }
        }
    }

    [RegisterEvent(501)]
    public static void AfterMurderCleanupHandler(AfterMurderEvent @event)
    {
        var victim = @event.Target;
        if (victim == null) return;

        if (!CannibalSystem.IsDigestKillVictim(victim.PlayerId)) return;

        try
        {
            if (victim.TryGetModifier<MysticDeathNotifierModifier>(out var mysticMod))
            {
                victim.RemoveModifier(mysticMod);
            }
        }
        catch
        {
            // Ignore potential issues when removing Mystic modifier during cleanup
        }
    }

    [RegisterEvent]
    public static void PlayerDeathEventHandler(PlayerDeathEvent @event)
    {
        var deadPlayer = @event.Player;
        if (deadPlayer != null)
        {
            var swallowed = CannibalSystem.GetSwallowedByCannibal(deadPlayer.PlayerId);
            if (swallowed.Count > 0)
            {
                CannibalSystem.ReleaseAll(deadPlayer.PlayerId);
            }
        }
    }

    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        if (@event.Target != null && CannibalSystem.IsSwallowed(@event.Target.PlayerId))
        {
            @event.Cancel();
        }
    }
}