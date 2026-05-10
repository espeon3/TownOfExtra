using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Buttons;

public sealed class TricksterPlaceButton : TownOfUsRoleButton<TricksterRole>
{
    public override string Name => "Place Body";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.TricksterRoleColour;
    public override float Cooldown => BodyPlaced ? 0f : OptionGroupSingleton<TricksterRoleOptions>.Instance.PlaceCooldown;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.TricksterPlaceButton;
    public static bool BodyPlaced;
    
    public static TricksterPlaceButton Instance;
    
    public override bool CanUse()
    {
        return (BodyPlaced || TricksterRole.HasSampledColour) && Timer <= 0;
    }

    public override void ClickHandler()
    {
        if (!CanUse()) return;
        OnClick();
    }

    protected override void OnClick()
    {
        if (BodyPlaced)
        {
            BodyPlaced = false;
            Timer = OptionGroupSingleton<TricksterRoleOptions>.Instance.PlaceCooldown;
    
            if (TricksterRole.SpawnedBodies.Count > 0)
            {
                var oldest = TricksterRole.SpawnedBodies[0];
                if (oldest != null)
                {
                    TricksterRpcs.RpcDestroyFakeBodies(GetTrickster(), oldest.ParentId);
                }
            }
            return;
        }
        BodyPlaced = true;

        var colourName = Palette.GetColorName(TricksterRole.SampledColourId);
        var colour = Palette.PlayerColors[TricksterRole.SampledColourId];
    
        Helpers.CreateAndShowNotification(
            $"Placed fake body with colour <color=#{colour.ToHtmlStringRGBA()}>{colourName}</color>!",
            Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.TricksterPlaceButton.LoadAsset()
        ).AdjustNotification();

        PlayerControl trickster = GetTrickster();
        if (trickster == null) return;
        TricksterRpcs.RpcPlaceFakeBody(trickster, (byte)TricksterRole.SampledColourId, trickster.PlayerId);
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (BodyPlaced)
        {
            OverrideName("Remove Body");
        }
        else if (TricksterRole.HasSampledColour)
        {
            var colourName = Palette.GetColorName(TricksterRole.SampledColourId);
            OverrideName($"Place {colourName}");
        }
        else
        {
            OverrideName("Place Body");
        }
    }
    
    public static DeadBody CreateDeadBody(Vector3 position, byte colorId, byte bodyParentId, PlayerControl colorSource)
    {
        if (colorSource?.Data?.DefaultOutfit == null) return null;
        if (GameManager.Instance?.deadBodyPrefab == null || GameManager.Instance.deadBodyPrefab.Length == 0) return null;

        var baseColorId = colorSource.Data.DefaultOutfit.ColorId;
        colorSource.Data.DefaultOutfit.ColorId = colorId;

        var deadBody = Object.Instantiate(GameManager.Instance.deadBodyPrefab[0]);
        deadBody.ParentId = bodyParentId;

        foreach (var bodyRenderer in deadBody.bodyRenderers)
            colorSource.SetPlayerMaterialColors(bodyRenderer);

        colorSource.SetPlayerMaterialColors(deadBody.bloodSplatter);

        var bodyPosition = position + colorSource.KillAnimations[0].BodyOffset;
        bodyPosition.z = bodyPosition.y / 1000f;
        deadBody.transform.position = bodyPosition;

        colorSource.Data.DefaultOutfit.ColorId = baseColorId;

        deadBody.enabled = true;
    
        return deadBody;
    }
    
    private static PlayerControl GetTrickster()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.IsRole<TricksterRole>())
            {
                return p;
            }
        }

        return null;
    }
}