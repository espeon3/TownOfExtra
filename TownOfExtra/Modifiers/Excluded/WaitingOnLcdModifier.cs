using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class WaitingOnLcdModifier : BaseModifier
{
    public override string ModifierName => "(waiting on) Lucid Dreaming";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<WaitingOnLcdModifier>();
    }
}