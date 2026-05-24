using MiraAPI.Modifiers;

namespace TownOfExtra.Modifiers;

public class PendingEraseModifier : BaseModifier
{
    public override string ModifierName => "Pending Erase";
    public override bool HideOnUi => true;
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<PendingEraseModifier>();
    }
}