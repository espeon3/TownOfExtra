using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public class LinkedModifier(PlayerControl with) : BaseModifier
{
    public override string ModifierName => "Linked";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.VinculatorChainButton;

    public override string GetDescription()
    {
        return $"Your fate is linked with {with.Data.PlayerName}!";
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;
        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor));
        var notif = Helpers.CreateAndShowNotification(
            $"Your fate has been {TownOfUsColors.Impostor.ToTextColor()}linked</color> with {with.Data.PlayerName}!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VinculatorChainButton.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnDeactivate()
    {
        if (!Player.AmOwner) return;
        Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor));
        var notif = Helpers.CreateAndShowNotification(
            $"Your fate is no longer {TownOfUsColors.Impostor.ToTextColor()}linked</color> with {with.Data.PlayerName}!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VinculatorChainButton.LoadAsset());
        notif.AdjustNotification();
    }

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<LinkedModifier>();
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            p.RpcRemoveModifier<LinkedModifier>();
        }
    }
}