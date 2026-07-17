using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using System.Collections;
using TownOfUs.Events;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfExtra.Modifiers.Excluded;
using TownOfUs.Modifiers;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TownOfExtra.Options.Roles;

namespace TownOfExtra.Modules;

public static class CannibalSystem
{
    private static readonly Dictionary<byte, HashSet<byte>> SwallowedPlayers = [];
    private static readonly HashSet<byte> AllSwallowed = [];
    private static readonly Dictionary<byte, Vector2> OriginalPositions = [];
    private static IEnumerator? _spectateCoroutine;
    private static GameObject? _swallowedNotificationObject;
    private static readonly Dictionary<byte, Vector2> LastCannibalPositions = [];
    private static readonly HashSet<byte> PendingDigestVictims = [];
    private static readonly Dictionary<byte, byte> PendingDigestKillers = [];
    private static readonly HashSet<byte> DigestKillVictims = [];
    private static bool _preWinDigestDone;
    public static bool IsPendingDigest(byte victimId) => PendingDigestVictims.Contains(victimId);
    public static byte? GetDigestKiller(byte victimId) =>
        PendingDigestKillers.TryGetValue(victimId, out var id) ? id : null;
    public static void ClearPendingDigest(byte victimId)
    {
        PendingDigestVictims.Remove(victimId);
        PendingDigestKillers.Remove(victimId);
    }

    public static bool IsDigestKillVictim(byte victimId) => DigestKillVictims.Contains(victimId);
    public static void ClearDigestKillVictim(byte victimId) => DigestKillVictims.Remove(victimId);

    public static bool IsSwallowed(byte playerId) => AllSwallowed.Contains(playerId);

    public static HashSet<byte> GetSwallowedByCannibal(byte CannibalId)
    {
        return SwallowedPlayers.TryGetValue(CannibalId, out var set) ? set : [];
    }

    private static readonly Dictionary<byte, byte> SwallowTracker = [];
    private static readonly Dictionary<byte, DateTime> SwallowTimes = [];

    public static DateTime? GetSwallowTime(byte victimId)
    {
        return SwallowTimes.TryGetValue(victimId, out var time) ? time : null;
    }

    public static byte? GetCannibalOf(byte victimId)
    {
        return SwallowTracker.TryGetValue(victimId, out var CannibalId) ? CannibalId : null;
    }

    public static void UpdateCannibalPosition(byte CannibalId, Vector2 position)
    {
        LastCannibalPositions[CannibalId] = position;
    }

    // ==================== FOOTSTEPS HELPERS ====================

    private static bool LocalInvestigatorActive()
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data == null || local.Data.IsDead) return false;

        return local.HasModifier<InvestigatorModifier>();
    }

    private static void RemoveFootstepsIfPresent(PlayerControl player)
    {
        try
        {
            if (player != null && player.TryGetModifier<FootstepsModifier>(out var footsteps))
                player.RemoveModifier(footsteps);
        }
        catch { /* ignore modifier removal errors */ }
    }

    private static void RestoreFootstepsIfNeeded(PlayerControl player)
    {
        try
        {
            if (player == null || player.Data == null || player.HasDied()) return;
            if (IsSwallowed(player.PlayerId)) return;
            if (!LocalInvestigatorActive()) return;

            if (!player.HasModifier<FootstepsModifier>())
                player.AddModifier<FootstepsModifier>();
        }
        catch { /* ignore modifier restoration errors */ }
    }

    // ==================== PRE-WIN DIGEST ====================

    public static bool CheckAndDigestForWin(PlayerControl Cannibal)
    {
        if (_preWinDigestDone) return false;
        if (Cannibal == null || Cannibal.HasDied()) return false;
        if (!Cannibal.AmOwner) return false;

        var swallowed = GetSwallowedByCannibal(Cannibal.PlayerId);
        if (swallowed.Count == 0) return false;

        var nonSwallowedAliveOthers = 0;
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player == null || player.Data == null || player.HasDied()) continue;
            if (player.PlayerId == Cannibal.PlayerId) continue;
            if (IsSwallowed(player.PlayerId)) continue;
            nonSwallowedAliveOthers++;
        }

        if (nonSwallowedAliveOthers > 0) return false;

        return false;
    }

    // ==================== SWALLOW / DIGEST / RELEASE ====================

    public static void SwallowPlayer(byte CannibalId, byte victimId)
    {
        if (!SwallowedPlayers.TryGetValue(CannibalId, out var set))
        {
            set = [];
            SwallowedPlayers[CannibalId] = set;
        }

        set.Add(victimId);
        AllSwallowed.Add(victimId);
        SwallowTracker[victimId] = CannibalId;
        SwallowTimes[victimId] = DateTime.UtcNow;

        var victim = MiscUtils.PlayerById(victimId);
        if (victim != null)
        {
            OriginalPositions[victimId] = victim.GetTruePosition();
            EndExternalControl(victim);
            victim.NetTransform.Halt();
            victim.moveable = false;
            victim.Visible = false;

            var col = victim.GetComponent<UnityEngine.Collider2D>();
            if (col != null) col.enabled = false;

            var Cannibal = MiscUtils.PlayerById(CannibalId);
            if (Cannibal != null)
            {
                var CannibalPos = Cannibal.GetTruePosition();
                victim.transform.position = new Vector3(CannibalPos.x, CannibalPos.y, victim.transform.position.z);
                if (victim.MyPhysics != null && victim.MyPhysics.body != null)
                {
                    victim.MyPhysics.body.position = CannibalPos;
                    victim.MyPhysics.body.velocity = Vector2.zero;
                }
                ExtensionNetTransformBacklogUtils.FlushAndSnap(victim);
            }

            if (!victim.HasModifier<CannibalSwallowedModifier>())
            {
                try
                {
                    victim.AddModifier<CannibalSwallowedModifier>();
                    if (victim.TryGetModifier<CannibalSwallowedModifier>(out var mod))
                        mod.CannibalId = CannibalId;
                }
                catch (System.Exception ex)
                {
                    Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to add modifier: {ex.Message}");
                }
            }

            RemoveFootstepsIfPresent(victim);

            bool isMeVictim = victim.AmOwner;
            bool isMeCannibal = Cannibal != null && Cannibal.AmOwner;

            if (isMeVictim || isMeCannibal)
            {

                try
                {
                    TownOfUs.Assets.TouAudio.PlaySound(TownOfExtraAssets.CannibalSwallowSound);
                }
                catch (System.Exception ex)
                {
                    Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Swallow sound error: {ex.Message}");
                }
            }

                ShowSwallowedNotification();
                StartSpectatingCannibal(CannibalId);

                try
                {
                    if (MapBehaviour.Instance != null)
                    {
                        var canUseMap = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanUseMapWhileSwallowed;
                        if (!canUseMap)
                        {
                            MapBehaviour.Instance.Close();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to close MapBehaviour: {ex.Message}");
                }
            }
        }


    private static void EndExternalControl(PlayerControl victim)
    {
        try
        {
            if (TownOfUs.Modules.ControlSystem.ParasiteControlState.IsControlled(victim.PlayerId, out var parasiteId))
            {
                var parasite = MiscUtils.PlayerById(parasiteId);
                if (parasite?.Data?.Role is TownOfUs.Roles.Impostor.ParasiteRole)
                    TownOfUs.Roles.Impostor.ParasiteRole.RpcParasiteEndControl(parasite, victim);
            }
        }
        catch { /* ignore Parasite/Puppeteer state errors */ }

        try
        {
            if (TownOfUs.Modules.ControlSystem.PuppeteerControlState.IsControlled(victim.PlayerId, out var puppeteerId))
            {
                var puppeteer = MiscUtils.PlayerById(puppeteerId);
                if (puppeteer?.Data?.Role is TownOfUs.Roles.Impostor.PuppeteerRole)
                    TownOfUs.Roles.Impostor.PuppeteerRole.RpcPuppeteerEndControl(puppeteer, victim);
            }
        }
        catch { /* ignore Puppeteer state errors */ }

        try
        {
            if (victim.Data?.Role is TownOfUs.Roles.Impostor.ParasiteRole parasiteRole && parasiteRole.Controlled != null)
                TownOfUs.Roles.Impostor.ParasiteRole.RpcParasiteEndControl(victim, parasiteRole.Controlled);
        }
        catch { /* ignore Parasite role state errors */ }

        try
        {
            if (victim.Data?.Role is TownOfUs.Roles.Impostor.PuppeteerRole puppeteerRole && puppeteerRole.Controlled != null)
                TownOfUs.Roles.Impostor.PuppeteerRole.RpcPuppeteerEndControl(victim, puppeteerRole.Controlled);
        }
        catch { /* ignore Puppeteer role state errors */ }
    }

    public static void DigestAll(byte CannibalId)
    {
        if (!SwallowedPlayers.TryGetValue(CannibalId, out var victims)) return;

        foreach (var victimId in victims.ToList())
        {
            var victim = MiscUtils.PlayerById(victimId);

            if (victim != null && !victim.HasDied())
            {
                AllSwallowed.Remove(victimId);
                SwallowTracker.Remove(victimId);
                SwallowTimes.Remove(victimId);
                OriginalPositions.Remove(victimId);

                DigestKillVictims.Add(victimId);
                victim.Die(DeathReason.Kill, false);

                var Cannibal = MiscUtils.PlayerById(CannibalId);
                if (Cannibal != null)
                {
                    var afterMurderEvent = new AfterMurderEvent(Cannibal, victim, null);
                    MiraEventManager.InvokeEvent(afterMurderEvent);
                }

                var localPlayer = PlayerControl.LocalPlayer;
                bool isGhostOrCannibal = localPlayer != null && (localPlayer.Data.IsDead || localPlayer.PlayerId == CannibalId);

                if (!victim.HasModifier<DeathHandlerModifier>())
                {
                    try { victim.AddModifier<DeathHandlerModifier>(); } catch { /* ignore death handler addition failure */ }
                }

                try
                {
                    string killerText = (isGhostOrCannibal && Cannibal != null)
                        ? TouLocale.GetParsed("ExtensionDiedByCannibal", "by <player>").Replace("<player>", Cannibal.Data.PlayerName)
                        : "";

                    DeathHandlerModifier.UpdateDeathHandlerImmediate(
                        victim,
                        causeOfDeath: "Eaten",
                        roundOfDeath: DeathEventHandlers.CurrentRound,
                        diedThisRound: DeathHandlerOverride.SetTrue,
                        killedBy: killerText,
                        lockInfo: DeathHandlerOverride.SetTrue
                    );
                }
                catch { /* ignore death handler update errors */ }
            }

            if (victim != null)
            {
                RemoveSwallowedModifier(victim);
                victim.Visible = true;
                victim.moveable = true;

                if (victim.AmOwner)
                {
                    StopSpectatingCannibal();
                    HideSwallowedNotification();
                    Coroutines.Start(CoRefreshHud());
                }
            }

            AllSwallowed.Remove(victimId);
            SwallowTracker.Remove(victimId);
            SwallowTimes.Remove(victimId);
            OriginalPositions.Remove(victimId);


            if (AmongUsClient.Instance.AmHost && victim != null)
            {
                try
                {
                    var role = victim.Data?.Role;
                    if (role is TownOfUs.Roles.ITownOfUsRole touRole)
                    {
                        var ghostRole = touRole.Configuration.GhostRole;
                        if ((int)ghostRole != -1)
                        {
                            victim.RpcSetRole(ghostRole);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Role set error: {ex.Message}");
                }
            }
        }

        SwallowedPlayers.Remove(CannibalId);
    }

    private static IEnumerator CoRefreshHud()
    {
        yield return new WaitForSeconds(0.1f);
        if (HudManager.Instance != null)
        {
            try
            {
                HudManager.Instance.SetHudActive(false);
                HudManager.Instance.SetHudActive(true);
            }
            catch { /* ignore HUD refresh errors */ }
        }
    }

    public static void ReleaseAllAtPosition(byte CannibalId, Vector2 releasePosition)
    {
        if (!SwallowedPlayers.TryGetValue(CannibalId, out var victims) || victims.Count == 0) return;

        var safePosition = FindSafePosition(releasePosition);
        DoRelease(CannibalId, victims, safePosition);
    }

    public static void ReleaseAll(byte CannibalId)
    {
        if (!SwallowedPlayers.TryGetValue(CannibalId, out var victims) || victims.Count == 0) return;

        Vector2 releasePosition;

        if (LastCannibalPositions.TryGetValue(CannibalId, out var trackedPos))
        {
            releasePosition = trackedPos;
        }
        else
        {
            var Cannibal = MiscUtils.PlayerById(CannibalId);
            if (Cannibal != null)
            {
                releasePosition = Cannibal.GetTruePosition();
                if (releasePosition.x < -500f || releasePosition.y < -500f)
                    releasePosition = GetFallbackPosition(victims);
            }
            else
            {
                releasePosition = GetFallbackPosition(victims);
            }
        }

        var safePosition = FindSafePosition(releasePosition);
        DoRelease(CannibalId, victims, safePosition);
    }

    private static Vector2 GetFallbackPosition(HashSet<byte> victims)
    {
        foreach (var victimId in victims)
        {
            if (OriginalPositions.TryGetValue(victimId, out var origPos)) return origPos;
        }
        return Vector2.zero;
    }

    private static void DoRelease(byte CannibalId, HashSet<byte> victims, Vector2 safePosition)
    {
        foreach (var victimId in victims.ToList())
        {
            var victim = MiscUtils.PlayerById(victimId);
            if (victim != null)
            {
                RemoveSwallowedModifier(victim);

                try
                {
                    if (victim.TryGetModifier<DeathHandlerModifier>(out var dhMod))
                        victim.RemoveModifier(dhMod);
                }
                catch { /* ignore death handler removal errors */ }

                if (!victim.HasDied())
                {
                    victim.Visible = true;
                    victim.moveable = true;
                    victim.transform.position = new Vector3(safePosition.x, safePosition.y, 0f);
                    if (victim.MyPhysics != null && victim.MyPhysics.body != null)
                    {
                        victim.MyPhysics.body.position = safePosition;
                        victim.MyPhysics.body.velocity = Vector2.zero;
                    }
                    var col = victim.GetComponent<UnityEngine.Collider2D>();
                    if (col != null) col.enabled = true;
                    ExtensionNetTransformBacklogUtils.FlushAndSnap(victim);

                    RestoreFootstepsIfNeeded(victim);
                }

                if (victim.AmOwner)
                {
                    StopSpectatingCannibal();
                    HideSwallowedNotification();
                    if (!victim.HasDied()) ShowReleaseNotification();
                }

                Logger<TownOfExtraPlugin>.Info(
                    $"[CannibalSystem] Released player {victimId} at ({safePosition.x}, {safePosition.y})");
            }

            AllSwallowed.Remove(victimId);
            SwallowTracker.Remove(victimId);
            SwallowTimes.Remove(victimId);
            OriginalPositions.Remove(victimId);
        }

        victims.Clear();
        SwallowedPlayers.Remove(CannibalId);
        LastCannibalPositions.Remove(CannibalId);
    }

    public static void ReleaseSinglePlayer(byte victimId)
    {
        if (!SwallowTracker.TryGetValue(victimId, out var CannibalId)) return;
        if (!SwallowedPlayers.TryGetValue(CannibalId, out var victims)) return;

        var victim = MiscUtils.PlayerById(victimId);
        if (victim != null)
        {
            RemoveSwallowedModifier(victim);
            try
            {
                if (victim.TryGetModifier<DeathHandlerModifier>(out var dhMod))
                    victim.RemoveModifier(dhMod);
            }
            catch { /* ignore death handler removal errors */ }

            if (!victim.HasDied())
            {
                victim.Visible = true;
                victim.moveable = true;

                if (OriginalPositions.TryGetValue(victimId, out var origPos))
                {
                    victim.transform.position = new Vector3(origPos.x, origPos.y, 0f);
                    if (victim.MyPhysics != null && victim.MyPhysics.body != null)
                    {
                        victim.MyPhysics.body.position = origPos;
                        victim.MyPhysics.body.velocity = Vector2.zero;
                    }
                    ExtensionNetTransformBacklogUtils.FlushAndSnap(victim);
                }
                var col = victim.GetComponent<UnityEngine.Collider2D>();
                if (col != null) col.enabled = true;

                RestoreFootstepsIfNeeded(victim);
            }

            if (victim.AmOwner)
            {
                StopSpectatingCannibal();
                HideSwallowedNotification();
                if (!victim.HasDied()) ShowReleaseNotification();
            }
        }

        victims.Remove(victimId);
        AllSwallowed.Remove(victimId);
        SwallowTracker.Remove(victimId);
        SwallowTimes.Remove(victimId);
        OriginalPositions.Remove(victimId);

        if (victims.Count == 0)
        {
            SwallowedPlayers.Remove(CannibalId);
            LastCannibalPositions.Remove(CannibalId);
        }
    }

    private static Vector2 FindSafePosition(Vector2 targetPosition)
    {
        if (targetPosition.x < -500f || targetPosition.y < -500f)
        {
            var safePos = OriginalPositions.Values.FirstOrDefault(pos => IsPositionSafe(pos, pos));
            if (safePos != default) return safePos;
            return Vector2.zero;
        }

        if (IsPositionSafe(targetPosition, targetPosition)) return targetPosition;

        float[] distances = [0.3f, 0.5f, 0.7f, 1.0f, 1.3f, 1.5f, 2.0f];
        int directions = 8;

        foreach (var dist in distances)
        {
            for (int i = 0; i < directions; i++)
            {
                var angle = (2f * Mathf.PI * i) / directions;
                var candidate = targetPosition + new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

                if (IsPositionSafe(candidate, targetPosition)) return candidate;
            }
        }

        var fallbackPos = OriginalPositions.Values.FirstOrDefault(pos => IsPositionSafe(pos, pos));
        if (fallbackPos != default) return fallbackPos;

        try
        {
            if (ShipStatus.Instance != null)
            {
                var spawn = ShipStatus.Instance.InitialSpawnCenter;
                if (IsPositionSafe(spawn, spawn)) return spawn;
            }
        }
        catch { /* ignore ship status center errors */ }

        return targetPosition;
    }

    private static bool IsPositionSafe(Vector2 candidate, Vector2 fromPosition)
    {
        try
        {
            if (ShipStatus.Instance == null) return true;

            var hit = Physics2D.OverlapCircle(candidate, 0.22f, Constants.ShipAndAllObjectsMask);
            if (hit != null) return false;

            if (Vector2.Distance(candidate, fromPosition) > 0.1f)
            {
                var wallHit = Physics2D.Linecast(fromPosition, candidate, Constants.ShipAndAllObjectsMask);
                if (wallHit.collider != null) return false;
            }

            return true;
        }
        catch { /* fallback to safe default on error */ return true; }
    }

    private static void RemoveSwallowedModifier(PlayerControl player)
    {
        if (player.TryGetModifier<CannibalSwallowedModifier>(out var mod))
            player.RemoveModifier(mod);
    }

    public static void StartSpectatingCannibal(byte CannibalId)
    {
        StopSpectatingCannibal();
        _spectateCoroutine = CoSpectateCannibal(CannibalId);
        Coroutines.Start(_spectateCoroutine);
    }

    public static void StopSpectatingCannibal()
    {
        if (_spectateCoroutine != null)
        {
            Coroutines.Stop(_spectateCoroutine);
            _spectateCoroutine = null;
        }

        var localPlayer = PlayerControl.LocalPlayer;
        if (localPlayer != null && Camera.main != null)
        {
            var follower = Camera.main.GetComponent<FollowerCamera>();
            follower?.SetTarget(localPlayer);
        }
    }

    private static IEnumerator CoSpectateCannibal(byte CannibalId)
    {
        yield return new WaitForSeconds(0.1f);

        var localPlayer = PlayerControl.LocalPlayer;
        if (localPlayer == null) yield break;

        while (true)
        {
            if (LobbyBehaviour.Instance != null || AmongUsClient.Instance == null ||
                !AmongUsClient.Instance.IsGameStarted)
                yield break;

            if (IsSwallowed(localPlayer.PlayerId))
            {
                var Cannibal = MiscUtils.PlayerById(CannibalId);
                if (Cannibal != null && !Cannibal.HasDied() && Camera.main != null)
                {
                    var follower = Camera.main.GetComponent<FollowerCamera>();
                    if (follower != null && follower.Target != Cannibal)
                    {
                        follower.SetTarget(Cannibal);
                    }
                }
            }
            else
            {
                if (Camera.main != null)
                {
                    var follower = Camera.main.GetComponent<FollowerCamera>();
                    if (follower != null && follower.Target != localPlayer)
                    {
                        follower.SetTarget(localPlayer);
                    }
                }
                yield break;
            }

            yield return null;
        }
    }

    public static void ShowSwallowedNotification()
    {
        HideSwallowedNotification();
        if (HudManager.Instance == null) return;

        try
        {
            var line1 = TouLocale.Get("ExtensionCannibalSwallowedNotification", "You have been swallowed by Cannibal!");
            var message = line1.Replace("/n", "\n").Replace("\\n", "\n");

            Coroutines.Start(CoShowNotificationThreeTimes(message));
        }
        catch (System.Exception ex)
        {
            Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to start swallow notifications: {ex.Message}");
        }
    }

    private static IEnumerator CoShowNotificationThreeTimes(string message)
    {
        for (int i = 0; i < 3; i++)
        {
            HideSwallowedNotification();
            try
            {
                var notif = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfExtraColours.CannibalRoleColour.ToTextColor()}{message}</color></b>",
                    TownOfExtraColours.CannibalRoleColour,
                    new Vector3(0f, 1f, -20f),
                    spr: TownOfExtraAssets.CannibalRoleIcon.LoadAsset());

                if (notif != null)
                {
                    _swallowedNotificationObject = notif.gameObject;
                    try { notif.AdjustNotification(); } catch { /* Ignore adjustment failures on certain notification templates */ }
                    try
                    {
                        var canvasGroup = notif.GetComponent<CanvasGroup>();
                        if (canvasGroup != null) canvasGroup.alpha = 1f;
                    }
                    catch { /* Ignore canvas group failures on some notification layouts */ }
                }
            }
            catch (System.Exception ex)
            {
                Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to show notification attempt {i}: {ex.Message}");
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public static void ShowReleaseNotification()
    {
        HideSwallowedNotification();
        if (HudManager.Instance == null) return;

        try
        {
            var line1 = TouLocale.Get("ExtensionCannibalReleasedNotification", "You have been released from the Cannibal!");
            var message = line1.Replace("/n", "\n").Replace("\\n", "\n");

            Coroutines.Start(CoShowReleaseNotificationThreeTimes(message));
        }
        catch (System.Exception ex)
        {
            Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to start release notifications: {ex.Message}");
        }
    }

    private static IEnumerator CoShowReleaseNotificationThreeTimes(string message)
    {
        for (int i = 0; i < 3; i++)
        {
            HideSwallowedNotification();
            try
            {
                var notif = Helpers.CreateAndShowNotification(
                    $"<b>{TownOfExtraColours.CannibalRoleColour.ToTextColor()}{message}</color></b>",
                    TownOfExtraColours.CannibalRoleColour,
                    new Vector3(0f, 1f, -20f),
                    spr: TownOfExtraAssets.CannibalRoleIcon.LoadAsset());

                if (notif != null)
                {
                    _swallowedNotificationObject = notif.gameObject;
                    try { notif.AdjustNotification(); } catch { /* Ignore adjustment failures on certain notification templates */ }
                    try
                    {
                        var canvasGroup = notif.GetComponent<CanvasGroup>();
                        if (canvasGroup != null) canvasGroup.alpha = 1f;
                    }
                    catch { /* Ignore canvas group failures on some notification layouts */ }
                }
            }
            catch (System.Exception ex)
            {
                Logger<TownOfExtraPlugin>.Error($"[CannibalSystem] Failed to show release notification attempt {i}: {ex.Message}");
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public static void HideSwallowedNotification()
    {
        if (_swallowedNotificationObject != null)
        {
            UnityEngine.Object.Destroy(_swallowedNotificationObject);
            _swallowedNotificationObject = null;
        }
    }

    public static void ClearForCannibal(byte CannibalId)
    {
        if (SwallowedPlayers.TryGetValue(CannibalId, out var victims))
        {
            foreach (var victimId in victims.ToList())
            {
                var victim = MiscUtils.PlayerById(victimId);
                if (victim != null)
                {
                    RemoveSwallowedModifier(victim);
                    RestoreFootstepsIfNeeded(victim);

                    if (victim.AmOwner)
                    {
                        StopSpectatingCannibal();
                        HideSwallowedNotification();
                    }
                }

                AllSwallowed.Remove(victimId);
                SwallowTracker.Remove(victimId);
                SwallowTimes.Remove(victimId);
                OriginalPositions.Remove(victimId);
            }

            SwallowedPlayers.Remove(CannibalId);
        }

        LastCannibalPositions.Remove(CannibalId);
    }

    public static void ClearAll()
    {
        StopSpectatingCannibal();
        HideSwallowedNotification();

        foreach (var CannibalId in SwallowedPlayers.Keys.ToList())
        {
            if (SwallowedPlayers.TryGetValue(CannibalId, out var victims))
            {
                foreach (var victimId in victims)
                {
                    var victim = MiscUtils.PlayerById(victimId);
                    if (victim != null)
                    {
                        RemoveSwallowedModifier(victim);
                        victim.Visible = true;
                        victim.moveable = true;
                        var col = victim.GetComponent<UnityEngine.Collider2D>();
                        if (col != null) col.enabled = true;
                        RestoreFootstepsIfNeeded(victim);
                    }
                }
            }
        }

        SwallowedPlayers.Clear();
        AllSwallowed.Clear();
        SwallowTracker.Clear();
        SwallowTimes.Clear();
        OriginalPositions.Clear();
        LastCannibalPositions.Clear();
        PendingDigestVictims.Clear();
        PendingDigestKillers.Clear();
        DigestKillVictims.Clear();
        _preWinDigestDone = false;
    }

    public static void ForceResetAllPlayers()
    {
        StopSpectatingCannibal();
        HideSwallowedNotification();

        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player == null) continue;
            try { RemoveSwallowedModifier(player); } catch { /* ignore cleanup error */ }
            player.Visible = true;
            player.moveable = true;
            var col = player.GetComponent<UnityEngine.Collider2D>();
            if (col != null) col.enabled = true;
            RestoreFootstepsIfNeeded(player);
            try { player.NetTransform.Halt(); } catch { /* ignore halt error */ }
        }

        SwallowedPlayers.Clear();
        AllSwallowed.Clear();
        SwallowTracker.Clear();
        SwallowTimes.Clear();
        OriginalPositions.Clear();
        LastCannibalPositions.Clear();
        PendingDigestVictims.Clear();
        PendingDigestKillers.Clear();
        DigestKillVictims.Clear();
        _preWinDigestDone = false;
    }
}