using System.Linq;
using TownOfUs.Utilities;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using TownOfExtra.Events.Custom;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Modules;
using TownOfUs.Modules.TimeLord;
using TownOfUs.Modules.Components;
using TownOfUs.Options;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfExtra.Networking;

public class VultureRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.VultureCleanBody, LocalHandling = RpcLocalHandling.Before)]
    public static void RpcCleanBody(PlayerControl player, byte bodyId)
    {
        var body = TimeLordBodyManager.FindDeadBodyIncludingInactive(bodyId);
        if (body == null)
        {
            body = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == bodyId);
        }

        if (body != null)
        {
            var toexAbilityEvent = new TownOfExtraAbilityEvent(AbilityType.VultureEatBody, player, body);
            MiraEventManager.InvokeEvent(toexAbilityEvent);

            var isHost = AmongUsClient.Instance && AmongUsClient.Instance.AmHost;
            var optionEnabled = OptionGroupSingleton<TimeLordOptions>.Instance.UncleanBodiesOnRewind;
            var destroyBody = (BodyVitalsMode)OptionGroupSingleton<GameMechanicOptions>.Instance.CleanedBodiesAppearance.Value;

            var shouldRecord = isHost ? optionEnabled : (optionEnabled || TimeLordRewindSystem.MatchHasTimeLord());


            if (shouldRecord)
            {
                var bodyPlayer = MiscUtils.PlayerById(bodyId);
                if (bodyPlayer != null)
                {
                    TownOfUs.Events.Crewmate.TimeLordEventHandlers.RecordBodyCleaned(player, body, body.transform.position, 
                        TimeLordBodyManager.CleanedBodySource.Unknown);
                }
                Coroutines.Start(TimeLordBodyManager.CoHideBodyForTimeLord(body, destroyBody));
            }
            else
            {
                Coroutines.Start(body.CoCleanCustom(destroyBody));
            }
            Coroutines.Start(CrimeSceneComponent.CoClean(body));
        }
    }

    [MethodRpc((uint)TownOfExtraRpcs.VultureChangeToAmne)]
    public static void RpcChangeVultureToAmne(int aliveOthers, int impostors)
    {
        PlayerControl player = PlayerControl.LocalPlayer;
        if (player.GetTownOfUsRole() is not VultureRole || player.Data.IsDead) return;
        if (aliveOthers > impostors * 2) return;

        player.RpcChangeRole(RoleId.Get<AmnesiacRole>());

        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Amnesiac));
        var notif = Helpers.CreateAndShowNotification(
            $"There are no longer enough players for you to realistically win. You have become an {TownOfUsColors.Amnesiac.ToTextColor()}Amnesiac</color>.",
            Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Amnesiac.LoadAsset());
        notif.AdjustNotification();
    }
}