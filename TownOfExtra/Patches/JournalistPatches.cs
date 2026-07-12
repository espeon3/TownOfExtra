using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Investigative;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Utilities;

namespace TownOfExtra.Patches;

public static class JournalistChatPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    [HarmonyPrefix]
    public static bool SendChatPatch(ChatController __instance)
    {
        if (MeetingHud.Instance || ExileController.Instance || PlayerControl.LocalPlayer.Data.IsDead)
        {
            return true;
        }

        var text = __instance.freeChatField.Text.WithoutRichText();

        if (text.Length < 1 || text.Length > 301)
        {
            return true;
        }

        if (PlayerControl.LocalPlayer.Data.Role is JournalistRole ||
            PlayerControl.LocalPlayer.HasModifier<InterviewModifier>())
        {
            if ((PlayerControl.LocalPlayer.HasModifier<ParasiteInfectedModifier>() ||
                 PlayerControl.LocalPlayer.HasModifier<PuppeteerControlModifier>()) &&
                !OptionGroupSingleton<JournalistRoleOptions>.Instance.CanSpeakWhileControlled)
            {
                MiscUtils.AddTeamChat(PlayerControl.LocalPlayer.Data,
                    $"{TownOfExtraColours.JournalistRoleColour.ToTextColor()}{PlayerControl.LocalPlayer.Data.PlayerName}</color>",
                    "You are under control! Your message cannot be sent.", blackoutText: false,
                    bubbleType: BubbleType.Other, onLeft: false);
            }
            else
            {
                JournalistRpcs.RpcSendJournalistChat(PlayerControl.LocalPlayer, text);
            }

            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();

            return false;
        }

        return true;
    }
}