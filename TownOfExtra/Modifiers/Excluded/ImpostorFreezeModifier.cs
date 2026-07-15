using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options.Roles;
using TownOfUs.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class ImpostorFreezeModifier : BaseRevealModifier
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
        
        Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.FreezeColour, Duration));
        var notif = Helpers.CreateAndShowNotification(
            $"Players have been {TownOfExtraColours.FreezeColour.ToTextColor()}frozen</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.FreezerFreezeButton.LoadAsset());
        notif.AdjustNotification();
    }
    
    public override void OnDeactivate()
    {
        ExtraNameText = "";
        if (!Player.AmOwner) return;
        var notif = Helpers.CreateAndShowNotification(
            $"Players have been {TownOfExtraColours.FreezeColour.ToTextColor()}unfrozen</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.FreezerFreezeButton.LoadAsset());
        notif.AdjustNotification();
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (MeetingHud.Instance) return;

        ExtraNameText = TimerActive 
            ? $"<br><size=70%>{Palette.CrewmateBlue.ToTextColor()}Frozen: {TimeRemaining:F1}s</color></size>"
            : "";
    }
    
    public override void OnDeath(DeathReason reason)
    {
        if (!Player.AmOwner) return;
        Player.RpcRemoveModifier<ImpostorFreezeModifier>();
    }

    public override void OnMeetingStart()
    {
        Player.RemoveModifier(this);
    }
}