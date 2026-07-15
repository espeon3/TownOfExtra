using HarmonyLib;
//todo: using TownOfExtra.Achievements;

namespace TownOfExtra.Patches;
[HarmonyPatch]
public class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Awake))]
    [HarmonyPatch(Priority.Last)]
    [HarmonyPostfix]
    public static void OnMainMenuAwakePostfix(MainMenuManager __instance)
    {
        //todo: PlayerControl.LocalPlayer.RpcAwardAchievement(AApi.GetInstance()?.LaunchGame);
    }
}