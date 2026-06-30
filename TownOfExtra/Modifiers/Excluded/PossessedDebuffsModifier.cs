using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class PossessedDebuffsModifier : BaseModifier
{
    public override string ModifierName => "Possessed (debuffs)";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<PossessedDebuffsModifier>();
    }
}