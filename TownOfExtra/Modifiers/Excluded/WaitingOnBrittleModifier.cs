using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class WaitingOnBrittleModifier : BaseModifier
{
    public override string ModifierName => "(waiting on) Brittle";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<WaitingOnBrittleModifier>();
    }
}