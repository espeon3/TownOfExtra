using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Events;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class LucidDreamingModifier : BaseModifier
{
    public override string ModifierName => "Lucid Dreaming";

    public override string GetDescription() =>
        "You are having a lucid dream, you cannot use abilities or die whilst in this state." +
        $"\n{(int)OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration - DreamCasterEvents.Rounds[PlayerControl.LocalPlayer]} round{((int)OptionGroupSingleton<DreamCasterRoleOptions>.Instance.CastDuration - DreamCasterEvents.Rounds[PlayerControl.LocalPlayer] != 1 ? "s" : "")} left";

    public override bool HideOnUi => false;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.DreamCasterRoleIcon;

    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<LucidDreamingModifier>();
    }
}