using System.Collections.Generic;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Events;

public class DreamCasterEvents
{
    public static Dictionary<PlayerControl, int> Rounds = new Dictionary<PlayerControl, int>();

    [RegisterEvent]
    public static void OnRoundStart(RoundStartEvent e)
    {
        var p = PlayerControl.LocalPlayer;
        bool justAdded = false;

        if (p.HasModifier<WaitingOnLcdModifier>())
        {
            p.RpcRemoveModifier<WaitingOnLcdModifier>();
            p.RpcAddModifier<LucidDreamingModifier>();

            Rounds[p] = 0;
            justAdded = true;

            var notif = Helpers.CreateAndShowNotification(
                $"You have fallen into a {Palette.ImpostorRed.ToTextColor()}lucid dream</color>... Your abilities will not work & you cannot die. This will last for {Palette.ImpostorRed.ToTextColor()}{OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration} round{((int)OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration != 1 ? "s" : "")}</color>.",
                Color.white, new Vector3(0f, 1f, -20f),
                spr: TownOfExtraAssets.DreamCasterRoleIcon.LoadAsset());
            notif.AdjustNotification();
        }

        if (p.HasModifier<LucidDreamingModifier>())
        {
            if (justAdded) return;
            Rounds[p]++;

            float maxRounds = OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration;

            if (Rounds[p] >= maxRounds)
            {
                p.RpcRemoveModifier<LucidDreamingModifier>();
                Rounds.Remove(p);

                var onotif = Helpers.CreateAndShowNotification(
                    $"You feel more awake... Your {Palette.ImpostorRed.ToTextColor()}lucid dream</color> has ended.",
                    Color.white, new Vector3(0f, 1f, -20f),
                    spr: TownOfExtraAssets.DreamCasterRoleIcon.LoadAsset());
                onotif.AdjustNotification();
            }
            else
            {
                float roundsLeft = maxRounds - Rounds[p];

                var notif = Helpers.CreateAndShowNotification(
                    $"You feel slightly more awake... Your {Palette.ImpostorRed.ToTextColor()}lucid dream</color> has {Palette.ImpostorRed.ToTextColor()}{roundsLeft}</color> round{((int)OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration - Rounds[PlayerControl.LocalPlayer] != 1 ? "s" : "")} left.",
                    Color.white, new Vector3(0f, 1f, -20f),
                    spr: TownOfExtraAssets.DreamCasterRoleIcon.LoadAsset());
                notif.AdjustNotification();
            }
        }
    }

    [RegisterEvent]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent e)
    {
        var source = PlayerControl.LocalPlayer;
        var button = e.Button;

        if (!button.CanClick())
        {
            return;
        }

        if (source.HasModifier<LucidDreamingModifier>()) e.Cancel();
    }
    
    [RegisterEvent]
    public static void VanillaButtonClickEventHandler(VanillaButtonClickEvent e)
    {
        var source = PlayerControl.LocalPlayer;
        var button = e.Button;

        if (button == null)
        {
            return;
        }

        if (source.HasModifier<LucidDreamingModifier>()) e.Cancel();
    }

    [RegisterEvent]
    public static void BeforeMurderEventHandler(BeforeMurderEvent e)
    {
        var source = e.Source;
        var target = e.Target;

        if (source.HasModifier<LucidDreamingModifier>() || target.HasModifier<LucidDreamingModifier>()) e.Cancel();
    }
}