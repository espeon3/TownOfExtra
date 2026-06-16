using System.Linq;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfUs.Buttons;
using TownOfUs.Extensions;
using TownOfUs.Modules.Components;
using TownOfUs.Options;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class StrikerStrikeButton : TownOfUsRoleButton<StrikerRole>
{
    public override string Name => "Strike";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<StrikerRoleOptions>.Instance.StrikeCooldown;
    public override float EffectDuration => OptionGroupSingleton<StrikerRoleOptions>.Instance.ImpendingDoomDuration;
    public override bool HasEffect => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.StrikerStrikeButton;

    public override void ClickHandler()
    {
        if (!CanClick() || !CanUse()) return;
        OnClick();
    }

    protected override void OnClick()
    {
        var p = PlayerControl.LocalPlayer;

        var playerMenu = CustomPlayerMenu.Create();
        playerMenu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        playerMenu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        playerMenu.Begin(
            plr => !plr.Data.Disconnected && !plr.Data.IsDead && !plr.AmOwner && !plr.IsImpostor(),
            plr =>
            {
                playerMenu.ForceClose();

                if (plr == null)
                {
                    return;
                }

                var shapeMenu = GuesserMenu.Create();
                
                shapeMenu.Begin(IsRoleValid, role =>
                {
                    var realRole = plr.Data.Role;
                    var cachedMod = plr.GetModifiers<BaseModifier>().FirstOrDefault(x => x is ICachedRole) as ICachedRole;

                    var pickVictim = role.Role == realRole.Role;
                    if (cachedMod != null)
                    {
                        switch (cachedMod.GuessMode)
                        {
                            case CacheRoleGuess.ActiveRole:
                                pickVictim = role.Role == realRole.Role;
                                break;
                            case CacheRoleGuess.CachedRole:
                                pickVictim = role.Role == cachedMod.CachedRole.Role;
                                break;
                            default:
                                pickVictim = role.Role == cachedMod.CachedRole.Role || role.Role == realRole.Role;
                                break;
                        }
                    }

                    var victim = pickVictim ? plr : p;
                    victim.RpcAddModifier<ImpendingDoomModifierv>(victim, victim == p);

                    EffectActive = true;
                    Timer = EffectDuration;
                });
            }
        );

        foreach (var panel in playerMenu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }
    
    public override void OnEffectEnd()
    {
        Timer = Cooldown;
    }
    
    private bool IsRoleValid(RoleBehaviour role)
    {
        if (role.IsDead) return false;
        if (role is IGhostRole) return false;
        if (role is IUnguessable { IsGuessable: false }) return false;
        if (role.GetRoleAlignment() == RoleAlignment.GameOutlier) return false;

        var strikerOpts = OptionGroupSingleton<StrikerRoleOptions>.Instance;
        var alignment = role.GetRoleAlignment();

        if (strikerOpts.ShareAssassinSettings)
        {
            var assassinOpts = OptionGroupSingleton<AssassinOptions>.Instance;

            if (alignment == RoleAlignment.CrewmateInvestigative) return assassinOpts.AssassinGuessInvest;
            if (role.IsCrewmate() && role is ICustomRole) return true;
            if (role.IsCrewmate()) return assassinOpts.AssassinCrewmateGuess;
            if (role.IsImpostor()) return assassinOpts.AssassinGuessImpostors;
            if (alignment == RoleAlignment.NeutralBenign) return assassinOpts.AssassinGuessNeutralBenign;
            if (alignment == RoleAlignment.NeutralEvil) return assassinOpts.AssassinGuessNeutralEvil;
            if (alignment == RoleAlignment.NeutralKilling) return assassinOpts.AssassinGuessNeutralKilling;
            if (alignment == RoleAlignment.NeutralOutlier) return assassinOpts.AssassinGuessNeutralOutlier;

            return false;
        }

        if (alignment == RoleAlignment.CrewmateInvestigative)
            return strikerOpts.StrikerGuessInvest.Value;

        if (role.IsCrewmate() && role is ICustomRole) return true;
        if (role.IsCrewmate()) return strikerOpts.StrikerGuessCrewmate.Value;
        if (role.IsImpostor()) return strikerOpts.StrikerGuessImpostors.Value;
        if (alignment == RoleAlignment.NeutralBenign) return strikerOpts.StrikerGuessNeutralBenign.Value;
        if (alignment == RoleAlignment.NeutralEvil) return strikerOpts.StrikerGuessNeutralEvil.Value;
        if (alignment == RoleAlignment.NeutralKilling) return strikerOpts.StrikerGuessNeutralKilling.Value;
        if (alignment == RoleAlignment.NeutralOutlier) return strikerOpts.StrikerGuessNeutralOutlier.Value;

        return false;
    }
}