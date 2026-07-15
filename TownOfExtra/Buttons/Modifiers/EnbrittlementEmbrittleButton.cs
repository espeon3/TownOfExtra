using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Modifiers.Game.Impostor.Utility;
using TownOfExtra.Networking.Global;
using TownOfExtra.Options;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons.Modifiers;

public sealed class EmbrittlementEmbrittleButton : TownOfUsTargetButton<PlayerControl>
{
    public override string Name => "Embrittle";
    public override BaseKeybind Keybind => Keybinds.ModifierAction;
    public override Color TextOutlineColor => TownOfExtraColours.ShockwaveModifierColour;
    public override float Cooldown => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveCooldown.Value;
    public override int MaxUses => OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveUses;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ShockwaveShockwaveButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return PlayerControl.LocalPlayer &&
               PlayerControl.LocalPlayer.HasModifier<EmbrittlementModifier>() &&
               !PlayerControl.LocalPlayer.Data.IsDead;
    }

    public override PlayerControl GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    public override void SetOutline(bool active)
    {
        if (Target != null && !PlayerControl.LocalPlayer.HasDied())
        {
            Target.cosmetics.currentBodySprite.BodySprite.SetOutline(active ? TextOutlineColor : null);
        }
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSendNotification(
            $"{Target.Data.PlayerName} will be {TownOfExtraColours.EmbrittlementModifierColour.ToTextColor()}embrittled</color> after the next meeting.",
            "EmbrittlementModifierIcon",
            "ImpModIcon",
            200
        );
        
        if (!Target.HasModifier<WaitingOnBrittleModifier>() && !Target.HasModifier<BrittleModifier>()) {
            Target.RpcAddModifier<WaitingOnBrittleModifier>();
        }
    }
}