using MiraAPI.Modifiers;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class InterviewModifier : BaseModifier
{
    public override string ModifierName => "In Interview";
    public override bool HideOnUi => true;
    
    public bool Active { get; set; } = false;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<InterviewModifier>();
    }
    
    public void OnRoundStart()
    {
        if (!Player.AmOwner)
        {
            return;
        }

        HudManager.Instance.Chat.SetVisible(true);
    }
}