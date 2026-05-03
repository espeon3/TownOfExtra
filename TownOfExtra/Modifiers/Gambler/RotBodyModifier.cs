using System.Collections;
using System.Linq;
using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Modules.Components;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Gambler;

public class RotBodyModifier : BaseModifier
{
    public override string ModifierName => "Gambler Ability";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.GamblerRoleIcon;
    public override bool HideOnUi => false;
    public override Color FreeplayFileColor => Palette.ImpostorRoleHeaderDarkRed;

    public override string GetDescription()
    {
        return $"Your next kill's body will be dissolve in {OptionGroupSingleton<GamblerRoleOptions>.Instance.ViperBodyDissolveDuration.Value} seconds.";
    }
    
    public static IEnumerator StartRotting(PlayerControl player, PlayerControl killer = null)
    {
        var rotting = Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == player.PlayerId);
        if (rotting == null)
        {
            yield break;
        }
        
        CrimeSceneComponent.ClearCrimeScene(rotting);
        Coroutines.Start(CoSetUpRot(rotting, player, killer == null ? player : killer));
    }

    public static IEnumerator CoSetUpRot(DeadBody body, PlayerControl target, PlayerControl killer)
    {
        yield return new WaitForEndOfFrame();
        ViperDeadBody deadBody = Object.Instantiate(GameManager.Instance.deadBodyPrefab[1]).Cast<ViperDeadBody>();
        deadBody.enabled = false;
        deadBody.ParentId = target.PlayerId;
        deadBody.bodyRenderers.Do(x => target.SetPlayerMaterialColors(x));
        target.SetPlayerMaterialColors(deadBody.bloodSplatter);
        deadBody.transform.position = body.transform.position;
        body.ClearBody();
        deadBody.SetupViperInfo(OptionGroupSingleton<GamblerRoleOptions>.Instance.ViperBodyDissolveDuration.Value, killer, target);
        deadBody.enabled = true;
    }
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<RotBodyModifier>();
    }
}