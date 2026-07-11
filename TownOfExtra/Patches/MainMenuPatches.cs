using HarmonyLib;
using TownOfExtra.Achievements;
using TownOfExtra.Networking.Global;

namespace TownOfExtra.Patches;
[HarmonyPatch]
public class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Awake))]
    [HarmonyPatch(Priority.Last)]
    [HarmonyPostfix]
    public static void OnMainMenuAwakePostfix(MainMenuManager __instance)
    {
        PlayerControl.LocalPlayer.RpcAwardAchievement(AApi.GetInstance()?.LaunchGame);
    }
}