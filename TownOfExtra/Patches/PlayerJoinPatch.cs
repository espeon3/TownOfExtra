using System;
using System.Collections;
using System.Text.Json;
using HarmonyLib;
using Reactor.Utilities;
using TownOfUs.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
public static class PlayerJoinPatch
{
#pragma warning disable S1075 // URIs should not be hardcoded
    private const string LatestReleaseUrl = "https://api.github.com/repos/Mehzxzz/TownOfExtra/releases/latest";
#pragma warning restore S1075 // URIs should not be hardcoded

    internal static void Postfix()
    {
        Coroutines.Start(CoSendJoinMsg());
    }

    internal static IEnumerator CoSendJoinMsg()
    {
        while (!AmongUsClient.Instance) yield return null;
        while (!PlayerControl.LocalPlayer) yield return new WaitForEndOfFrame();
        while (!HudManager.Instance || !HudManager.Instance.Chat) yield return null;

        var p = PlayerControl.LocalPlayer;
        if (!p.AmOwner) yield break;

        string latestVersion = null;
        yield return CoCheckForUpdate(v => latestVersion = v);

        if (latestVersion != null && IsNewerVersion(latestVersion, TownOfExtraPluginInfo.Version))
        {
            MiscUtils.AddSystemChat(p.Data,
                $"{TownOfExtraColours.GlobalModColour.ToTextColor()}Update Checker</color>",
                $"<b>A new update to Town of Extra ({latestVersion}) is available!</b>\n" +
                "You can download it at https://github.com/Mehzxzz/TownOfExtra/releases/latest\n\n" +
                "If you find any issues with the mod, or have any ideas to improve it, please report them in our discord or the github issue tracker!"
            );
            if (!HudManager.Instance.Chat.IsOpenOrOpening) HudManager.Instance.Chat.Toggle();
        }
        else
        {
            MiscUtils.AddSystemChat(p.Data,
                $"{TownOfExtraColours.GlobalModColour.ToTextColor()}Town of Extra</color>",
                "<b>Thank you for using to town of extra!</b>\n" +
                "You are on the latest version.\n\n" +
                "If you find any issues with the mod, or have any ideas to improve it, please report them in our discord or the github issue tracker!"
            );
        }
    }

    private static IEnumerator CoCheckForUpdate(Action<string> onResult)
    {
        var request = UnityWebRequest.Get(LatestReleaseUrl);
        request.SetRequestHeader("User-Agent", "TownOfExtra-UpdateChecker");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError($"Couldn't fetch latest release from github: {request.error}");
            yield break;
        }

        try
        {
            using var jsonDocument = JsonDocument.Parse(request.downloadHandler.text);
            var releaseName = jsonDocument.RootElement.GetProperty("name").GetString();
            onResult(releaseName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Couldn't parse latest release from github: {ex.Message}");
        }
    }

    private static bool IsNewerVersion(string latest, string current)
    {

        if (Version.TryParse(latest, out var latestVer) &&
            Version.TryParse(current, out var currentVer))
        {
            return latestVer > currentVer;
        }

        return !string.Equals(latest, current, StringComparison.OrdinalIgnoreCase);
    }
}