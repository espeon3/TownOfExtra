using System.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Modifiers.Gambler;

public class TeleportBackModifier : BaseModifier
{
    public override string ModifierName => "Gambler Ability";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.GamblerRoleIcon;
    public override bool HideOnUi => false;
    public override Color FreeplayFileColor => Palette.ImpostorRoleHeaderDarkRed;

    public override string GetDescription()
    {
        return $"You will teleport back to your next kill after {OptionGroupSingleton<GamblerRoleOptions>.Instance.TeleportBackDelay.Value} seconds.";
    }
    
    public static IEnumerator StartDelay(PlayerControl target, PlayerControl killer, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var killPosition = target.transform.position;
        if (killer == null || killer.Data.IsDead) yield break;

        killer.transform.position = killPosition;
    }
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<TeleportBackModifier>();
    }
}