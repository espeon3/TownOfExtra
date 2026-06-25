using System.Linq;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Neutral;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ShadowWalkerEnshroudButton  : TownOfUsRoleButton<ShadowWalkerRole>
{
    public override string Name => "Enshroud";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.ShadowWalkerRoleColour;
    public override float Cooldown => OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.EnshroudCooldown;
    public override float EffectDuration => OptionGroupSingleton<ShadowWalkerRoleOptions>.Instance.EnshroudDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ShadowWalkerEnshroudButton;

    public override bool CanUse()
    {
        if (HudManager.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.HasModifier<GlitchHackedModifier>() || PlayerControl.LocalPlayer
                .GetModifiers<DisabledModifier>().Any(x => !x.CanUseAbilities))
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.HasModifier<EnshroudedKillModifier>())
        {
            return false;
        }

        return ((Timer <= 0 && !EffectActive && (!LimitedUses || UsesLeft > 0)) ||
                (EffectActive && Timer <= EffectDuration - 2f));
    }
    
    public override void ClickHandler()
    {
        if (!CanUse())
        {
            return;
        }

        OnClick();
        Button?.SetDisabled();
        if (EffectActive)
        {
            Timer = Cooldown;
            EffectActive = false;
            OnEffectEnd();
        }
        else if (HasEffect)
        {
            EffectActive = true;
            Timer = EffectDuration;
        }
        else
        {
            Timer = Cooldown;
        }
    }
    
    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.RpcAddModifier<EnshroudedModifier>();
    }

    public override void OnEffectEnd()
    {
        if (!PlayerControl.LocalPlayer.HasModifier<EnshroudedModifier>()) return;
        PlayerControl.LocalPlayer.RpcRemoveModifier<EnshroudedModifier>();
    }
}