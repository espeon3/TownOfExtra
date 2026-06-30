using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiraAPI.Events;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Utilities;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.GameOver;
using UnityEngine;

namespace TownOfExtra.Events;

public class PoltergeistEvents
{
    [RegisterEvent]
    public static void OnPoltergeistPossess(TownOfExtraAbilityEvent e)
    {
        if (e.AbilityType != AbilityType.PoltergeistPossessPlayer) return;

        Coroutines.Start(CheckPoltergeistPossess());
    }
    
    private static IEnumerator CheckPoltergeistPossess()
    {
        yield return new WaitForSeconds(0.5f);

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

                foreach (var poltergeist in CustomRoleUtils
                             .GetActiveRolesOfType<PoltergeistRole>()
                             .Where(t => t.WinConditionMet()))
                {
                    winners.Add(poltergeist.Player.Data);
                }

                CustomGameOver.Trigger<NeutralGameOver>(winners);
            }
        }
    }
}