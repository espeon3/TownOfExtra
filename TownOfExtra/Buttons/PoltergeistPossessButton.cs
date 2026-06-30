using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class PoltergeistPossessedButton : TownOfUsKillRoleButton<PoltergeistRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Possess";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.PoltergeistRoleColour;
    public override float Cooldown => OptionGroupSingleton<PoltergeistRoleOptions>.Instance.PossessCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PoltergeistPossessButton;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    public override PlayerControl GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
                x => !x.IsLover() && x.HasModifier<ScaredModifier>() && !x.HasModifier<PossessedModifier>());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false,
            x => x.HasModifier<ScaredModifier>() && !x.HasModifier<PossessedModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null || Target.HasModifier<PossessedModifier>()) return;
        
        Target.RpcRemoveModifier<ScaredModifier>();
        Target.RpcAddModifier<PossessedModifier>();
        
        var toexAbilityEvent = new TownOfExtraAbilityEvent(AbilityType.PoltergeistPossessPlayer, PlayerControl.LocalPlayer, Target);
        MiraEventManager.InvokeEvent(toexAbilityEvent);
    }
}