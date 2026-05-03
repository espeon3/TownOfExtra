using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfExtra.Modifiers.Gambler;

public class ShorterCdModifier : BaseModifier
{
    public override string ModifierName => "Gambler Ability";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.GamblerRoleIcon;
    public override bool HideOnUi => false;
    public override Color FreeplayFileColor => Palette.ImpostorRoleHeaderDarkRed;

    public override string GetDescription()
    {
        return "Your kill cooldown will be shorter after your next kill.";
    }
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ShorterCdModifier>();
    }
}