using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class PreviouslyShiftedModifier : BaseModifier
{
    public override string ModifierName => "Previously Shifted";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<PreviouslyShiftedModifier>();
    }
}