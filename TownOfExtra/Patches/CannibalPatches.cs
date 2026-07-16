using HarmonyLib;
using System;
using System.Reflection;
using TownOfUs.Buttons;
using UnityEngine;
using TownOfUs.Modifiers.Crewmate;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using TownOfUs.Events;
using TownOfUs.Modifiers;
using TownOfUs.Modules.Localization;
using TownOfUs.Utilities;
using TownOfUs.Modifiers.Game.Universal;
using UnityEngine.UI;
using TownOfExtra.Modules;
using MiraAPI.GameOptions;
using TownOfExtra.Options.Roles;
using TownOfUs.Modules;
using TownOfUs.Options.Roles.Crewmate;
using System.Linq;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(FootstepsModifier), nameof(FootstepsModifier.FixedUpdate))]
public static class CannibalFootstepsCleanupPatch
{
    [HarmonyPrefix]
    public static bool Prefix(FootstepsModifier __instance)
    {
        if (__instance == null || __instance.Player == null || __instance.Player.Data == null)
            return true;

        if (CannibalSystem.IsSwallowed(__instance.Player.PlayerId))
            return false;

        return true;
    }
}

[HarmonyPatch(typeof(FootstepsModifier), nameof(FootstepsModifier.OnActivate))]
public static class CannibalFootstepsPatch
{
    [HarmonyPrefix]
    public static bool Prefix(FootstepsModifier __instance)
    {
        try
        {
            if (__instance.Player != null && CannibalSystem.IsSwallowed(__instance.Player.PlayerId))
                return false;
        }
        catch { /* ignore */ }

        return true;
    }
}

[HarmonyPatch]
public static class CannibalInteractionPatches
{
    // ==================== BLOCK ALL CUSTOM BUTTONS FOR SWALLOWED PLAYERS ====================

    [HarmonyPatch(typeof(TownOfUsButton), nameof(TownOfUsButton.CanUse))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static bool TownOfUsButtonCanUsePrefix(ref bool __result)
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId))
        {
            __result = false;
            return false;
        }
        return true;
    }

    private static bool _wasSwallowedLastFrame;
    private static float _savedKillTimer;
    
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void HudManagerUpdatePostfix()
    {
        var local = PlayerControl.LocalPlayer;
        bool isSwallowed = local != null && CannibalSystem.IsSwallowed(local.PlayerId);

        if (isSwallowed)
        {

            if (HudManager.Instance != null && HudManager.Instance.ShadowQuad != null && HudManager.Instance.ShadowQuad.gameObject.activeSelf)
            {
                HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
            }

            foreach (var button in CustomButtonManager.Buttons)
            {
                if (button?.Button != null && button.Button.gameObject.activeSelf)
                {
                    button.Button.gameObject.SetActive(false);
                }
            }

            if (HudManager.Instance != null)
            {
                if (HudManager.Instance.UseButton != null && HudManager.Instance.UseButton.gameObject.activeSelf)
                {
                    HudManager.Instance.UseButton.gameObject.SetActive(false);
                }
                if (HudManager.Instance.ReportButton != null && HudManager.Instance.ReportButton.gameObject.activeSelf)
                {
                    HudManager.Instance.ReportButton.gameObject.SetActive(false);
                }

                var canUseMap = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanUseMapWhileSwallowed;
                if (!canUseMap)
                {
                    if (HudManager.Instance.MapButton != null && HudManager.Instance.MapButton.gameObject.activeSelf)
                    {
                        HudManager.Instance.MapButton.gameObject.SetActive(false);
                    }
                    if (HudManager.Instance.SabotageButton != null && HudManager.Instance.SabotageButton.gameObject.activeSelf)
                    {
                        HudManager.Instance.SabotageButton.gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (_wasSwallowedLastFrame)
        {
            if (HudManager.Instance != null && HudManager.Instance.ShadowQuad != null && !HudManager.Instance.ShadowQuad.gameObject.activeSelf)
            {
                HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
            }

            foreach (var button in CustomButtonManager.Buttons)
            {
                if (button?.Button != null)
                {
                    button.Button.gameObject.SetActive(true);
                }
            }

            if (HudManager.Instance != null)
            {
                if (HudManager.Instance.MapButton != null)
                {
                    HudManager.Instance.MapButton.gameObject.SetActive(true);
                }
            }
        }
        
        _wasSwallowedLastFrame = isSwallowed;
        UpdateOverlay(isSwallowed);
    }

    private static GameObject? _CannibalOverlay;

    private static void UpdateOverlay(bool isSwallowed)
    {
        if (isSwallowed)
        {
            if (_CannibalOverlay == null && HudManager.Instance != null)
            {
                _CannibalOverlay = new GameObject("CannibalSwallowedOverlay");
                _CannibalOverlay.transform.SetParent(HudManager.Instance.transform, false);
                _CannibalOverlay.transform.SetAsFirstSibling(); // Put behind other UI buttons, but in front of game screen
                
                var image = _CannibalOverlay.AddComponent<Image>();
                image.color = new Color(0.3f, 0.05f, 0.05f, 0.35f); // Crimson/dark pink semi-transparent overlay
                
                var rect = _CannibalOverlay.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }
            }
            else if (_CannibalOverlay != null && !_CannibalOverlay.activeSelf)
            {
                _CannibalOverlay.SetActive(true);
            }
        }
        else
        {
            if (_CannibalOverlay != null && _CannibalOverlay.activeSelf)
            {
                _CannibalOverlay.SetActive(false);
            }
        }
    }


    [HarmonyPatch(typeof(HudManager), nameof(HudManager.ToggleMapVisible))]
    [HarmonyPrefix]
    public static bool HudManagerToggleMapVisiblePrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId))
        {
            var canUseMap = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanUseMapWhileSwallowed;
            if (!canUseMap)
            {
                return false;
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CanMove), MethodType.Getter)]
    [HarmonyPrefix]
    public static bool PlayerControlCanMovePrefix(PlayerControl __instance, ref bool __result)
    {
        if (__instance == PlayerControl.LocalPlayer && CannibalSystem.IsSwallowed(__instance.PlayerId))
        {
            var canUseMap = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanUseMapWhileSwallowed;
            if (canUseMap)
            {
                var stackTrace = new System.Diagnostics.StackTrace();
                for (int i = 1; i < stackTrace.FrameCount; i++)
                {
                    var method = stackTrace.GetFrame(i)?.GetMethod();
                    if (method == null) continue;
                    var type = method.DeclaringType;
                    if (type == null) continue;

                    if (type.Name.Contains("Map") || type.Name.Contains("HudManager") || type.Name.Contains("Sabotage") ||
                        method.Name.Contains("ToggleMap") || method.Name.Contains("Map") || method.Name.Contains("Sabotage"))
                    {
                        __result = true;
                        return false;
                    }
                }
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(CustomActionButton), nameof(CustomActionButton.FixedUpdateHandler))]
    [HarmonyPrefix]
    public static bool CustomActionButtonFixedUpdateHandlerPrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId))
        {
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    [HarmonyPostfix]
    [HarmonyPriority(Priority.Last)]
    public static void CalculateLightRadiusPostfix(ShipStatus __instance, NetworkedPlayerInfo player, ref float __result)
    {
        var localPlayer = PlayerControl.LocalPlayer;
        if (localPlayer != null && CannibalSystem.IsSwallowed(localPlayer.PlayerId))
        {
            __result = 100f; // Extremely large vision radius (super-torch) to illuminate the entire map
        }
    }

    [HarmonyPatch(typeof(TownOfUsButton), nameof(TownOfUsButton.ClickHandler))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static bool TownOfUsButtonClickPrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId)) return false;
        return true;
    }

    // ==================== BLOCK SWALLOWED PLAYER ABILITIES ====================

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    [HarmonyPrefix]
    public static bool KillButtonPrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(UseButton), nameof(UseButton.DoClick))]
    [HarmonyPrefix]
    public static bool UseButtonPrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
    [HarmonyPrefix]
    public static bool ReportButtonDoClickPrefix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null && CannibalSystem.IsSwallowed(local.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    [HarmonyPrefix]
    public static bool CmdReportBodyPrefix(PlayerControl __instance)
    {
        if (CannibalSystem.IsSwallowed(__instance.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
    [HarmonyPrefix]
    public static bool ReportDeadBodyPrefix(PlayerControl __instance)
    {
        if (CannibalSystem.IsSwallowed(__instance.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    [HarmonyPrefix]
    public static bool VentCanUsePrefix(ref float __result, [HarmonyArgument(0)] NetworkedPlayerInfo playerInfo)
    {
        if (playerInfo?.Object != null && CannibalSystem.IsSwallowed(playerInfo.PlayerId))
        {
            __result = float.MaxValue;
            return false;
        }
        return true;
    }

    // ==================== BLOCK TARGETING SWALLOWED PLAYERS ====================

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckMurder))]
    [HarmonyPrefix]
    public static bool CheckMurderPrefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
    {
        if (target != null && CannibalSystem.IsSwallowed(target.PlayerId)) return false;
        return true;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    [HarmonyPrefix]
    public static bool MurderPlayerPrefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
    {
        if (target == null || !CannibalSystem.IsSwallowed(target.PlayerId)) return true;

        var CannibalId = CannibalSystem.GetCannibalOf(target.PlayerId);
        if (CannibalId.HasValue && __instance.PlayerId == CannibalId.Value) return true;
        if (CannibalSystem.IsPendingDigest(target.PlayerId)) return true;

        return false;
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    [HarmonyPrefix]
    public static bool DiePrefix(PlayerControl __instance, [HarmonyArgument(0)] DeathReason reason)
    {
        if (!CannibalSystem.IsSwallowed(__instance.PlayerId)) return true;
        if (CannibalSystem.IsPendingDigest(__instance.PlayerId)) return true;
        if (CannibalSystem.IsDigestKillVictim(__instance.PlayerId)) return true;

        return false;
    }

    // ==================== PUPPETEER BLOCK ====================

    [HarmonyPatch(typeof(TownOfUs.Roles.Impostor.PuppeteerRole), nameof(TownOfUs.Roles.Impostor.PuppeteerRole.RpcPuppeteerControl))]
    [HarmonyPrefix]
    public static bool PuppeteerControlPrefix(PlayerControl target)
    {
        if (target != null && CannibalSystem.IsSwallowed(target.PlayerId)) return false;
        return true;
    }

    // ==================== POSITION SYNC + TRACKING + PRE-WIN DIGEST ====================

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    [HarmonyPrefix]
    public static void PlayerFixedUpdatePrefix(PlayerControl __instance)
    {
        if (__instance == PlayerControl.LocalPlayer && CannibalSystem.IsSwallowed(__instance.PlayerId))
        {
            _savedKillTimer = __instance.killTimer;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    [HarmonyPostfix]
    public static void PlayerFixedUpdatePostfix(PlayerControl __instance)
    {
        if (__instance == null || __instance.Data == null) return;

        if (__instance == PlayerControl.LocalPlayer && CannibalSystem.IsSwallowed(__instance.PlayerId))
        {
            __instance.killTimer = _savedKillTimer;
        }

        if (CannibalSystem.IsSwallowed(__instance.PlayerId) && !__instance.HasDied())
        {
            if (__instance.Visible) __instance.Visible = false;
            if (__instance.moveable) __instance.moveable = false;

            var col = __instance.GetComponent<UnityEngine.Collider2D>();
            if (col != null && col.enabled) col.enabled = false;

            var CannibalId = CannibalSystem.GetCannibalOf(__instance.PlayerId);
            if (CannibalId.HasValue)
            {
                var Cannibal = MiscUtils.PlayerById(CannibalId.Value);
                if (Cannibal != null && !Cannibal.HasDied())
                {
                    var CannibalPos = Cannibal.GetTruePosition();
                    __instance.transform.position = new Vector3(CannibalPos.x, CannibalPos.y, __instance.transform.position.z);
                    if (__instance.MyPhysics != null && __instance.MyPhysics.body != null)
                    {
                        __instance.MyPhysics.body.position = CannibalPos;
                        __instance.MyPhysics.body.velocity = Vector2.zero;
                    }
                }
            }
        }
    }

    // ==================== EXILE CLEANUP ====================

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    [HarmonyPostfix]
    public static void ExileWrapUpPostfix()
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null) return;

        if (local.HasDied())
        {
            if (CannibalSystem.IsSwallowed(local.PlayerId)) CannibalSystem.ReleaseAll(CannibalSystem.GetCannibalOf(local.PlayerId) ?? 0);
            local.moveable = true;
            local.Visible = true;
        }

        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player == null || player.Data == null) continue;

            if (CannibalSystem.IsSwallowed(player.PlayerId) && player.HasDied())
            {
                var CannibalId = CannibalSystem.GetCannibalOf(player.PlayerId);
                if (CannibalId.HasValue) CannibalSystem.ClearForCannibal(CannibalId.Value);
            }
        }
    }

    // ==================== ZABICIE PRZED SPOTKANIEM ====================

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static void ReportDeadBodyDigestPrefix()
    {
        DigestAllCannibals();
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.StartMeeting))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static void StartMeetingPrefix()
    {
        DigestAllCannibals();
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static void MeetingHudStartPrefix()
    {
        DigestAllCannibals();
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



    // ==================== CLEANUP NA START GRY ====================

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    [HarmonyPrefix]
    public static void ShipStatusStartPrefix()
    {
        CannibalSystem.ClearAll();
    }

    // ==================== CLEANUP NA LOBBY ====================

    [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
    [HarmonyPostfix]
    public static void LobbyStartPostfix()
    {
        CannibalSystem.ForceResetAllPlayers();
    }

    // ==================== CLEANUP NA KONIEC GRY ====================

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    [HarmonyPostfix]
    public static void OnGameEndPostfix()
    {
        CannibalSystem.ForceResetAllPlayers();
    }

    // ==================== BLOCK NETWORK TRANSFORM FOR SWALLOWED PLAYERS ====================

    [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static bool CustomNetworkTransformFixedUpdatePrefix(CustomNetworkTransform __instance)
    {
        if (__instance.isPaused || !__instance.myPlayer) return true;

        if (CannibalSystem.IsSwallowed(__instance.myPlayer.PlayerId))
        {
            return false;
        }
        return true;
    }
}
public static class CannibalTargetBlockPatches
{
    public static void Init()
    {
        try
        {
            var harmony = TownOfExtraPlugin.Harmony;
            
            var getClosestMethod = FindMethod("GetClosestLivingPlayer");
            if (getClosestMethod != null)
            {
                var prefix = typeof(CannibalTargetBlockPatches).GetMethod(nameof(GetClosestLivingPlayerPrefix), BindingFlags.Static | BindingFlags.Public);
                if (prefix != null)
                {
                    harmony.Patch(getClosestMethod, prefix: new HarmonyMethod(prefix));
                }
            }

            // 2. Patch IsTargetValid dynamically on TownOfUsTargetButton<PlayerControl>
            var isTargetValidMethod = AccessTools.Method(typeof(TownOfUsTargetButton<PlayerControl>), nameof(TownOfUsTargetButton<PlayerControl>.IsTargetValid));
            if (isTargetValidMethod != null)
            {
                var postfix = typeof(CannibalTargetBlockPatches).GetMethod(nameof(IsTargetValidPostfix), BindingFlags.Static | BindingFlags.Public);
                if (postfix != null)
                {
                    harmony.Patch(isTargetValidMethod, postfix: new HarmonyMethod(postfix));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private static MethodInfo? FindMethod(string methodName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type == null) continue;
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (method.Name == methodName)
                        {
                            return method;
                        }
                    }
                }
            }
            catch { }
        }
        return null;
    }

    public static void GetClosestLivingPlayerPrefix(ref System.Predicate<PlayerControl>? predicate)
    {
        var originalPredicate = predicate;
        predicate = new System.Predicate<PlayerControl>(x =>
        {
            if (x == null || CannibalSystem.IsSwallowed(x.PlayerId)) return false;
            return originalPredicate == null || originalPredicate(x);
        });
    }

    public static void IsTargetValidPostfix(PlayerControl? target, ref bool __result)
    {
        if (target != null && CannibalSystem.IsSwallowed(target.PlayerId))
        {
            __result = false;
        }
    }
}

[HarmonyPatch]
public static class TimeLordFixesPatch
{
    [HarmonyPatch(typeof(TimeLordRewindSystem), nameof(TimeLordRewindSystem.CancelRewindForMeeting))]
    [HarmonyPostfix]
    public static void CancelRewindForMeetingPostfix()
    {
        EjectAllSwallowed();
    }

    [HarmonyPatch(typeof(TimeLordRewindSystem), nameof(TimeLordRewindSystem.StartRewind))]
    [HarmonyPostfix]
    public static void StartRewindPostfix()
    {
        // Also eject when starting just to be safe and prevent position desyncs during rewind
        EjectAllSwallowed();
    }
    private static bool _wasRewinding;

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void HudUpdatePostfix()
    {
        bool isRewinding = TimeLordRewindSystem.IsRewinding;
        if (_wasRewinding && !isRewinding)
        {
            EjectAllSwallowed();
        }
        _wasRewinding = isRewinding;
    }

    private static void EjectAllSwallowed()
    {
        try
        {
            var history = Math.Clamp(OptionGroupSingleton<TimeLordOptions>.Instance.RewindHistorySeconds, 0.25f, 120f);
            var cutoff = DateTime.UtcNow - TimeSpan.FromSeconds(history);

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player == null) continue;

                var swallowTime = CannibalSystem.GetSwallowTime(player.PlayerId);
                if (swallowTime.HasValue && swallowTime.Value > cutoff)
                {
                    CannibalSystem.ReleaseSinglePlayer(player.PlayerId);
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"[TimeLordFixesPatch] Error releasing swallowed players: {ex.Message}");
        }
    }
}