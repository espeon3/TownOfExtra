using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class ShiftedModifier : BaseModifier
{
    public override string ModifierName => "Shifted";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ShiftedModifier>();
    }
}