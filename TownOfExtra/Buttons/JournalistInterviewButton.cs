using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Investigative;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class JournalistInterviewButton : TownOfUsRoleButton<JournalistRole, PlayerControl>
{
    public override string Name => "Interview";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.JournalistRoleColour;

    public override float Cooldown => OptionGroupSingleton<JournalistRoleOptions>.Instance.InterviewCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.JournalistInterviewButton;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(
            true,
            Distance,
            predicate: plr =>
                plr != null &&
                plr != PlayerControl.LocalPlayer &&
                !plr.HasDied() &&
                !plr.HasModifier<InterviewModifier>());
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            p.RpcRemoveModifier<InterviewModifier>();
        }
        
        Target.RpcAddModifier<InterviewModifier>();
    }
}