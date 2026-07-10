using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class WaitingOnShiftModifier : BaseModifier
{
    public override string ModifierName => "(waiting on) Shift";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<WaitingOnShiftModifier>();
    }
}