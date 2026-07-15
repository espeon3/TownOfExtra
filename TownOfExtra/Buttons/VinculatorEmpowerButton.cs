using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class VinculatorEmpowerButton : TownOfUsRoleButton<VinculatorRole>, IDiseaseableButton
{
    public override string Name => "Empower";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<VinculatorRoleOptions>.Instance.EmpowerCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<VinculatorRoleOptions>.Instance.EmpowerUses;
    public int ExtraUses { get; set; }
    public override bool ZeroIsInfinite => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.VinculatorEmpowerButton;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }
    
    public override bool CanUse()
    {
        if (MeetingHud.Instance) return false;
        
        int imps = 0;
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.IsImpostor() && player != PlayerControl.LocalPlayer)
            {
                imps++;
            }
        }

        bool zeroUses = UsesLeft <= 0 && MaxUses != 0;
        return Timer <= 0 && imps > 0 && !zeroUses;
    }

    protected override void OnClick()
    {
        VinculatorRpcs.RpcEmpowerImpostors(PlayerControl.LocalPlayer);
    }
}