using System.Collections.Generic;
using System.Linq;
using MiraAPI.Events;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.GameOver;

namespace TownOfExtra.Events;

public class PoltergeistEvents
{
    [RegisterEvent]
    public static void OnPoltergeistPossess(TownOfExtraAbilityEvent e)
    {
        if (e.AbilityType != AbilityType.PoltergeistPossessPlayer) return;
        
        int possessedCount = 0;
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<PossessedModifier>()) possessedCount++;
        }

        if (possessedCount >= OptionGroupSingleton<PoltergeistRoleOptions>.Instance.WinPossesses)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                List<NetworkedPlayerInfo> winners = new List<NetworkedPlayerInfo>();
                foreach (var poltergeist in CustomRoleUtils.GetActiveRolesOfType<PoltergeistRole>().Where(t => t.WinConditionMet())) 
                    winners.Add(poltergeist.Player.Data);
                CustomGameOver.Trigger<NeutralGameOver>(winners);
            }
        }
    }
}