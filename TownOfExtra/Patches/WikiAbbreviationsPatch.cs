using HarmonyLib;
using TownOfUs.Modules.Wiki;

namespace TownOfExtra.Patches;

[HarmonyPatch]
public static class WikiAbbreviationPatches
{
    [HarmonyPatch(typeof(IngameWikiMinigame), "RemoveNonCaps")]
    [HarmonyPostfix]
    public static void Postfix(ref string __result)
    {
        if (__result == "TOE")
        {
            __result = $"{TownOfExtraColours.GlobalModColour.ToTextColor()}<b>TOEX</b></color>";
        }
    }
}