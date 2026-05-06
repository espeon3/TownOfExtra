using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Gambler;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Support;
using TownOfUs;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Events
{
    public class GamblerEvents
    {
        private static bool _isProcessing;

        [RegisterEvent(50)]
        public static void BeforeMurderEventHandler(BeforeMurderEvent e)
        {
            var target = e.Target;
            var source = e.Source;

            if (_isProcessing) return;

            if (source.HasModifier<NoBodyModifier>() && !MeetingHud.Instance)
            {
                _isProcessing = true;
                e.Cancel();
                source.RpcCustomMurder(target, MeetingCheck.OutsideMeeting, createDeadBody: false);
                _isProcessing = false;
            }
        }

        [RegisterEvent(50)]
        public static void AfterMurderEventHandler(AfterMurderEvent e)
        {
            if (!AmongUsClient.Instance.AmHost || MeetingHud.Instance) return;

            var options = OptionGroupSingleton<GamblerRoleOptions>.Instance;
            var killer = e.Source;
            var victim = e.Target;

            if (killer == null) return;

            if (options.LongerCooldownEnabled)
            {
                if (killer.HasModifier<LongerCdModifier>())
                {
                    var newCd = killer.killTimer + options.CooldownBoost.Value;
                    if (newCd < 0) newCd = 0;
                    killer.SetKillTimer(newCd);
                }
            }

            if (options.ShorterCooldownEnabled)
            {
                if (killer.HasModifier<ShorterCdModifier>())
                {
                    var newCd = killer.killTimer - options.CooldownNerf.Value;
                    if (newCd < 0) newCd = 0;
                    killer.SetKillTimer(newCd);
                }
            }

            if (options.ViperBodyEnabled)
            {
                if (killer.HasModifier<RotBodyModifier>())
                {
                    Coroutines.Start(RotBodyModifier.StartRotting(e.Target, killer));
                }
            }

            if (options.SelfReportEnabled)
            {
                if (killer.HasModifier<SelfReportModifier>())
                {
                    if (!victim.HasModifier<CelebrityModifier>())
                    {
                        killer.CmdReportDeadBody(victim.Data);
                    }
                    else
                    {
                        if (!options.SelfReportIgnoreCelebrity) return;

                        foreach (PlayerControl p in PlayerControl.AllPlayerControls)
                        {
                            if (p == killer)
                            {
                                var notif = Helpers.CreateAndShowNotification(
                                    $"{victim.name} was the {TownOfUsColors.Celebrity.ToTextColor()}Celebrity</color>, so your self report has been canceled!",
                                    Color.white, new Vector3(0f, 1f, -20f),
                                    spr: TownOfExtraAssets.GamblerRoleIcon.LoadAsset());
                                notif.AdjustNotification();
                            }
                        }
                    }
                }
            }

            if (options.TeleportBackEnabled)
            {
                if (killer.HasModifier<TeleportBackModifier>())
                {
                    Coroutines.Start(TeleportBackModifier.StartDelay(victim, killer, options.TeleportBackDelay.Value));
                }
            }

            if (options.InvisibilityEnabled)
            {
                if (killer.HasModifier<InvisibilityModifier>())
                {
                    killer.RpcAddModifier<InvisibleModifier>();
                }
            }

            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if (p.Data.Role is GamblerRole)
                {
                    GamblerRole.ClearGambleEffect(p);
                    Coroutines.Start(GamblerRole.ApplyRandomGambleEffect(p));
                }
            }
        }

        [RegisterEvent]
        public static void GameEndEventHandler(GameEndEvent e)
        {
            GamblerRole.LastEffects.Clear();
        }
    }
}