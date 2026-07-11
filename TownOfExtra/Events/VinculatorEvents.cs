using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfExtra.Achievements;
using TownOfExtra.Buttons;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Networking;
using TownOfUs.Utilities;

namespace TownOfExtra.Events;

public class VinculatorEvents
{
    public static int ActiveEmpowerKillCount;
    public static int ActiveChainKillCount;
    
    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent e)
    {
        var opt = OptionGroupSingleton<VinculatorRoleOptions>.Instance;
        var button = CustomButtonSingleton<VinculatorEmpowerButton>.Instance;
        if (e.Source.AmOwner)
        {
            if (e.Source.GetTownOfUsRole() is not VinculatorRole)
            {
                return;
            }
            
            ++ActiveEmpowerKillCount;

            if (button.LimitedUses &&
                opt.EmpowerKillsForNew != 0 && opt.EmpowerKillsForNew <= ActiveEmpowerKillCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveEmpowerKillCount = 0;
            }
        }
    }
    
    [RegisterEvent]
    public static void ThatOtherAfterMurderEventHandler(AfterMurderEvent e)
    {
        var opt = OptionGroupSingleton<VinculatorRoleOptions>.Instance;
        var button = CustomButtonSingleton<VinculatorChainButton>.Instance;
        if (e.Source.AmOwner)
        {
            if (e.Source.GetTownOfUsRole() is not VinculatorRole)
            {
                return;
            }
            
            ++ActiveChainKillCount;

            if (button.LimitedUses &&
                opt.ChainKillsForNew != 0 && opt.ChainKillsForNew <= ActiveChainKillCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveChainKillCount = 0;
            }
        }
    }
    
    [RegisterEvent]
    public static void EjectionEventHandler(EjectionEvent e)
    {
        var exiled = e.ExileController.initData.networkedPlayer.Object;

        if (exiled == null ||
            e.ExileController.initData.networkedPlayer == null ||
            !exiled.HasModifier<LinkedModifier>())
            return;

        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.HasModifier<LinkedModifier>())
            {
                player.RpcSpecialMurder(player, ignoreShield:true, createDeadBody:false, causeOfDeath:"Unbound");
                foreach (var p in PlayerControl.AllPlayerControls)
                {
                    if (p.Data.Role is VinculatorRole)
                    {
                        p.RpcAwardAchievement(AApi.GetInstance()?.VinculatorLinkedVotedOut);
                    }
                }
            }
        }
    }
}