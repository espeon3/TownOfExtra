using MiraAPI.Hud;
using Reactor.Networking.Attributes;
using TownOfExtra.Buttons;

namespace TownOfExtra.Networking;

public class BarbarianRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.BarbarianNotifyTargetDeath)]
    public static void RpcNotifyBarbOfTargetDeath(PlayerControl player, string targetName)
    {
        if (PlayerControl.LocalPlayer != player) return;
        
        var targetBtn = CustomButtonSingleton<BarbarianTargetButton>.Instance;
        targetBtn.EffectActive = false;
        targetBtn.Timer = targetBtn.Cooldown;
        
        var attackBtn = CustomButtonSingleton<BarbarianAttackButton>.Instance;
        attackBtn.IncreaseUses();
        
        var clr = TownOfExtraColours.BarbarianRoleColour.ToTextColor();
        player.RpcSendNotification(
            $"Your {clr}target</color>, {targetName}, has been killed. You have gained an {clr}attack charge</color>!",
            "BarbarianTargetButton",
            "NeutButton",
            200,
            TownOfExtraColours.BarbarianRoleColour
        );
    }
}