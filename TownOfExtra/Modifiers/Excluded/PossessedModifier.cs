using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers.Excluded;

public class PossessedModifier : BaseModifier
{
    public override string ModifierName => "Possessed";
    public override bool HideOnUi => true;

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<PossessedModifier>();
    }
}