using System.Text.RegularExpressions;
using HarmonyLib;
using Reactor.Utilities;

namespace TownOfExtra.Patches;

[HarmonyPatch(typeof(ReactorCredits), "GetText")]
public static class PerfectCommsCreditsColorPatch
{
    private const string Label = $"{TownOfExtraPluginInfo.Name} {TownOfExtraPluginInfo.Version}";

    private static void Postfix(ref string __result)
    {
        if (string.IsNullOrEmpty(__result)) return;

        var coloredLabel = $"{TownOfExtraColours.GlobalModColour.ToTextColor()}<noparse>{Label}</noparse></color>";
        var updated = Regex.Replace(
            __result,
            $"<color=#[0-9A-Fa-f]{{3,8}}><noparse>{Regex.Escape(Label)}</noparse></color>",
            coloredLabel);

        if (ReferenceEquals(updated, __result) || updated == __result)
            updated = __result.Replace($"<noparse>{Label}</noparse>", coloredLabel);

        if (ReferenceEquals(updated, __result) || updated == __result)
            updated = __result.Replace(Label, coloredLabel);

        __result = updated;
    }
}