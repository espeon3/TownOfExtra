using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Game.Universal.Passive;
using TownOfExtra.Networking.Global;
using TownOfUs.Buttons;
using TownOfUs.Buttons.Neutral;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules;
using TownOfUs.Options;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public static class YouthlingEvents
{
    [RegisterEvent(-1000)]
    public static void BeforeMurderEventHandler(BeforeMurderEvent @event)
    {
        var source = @event.Source;
        var target = @event.Target;

        if (CheckForOverAge(@event, source, target))
        {
            ResetButtonTimer(source);
            source.RpcSendNotification(
                $"You cannot attack {target.Data.PlayerName} until they {TownOfExtraColours.YouthlingModifierColour.ToTextColor()}grow up</color>!",
                "YouthlingModifierIcon",
                "UniModIcon",
                200,
                TownOfExtraColours.YouthlingModifierColour
            );
        }
    }

    [RegisterEvent(-1000)]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent @event)
    {
        var source = PlayerControl.LocalPlayer;
        var button = @event.Button as CustomActionButton<PlayerControl>;
        var target = button?.Target;
        if (target == null || button is not IKillButton || !button.CanClick())
        {
            return;
        }

        if (CheckForOverAge(@event, source, target))
        {
            ResetButtonTimer(source, button);
            source.RpcSendNotification(
                $"You cannot attack {target.Data.PlayerName} until they {TownOfExtraColours.YouthlingModifierColour.ToTextColor()}grow up</color>!",
                "YouthlingModifierIcon",
                "UniModIcon",
                200,
                TownOfExtraColours.YouthlingModifierColour
            );
        }
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        var source = @event.Source;

        if (source.Data.Role is not MirrorcasterRole)
        {
            return;
        }

        if (source.TryGetModifier<AllianceGameModifier>(out var allyMod) && !allyMod.GetsPunished)
        {
            return;
        }

        var target = @event.Target;

        if (GameHistory.PlayerStats.TryGetValue(source.PlayerId, out var stats))
        {
            if (!target.IsCrewmate() ||
                (target.TryGetModifier<AllianceGameModifier>(out var allyMod2) && !allyMod2.GetsPunished))
            {
                stats.CorrectKills += 1;
            }
            else if (source != target)
            {
                stats.IncorrectKills += 1;
            }
        }
    }
    
    private static bool CheckForOverAge(MiraCancelableEvent @event, PlayerControl source, PlayerControl target)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return false;
        }

        if (!target.TryGetModifier<YouthlingModifier>(out var youthling) || youthling.Age >= 18)
        {
            return false;
        }

        @event.Cancel();

        return true;
    }

    private static void ResetButtonTimer(PlayerControl source, CustomActionButton<PlayerControl> button = null)
    {
        if (!source.AmOwner)
        {
            return;
        }

        button?.ResetCooldownAndOrEffect();

        if (source.Data.Role is WerewolfRole)
        {
            CustomButtonSingleton<WerewolfRampageButton>.Instance.ResetCooldownAndOrEffect();
        }
        source.SetKillTimer(OptionGroupSingleton<GeneralOptions>.Instance.TempSaveCdReset);
    }
}