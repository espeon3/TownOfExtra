using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class TricksterSampleButton : TownOfUsRoleButton<TricksterRole, PlayerControl>
{
    public override string Name => "Sample";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.TricksterRoleColour;

    public override float Cooldown => OptionGroupSingleton<TricksterRoleOptions>.Instance.SampleCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.TricksterSampleButton;

    public override PlayerControl GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(
            true,
            Distance,
            predicate: plr =>
                plr != null &&
                plr != PlayerControl.LocalPlayer &&
                !plr.HasDied());
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        var colourId = Target.Data.DefaultOutfit.ColorId;
        TricksterRole.SampledColourId = colourId;
        TricksterRole.HasSampledColour = true;
        
        var colourName = Palette.GetColorName(colourId);
        var colour = Palette.PlayerColors[colourId];

        var notif = Helpers.CreateAndShowNotification(
            $"Sampled colour <color=#{colour.ToHtmlStringRGBA()}>{colourName}</color> from {Target.Data.PlayerName}!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterSampleButton.LoadAsset()
        );
        notif.AdjustNotification();
    }
}