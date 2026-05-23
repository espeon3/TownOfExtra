using MiraAPI.Events;
using UnityEngine;

namespace TownOfExtra.Events.Custom;

public class TownOfExtraAbilityEvent : MiraEvent
{
    public TownOfExtraAbilityEvent(AbilityType ability, string result, PlayerControl player, MonoBehaviour target = null,
        MonoBehaviour target2 = null)
    {
        AbilityType = ability;
        Player = player;
        Target = target;
        Target2 = target2;
        Result = result;
    }
    
    public TownOfExtraAbilityEvent(AbilityType ability, PlayerControl player, MonoBehaviour target = null,
        MonoBehaviour target2 = null)
    {
        AbilityType = ability;
        Player = player;
        Target = target;
        Target2 = target2;
        Result = "No Information";
    }

    public PlayerControl Player { get; }
    public MonoBehaviour Target { get; set; }
    public MonoBehaviour Target2 { get; set; }
    public AbilityType AbilityType { get; }
    public string Result { get; }
}

public enum AbilityType
{
    VultureEatBody,
    PoltergeistPossessPlayer,
}