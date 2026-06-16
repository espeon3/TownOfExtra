using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class ErasedModifier : BaseModifier
{
    public override string ModifierName => "Erased";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ErasedModifier>();
    }
}