using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Networking;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ChiefShootButton : TownOfUsKillRoleButton<ChiefRole, PlayerControl>, IKillButton
{
    public override string Name => "Shoot";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.ChiefRoleColour;
    public override float Cooldown => OptionGroupSingleton<ChiefRoleOptions>.Instance.ShootCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<ChiefRoleOptions>.Instance.ShootUses;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ChiefShootButton;

    public override PlayerControl GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target,
            causeOfDeath: "Terminated"
        );
    }
}