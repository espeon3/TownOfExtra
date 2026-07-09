using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class EmbrittlementBrittleButton : TownOfUsRoleButton<EmbrittlementRole, PlayerControl>
{
    public override string Name => "Brittle";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Pallette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<EmbrittlementRoleOptions>.Instance.BrittleCooldown;
    public override float EffectDuration => OptionGroupSingleton<EmbrittlementRoleOptions>.Instance.BrittleDuration;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.Phmisc;
    
    protected override void OnClick()
    {
        if (Target == null) return;

        Target.RpcAddModifier<BrittleModifier>();
    }