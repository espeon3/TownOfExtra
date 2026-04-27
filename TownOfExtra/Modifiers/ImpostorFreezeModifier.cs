using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers;

public class ImpostorFreezeModifier : TimedModifier
{
    public override string ModifierName => "Freeze Active";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<FreezerRoleOptions>.Instance.FreezeDuration;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.FreezerRoleIcon;

    public override string GetDescription()
    {
        return $"Players are frozen for {TimeRemaining:F1}s!";
    }

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;
        
        Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.FrozenImpModifierColour, Duration));
        var notif = Helpers.CreateAndShowNotification(
            $"Players have been {TownOfExtraColours.FreezerRoleColour.ToTextColor()}frozen</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.FreezerRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
    
    public override void OnDeactivate()
    {
        if (!Player.AmOwner) return;
        var notif = Helpers.CreateAndShowNotification(
            $"Players have been {TownOfExtraColours.FreezerRoleColour.ToTextColor()}unfrozen</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.FreezerRoleIcon.LoadAsset());
        notif.AdjustNotification();
    }
}