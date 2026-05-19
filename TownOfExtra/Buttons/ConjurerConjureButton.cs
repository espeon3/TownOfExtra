using System.Collections;
using MiraAPI.GameOptions;
using TownOfExtra.Roles.Impostor.Power;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class ConjurerConjureButton : TownOfUsRoleButton<ConjurerRole>
{
    public override string Name => "Conjure";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => Palette.ImpostorRed;
    public override float Cooldown => OptionGroupSingleton<ConjurerRoleOptions>.Instance.ConjureCooldown;
    public override float EffectDuration => OptionGroupSingleton<ConjurerRoleOptions>.Instance.ConjureDuration;
    public override bool HasEffect => true;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ConjurerConjureButton;

    private bool _placing;
    private bool _fallen;
    private GameObject _preview;
    
    public override void ClickHandler()
    {
        if (!CanClick()) return;
        OnClick();
    }
    
    public override bool CanUse()
    {
        return Timer <= 0 && !_placing && !PlayerControl.LocalPlayer.inVent;
    }

    protected override void OnClick()
    {
        Coroutines.Start(StartPlacing());
    }

    private IEnumerator StartPlacing()
    {
        var camera = Camera.main;
        if (camera == null) yield break;
        
        EnterPlacingMode();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            while (true)
            {
                if (PlayerControl.LocalPlayer.inVent)
                {
                    ExitPlacingMode();
                    yield return null;
                }
                
                var screenToWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = screenToWorldPoint;
                    loc.z = 0f;
                    _preview.transform.position = loc;
                }

                if (Input.touchCount == 2)
                {
                    if (_fallen)
                    {
                        _fallen = false;
                        var renderer = _preview.GetComponent<SpriteRenderer>();
                        renderer.sprite = TownOfExtraAssets.ConjurerRockSprite.LoadAsset();
                    }
                    else
                    {
                        _fallen = true;
                        var renderer = _preview.GetComponent<SpriteRenderer>();
                        renderer.sprite = TownOfExtraAssets.ConjurerRockSpriteFallen.LoadAsset();
                    }
                }

                if (Input.touchCount == 3)
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.touchCount == 1)
                {
                    screenToWorldPoint.z = 0f;

                    ConjurerRpcs.RpcPlaceRock(PlayerControl.LocalPlayer, screenToWorldPoint.x, screenToWorldPoint.y, _fallen);
                    
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

                var screenToWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);

                if (_preview != null)
                {
                    var loc = screenToWorldPoint;
                    loc.z = 0f;
                    _preview.transform.position = loc;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (_fallen)
                    {
                        _fallen = false;
                        var renderer = _preview.GetComponent<SpriteRenderer>();
                        renderer.sprite = TownOfExtraAssets.ConjurerRockSprite.LoadAsset();
                    }
                    else
                    {
                        _fallen = true;
                        var renderer = _preview.GetComponent<SpriteRenderer>();
                        renderer.sprite = TownOfExtraAssets.ConjurerRockSpriteFallen.LoadAsset();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitPlacingMode();
                    yield break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    screenToWorldPoint.z = 0f;

                    ConjurerRpcs.RpcPlaceRock(PlayerControl.LocalPlayer, screenToWorldPoint.x, screenToWorldPoint.y, _fallen);
                    
                    ExitPlacingMode();
                    
                    EffectActive = true;
                    Timer = EffectDuration;
                    
                    yield break;
                }

                yield return null;
            }
        }
    }

    private void EnterPlacingMode()
    {
        OverrideName("Conjuring...");
        _placing = true;
        _fallen = false;

        Vector3 loc = Camera.main != null ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
        
        loc.z = 0f;

        _preview = new GameObject();
        _preview.transform.position = loc;

        var renderer = _preview.AddComponent<SpriteRenderer>();
        renderer.sprite = TownOfExtraAssets.ConjurerRockSprite.LoadAsset();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
    }

    private void ExitPlacingMode()
    {
        OverrideName("Conjure");
        _placing = false;

        if (_preview != null)
        {
            Object.Destroy(_preview);
            _preview = null;
        }
    }
    
    public override void OnEffectEnd()
    {
        Timer = Cooldown;
    }
}