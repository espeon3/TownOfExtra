using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;

namespace TownOfExtra.Events;

public class ShadowWalkerEvents
{
    [RegisterEvent]
    public static void OnKill(BeforeMurderEvent e)
    {
        var p = PlayerControl.LocalPlayer;
        
        if (p != e.Source) return;
        if (p.Data.Role is not ShadowWalkerRole) return;
        if (MeetingHud.Instance) return;
        
        if (ShadowWalkerRole.Enshrouded)
        {
            ShadowWalkerRole.CdIncrease += (int)OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.CdIncreasePerEnshroudedKill;
        }
        else
        {
            p.RpcAddModifier<EnshroudedKillModifier>();
        }
    }

    [RegisterEvent]
    public static void OnGameStart(IntroEndEvent e)
    {
        ShadowWalkerRole.CdIncrease = 0;
        ShadowWalkerRole.Enshrouded = false;
    }
}