using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers;

public class ScaredModifier : BaseModifier
{
    public override string ModifierName => "Scared";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ScaredModifier>();
    }
}