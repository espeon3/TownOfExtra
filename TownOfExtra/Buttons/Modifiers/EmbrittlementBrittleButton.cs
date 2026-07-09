using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Modifiers.Game.Impostor.Utility;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class EmbrittlementBrittleButton : TownOfUsRoleButton<EmbrittlementModifier, PlayerControl>
{
    public override string Name => "Target";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Pallette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<ImpostorModifierOptions>.Instance.BrittleCooldown;
    public override float EffectDuration => OptionGroupSingleton<ImpostorModifierOptions>.Instance.BrittleDuration;
    public override int MaxUses => OptionGroupSingleton<ImpostorModifierOptions>.Instance.BrittleUses;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.PhMisc;

    protected override void OnClick()
    {
        if (Target == null) return;

        Target.RpcAddModifier<EmbrittlementBrittleModifier>();
    }