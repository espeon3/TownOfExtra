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
        if (Target.HasModifier<FirstDeadShield>() || Target.HasModifier<BaseShieldModifier>())
        {
            if (MaxUses > 0) IncreaseUses();
            return;
        }

        var roleName = Target.GetTownOfUsRole()?.RoleName;
        var touColour = 
            roleName != null
            ? MiscUtils.GetRoleColour(roleName)
            : Color.white;
        var roleColour = 
            touColour == TownOfUsColors.Impostor 
            ? TownOfExtraColours.GetTownOfExtraRoleColour(roleName).ToTextColor()
            : touColour.ToTextColor();

        string c = TownOfUsColors.Crewmate.ToTextColor();
        
        if (Target.IsCrewmate())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Crewmate));
        }
        else if (Target.IsNeutral())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Neutral));
            c = TownOfUsColors.Neutral.ToTextColor();
        }
        else if (Target.IsImpostor())
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfUsColors.Impostor));
            c = TownOfUsColors.Impostor.ToTextColor();
        }
        
        var notif = Helpers.CreateAndShowNotification(
            $"{c}You have shot {Target.Data.PlayerName} and their role was {roleColour}{roleName}{c}!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.ChiefRoleIcon.LoadAsset());
        notif.AdjustNotification();
        
        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target,
            causeOfDeath: "Terminated"
        );
    }
}