using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class VinculatorChainButton : TownOfUsRoleButton<VinculatorRole>, IDiseaseableButton
{
    public override string Name => "Chain";
    public override BaseKeybind Keybind => Keybinds.TertiaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<VinculatorRoleOptions>.Instance.ChainCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<VinculatorRoleOptions>.Instance.ChainUses;
    public int ExtraUses { get; set; }
    public override bool ZeroIsInfinite => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.VinculatorChainButton;
    
    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    public override bool CanUse()
    {
        bool zeroUses = UsesLeft <= 0 && MaxUses != 0;
        return Timer <= 0 && !zeroUses;
    }
    
    public override void ClickHandler()
    {
        if (!CanClick()) return;
        OnClick();
    }

    protected override void OnClick()
    {
        var player1Menu = CustomPlayerMenu.Create();
        player1Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        player1Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        player1Menu.Begin(
            plr => !plr.Data.Disconnected && !plr.Data.IsDead && !plr.AmOwner && !plr.IsImpostor(),
            plr =>
            {
                player1Menu.ForceClose();

                if (plr == null)
                {
                    return;
                }

                var player2Menu = CustomPlayerMenu.Create();
                player2Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
                    PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
                player2Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
                    PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

                player2Menu.Begin(
                    plr2 => !plr2.Data.Disconnected && !plr2.Data.IsDead && !plr2.AmOwner && !plr2.IsImpostor() && plr2 != plr,
                    plr2 =>
                    {
                        player2Menu.Close();
                        if (plr2 == null)
                        {
                            return;
                        }

                        var notif = Helpers.CreateAndShowNotification(
                            $"You have {TownOfUsColors.Impostor.ToTextColor()}linked</color> the fate of {plr.Data.PlayerName} and {plr2.Data.PlayerName}!",
                            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.VinculatorChainButton.LoadAsset());
                        notif.AdjustNotification();
                        
                        plr.RpcAddModifier<LinkedModifier>(plr2);
                        plr2.RpcAddModifier<LinkedModifier>(plr);
                        
                        SetTimer(Cooldown);
                        if (MaxUses != 0)
                        {
                            UsesLeft--;
                            Button?.SetUsesRemaining(UsesLeft);
                        }
                    }
                );
                foreach (var panel in player2Menu.potentialVictims)
                {
                    panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
                    if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
                    {
                        panel.NameText.color = Color.white;
                    }
                }
            }
        );
        foreach (var panel in player1Menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }
}