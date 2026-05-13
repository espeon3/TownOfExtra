using MiraAPI.Hud;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public class VinculatorRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.VinculatorNotifyTeam)]
    public static void RpcEmpowerImpostors(PlayerControl sender)
    {
        PlayerControl p = PlayerControl.LocalPlayer;
        if (!p.IsImpostor()) return;
        
        if (p.GetTownOfUsRole() is VinculatorRole)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor)); 
            var notif = Helpers.CreateAndShowNotification(
                $"You have {TownOfUsColors.Impostor.ToTextColor()}empowered</color> your teammates!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VinculatorEmpowerButton.LoadAsset());
            notif.AdjustNotification();
            
            foreach (var btn in CustomButtonManager.Buttons) btn.ResetCooldownAndOrEffect();
            PlayerControl.LocalPlayer.SetKillTimer(GameOptionsManager.Instance.normalGameHostOptions.KillCooldown);
        }
        else
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor));
            var notif = Helpers.CreateAndShowNotification(
                $"You have been {TownOfUsColors.Impostor.ToTextColor()}empowered</color> by the {TownOfUsColors.Impostor.ToTextColor()}vinculator</color>, your cooldowns have been cleared!",
                Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VinculatorEmpowerButton.LoadAsset());
            notif.AdjustNotification();
            
            foreach (var btn in CustomButtonManager.Buttons) btn.SetTimer(0f);
            PlayerControl.LocalPlayer.SetKillTimer(0f);
        }
    }
}