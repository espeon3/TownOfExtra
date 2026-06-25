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
    [MethodRpc((uint)TownOfExtraRpcs.VinculatorEmpowerTeam)]
    public static void RpcEmpowerImpostors(PlayerControl sender)
    {
        PlayerControl p = PlayerControl.LocalPlayer;
        if (!p.IsImpostor()) return;
        
        if (p.GetTownOfUsRole() is VinculatorRole)
        {
            p.RpcSendNotification(
                $"You have {TownOfUsColors.Impostor.ToTextColor()}empowered</color> your teammates!",
                "VinculatorEmpowerButton",
                "ImpButton",
                flashColour: Palette.ImpostorRed
            );
            
            p.SetKillTimer(p.killTimer + GameOptionsManager.Instance.normalGameHostOptions.KillCooldown);
            foreach (var btn in CustomButtonManager.Buttons) btn.ResetCooldownAndOrEffect();
        }
        else
        {
            p.RpcSendNotification(
                $"You have been {TownOfUsColors.Impostor.ToTextColor()}empowered</color> by the {TownOfUsColors.Impostor.ToTextColor()}vinculator</color>, your cooldowns have been cleared!",
                "VinculatorEmpowerButton",
                "ImpButton",
                flashColour: Palette.ImpostorRed
            );
            
            p.SetKillTimer(0f);
            foreach (var btn in CustomButtonManager.Buttons) btn.SetTimer(0f);
        }
    }
}