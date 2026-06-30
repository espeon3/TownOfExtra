using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class ScaredDebuffsModifier : BaseModifier
{
    public override string ModifierName => "Scared (debuffs)";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ScaredDebuffsModifier>();
    }
}