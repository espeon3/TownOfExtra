using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Buttons;
using TownOfUs.Networking;
using TownOfUs.Options;
using TownOfUs.Options.Maps;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class CannibalEatButton : TownOfUsKillRoleButton<CannibalRole, PlayerControl>, IKillButton, IDiseaseableButton
{
    public override string Name => "Eat";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.CannibalRoleColour;
    public override float Cooldown => OptionGroupSingleton<CannibalRoleOptions>.Instance.IncreaseKcdOnKillOrNot ? OptionGroupSingleton<CannibalRoleOptions>.Instance.KillCooldown + CdIncrease : OptionGroupSingleton<CannibalRoleOptions>.Instance.KillCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.CannibalEatButton;

    public static float CdIncrease;
    
    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

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
        
        CannibalRole.EatenPlayers.Add(Target.PlayerId);
        
        if (OptionGroupSingleton<CannibalRoleOptions>.Instance.IncreaseKcdOnKillOrNot)
        {
            CdIncrease += OptionGroupSingleton<CannibalRoleOptions>.Instance.CdIncreaseOnKill.Value;
        }
        
        PlayerControl.LocalPlayer.RpcSpecialMurder(
            Target,
            createDeadBody: false,
            causeOfDeath: "Cannibalised"
        );
    }
}