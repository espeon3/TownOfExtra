using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Gambler;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Support;
using TownOfUs;
using TownOfUs.Modifiers.Game.Crewmate;

namespace TownOfExtra.Events
{
    public class GamblerEvents
    {
        [RegisterEvent(50)]
        public static void AfterMurderEventHandler(AfterMurderEvent e)
        {
            if (!AmongUsClient.Instance.AmHost) return;
            if (MeetingHud.Instance) return;
            
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
                    if (victim.HasModifier<CelebrityModifier>() && options.SelfReportIgnoreCelebrity)
                    {
                        GlobalRpcs.RpcSendNotification(
                            e.Source,
                            $"{victim.name} was the {TownOfUsColors.Celebrity.ToTextColor()}Celebrity</color>, so your self report has been canceled",
                            "GamblerRoleIcon",
                            "ImpRoleIcon"
                        );
                    }
                    else
                    {
                        killer.CmdReportDeadBody(victim.Data);
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

            if (killer.Data.Role is GamblerRole)
            {
                GamblerRole.ClearGambleEffect(killer);
                Coroutines.Start(GamblerRole.ApplyRandomGambleEffect(killer, msg =>
                    GlobalRpcs.RpcSendNotification(
                        e.Source, 
                        msg, 
                        "GamblerRoleIcon",
                        "ImpRoleIcon"
                    )
                ));
            }
        }

        [RegisterEvent]
        public static void GameEndEventHandler(GameEndEvent e)
        {
            GamblerRole.LastEffects.Clear();
        }
    }
}