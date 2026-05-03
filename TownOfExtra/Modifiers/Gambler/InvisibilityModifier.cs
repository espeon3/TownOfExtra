using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Modifiers.Gambler;

public class InvisibilityModifier : BaseModifier
{
    public override string ModifierName => "Gambler Ability";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.GamblerRoleIcon;
    public override bool HideOnUi => false;
    public override Color FreeplayFileColor => Palette.ImpostorRoleHeaderDarkRed;

    public override string GetDescription()
    {
        return $"After your next kill, you will become invisible for {OptionGroupSingleton<GamblerRoleOptions>.Instance.InvisibilityDuration.Value} seconds.";
    }
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<InvisibilityModifier>();
    }
}