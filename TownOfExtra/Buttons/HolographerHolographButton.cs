using System.Collections;
using TownOfExtra.Roles.Impostor.Concealing;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Modules;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class HolographerHolographButton : TownOfUsRoleButton<HolographerRole>
{
    public override string Name => "Holograph";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<HolographerRoleOptions>.Instance.HolographCooldown;
    public override float EffectDuration => OptionGroupSingleton<HolographerRoleOptions>.Instance.HologramDuration;
    public override bool HasEffect => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.HolographerHolographButton;

    private bool _placing;
    private CustomLocFakePlayer _preview;
    private bool _inMenu;
    
    public override void ClickHandler()
    {
        if (!CanClick()) return;
        OnClick();
    }
    
    public override bool CanUse()
    {
        return Timer <= 0 && !_placing && !PlayerControl.LocalPlayer.inVent && !_inMenu;
    }

    protected override void OnClick()
    {
        _inMenu = true;
        
        var menu = CustomPlayerMenu.Create();
        menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        if (OptionGroupSingleton<HolographerRoleOptions>.Instance.CanHolograph == CanHolographOptions.Dead)
        {
            menu.Begin(
                plr => plr.Data.IsDead,
                plr =>
                {
                    menu.ForceClose();
                    _inMenu = false;
                    if (plr != null)
                    {
                        Coroutines.Start(StartPlacing(plr));
                    }
                }
            );
        }
        else
        {
            menu.Begin(
                plr => plr.name != "",
                plr =>
                {
                    menu.ForceClose();
                    _inMenu = false;
                    if (plr != null)
                    {
                        Coroutines.Start(StartPlacing(plr));
                    }
                }
            );
        }

        foreach (var panel in menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }

    private IEnumerator StartPlacing(PlayerControl target)
    {
        var camera = Camera.main;
        if (camera == null) yield break;
        
        EnterPlacingMode(target);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            while (true)
            {
                if (PlayerControl.LocalPlayer.inVent)
                {
                    ExitPlacingMode();
                    yield return null;
                }
                
                var stwp = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = stwp;
                    loc.z = 0f;
                    _preview.location = loc;
                }

                if (Input.touchCount == 3)
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.touchCount == 2)
                {
                    ExitPlacingMode();
                    OnClick();
                    yield break;
                }

                if (Input.touchCount == 1)
                {
                    stwp.z = 0f;
                    
                    HolographerRpcs.RpcSpawnFakePlayer(target, stwp.x, stwp.y, stwp.z);
                    
                    ExitPlacingMode();
                    
                    EffectActive = true;
                    Timer = EffectDuration;
                    
                    yield break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                if (PlayerControl.LocalPlayer.inVent)
                {
                    ExitPlacingMode();
                    yield return null;
                }

                var stwp = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = stwp;
                    loc.z = 0f;
                    _preview.location = loc;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    ExitPlacingMode();
                    OnClick();
                    yield break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    stwp.z = 0f;

                    HolographerRpcs.RpcSpawnFakePlayer(target, stwp.x, stwp.y, stwp.z);
                    
                    ExitPlacingMode();
                    
                    EffectActive = true;
                    Timer = EffectDuration;
                    
                    yield break;
                }

                yield return null;
            }
        }
    }

    private void EnterPlacingMode(PlayerControl target)
    {
        OverrideName("Holographing...");
        _placing = true;

        Vector3 loc = Camera.main != null ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
        
        loc.z = 0f;

        _preview = new CustomLocFakePlayer(target, loc);
        _preview.opacity = 0.5f;
    }

    private void ExitPlacingMode()
    {
        OverrideName("Holograph");
        _placing = false;

        if (_preview != null)
        {
            _preview.Destroy();
            _preview = null;
        }
    }

    public override void OnEffectEnd()
    {
        Timer = Cooldown;
    }
}