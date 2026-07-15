using System.Linq;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Modules;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfExtra.Buttons;

public sealed class ClownPlaceButton : TownOfUsRoleButton<ClownRole>, IAftermathableButton
{
    public override string Name => "Place";
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.ClownRoleColour;
    public override float Cooldown => OptionGroupSingleton<ClownRoleOptions>.Instance.PlaceCooldown;
    public override float EffectDuration => OptionGroupSingleton<ClownRoleOptions>.Instance.PlaceDelay;
    public override bool ZeroIsInfinite { get; set; } = true;
    public override int MaxUses => (int)OptionGroupSingleton<ClownRoleOptions>.Instance.MaxJackInTheBoxes;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.ClownPlaceButton;

    public Vector2 VentSize { get; set; }
    public Vector3? SavedPos { get; set; }

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        var vents = Object.FindObjectsOfType<Vent>();

        if (vents.Count > 0)
        {
            VentSize = Vector2.Scale(vents[0].GetComponent<BoxCollider2D>().size, vents[0].transform.localScale) *
                       0.75f;
        }
    }

    public override bool CanUse()
    {
        if (!base.CanUse())
        {
            return false;
        }

        var hits = Physics2D.OverlapBoxAll(PlayerControl.LocalPlayer.transform.position, VentSize, 0);

        hits = hits.Where(c =>
            (c.name.Contains("Vent") || c.name.Contains("Door") || !c.isTrigger) && c.gameObject.layer != 8 &&
            c.gameObject.layer != 5).ToArray();

        var noConflict = !PhysicsHelpers.AnythingBetween(PlayerControl.LocalPlayer.Collider,
            PlayerControl.LocalPlayer.Collider.bounds.center, PlayerControl.LocalPlayer.transform.position,
            Constants.ShipAndAllObjectsMask,
            false);

        return hits.Count == 0 && noConflict && !ModCompatibility.GetPlayerElevator(PlayerControl.LocalPlayer).Item1;
    }

    public void AftermathHandler()
    {
        ClickHandler();
    }

    protected override void OnClick()
    {
        SavedPos = PlayerControl.LocalPlayer.transform.position;
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

        var id = GetNextVentId();

        ClownRpcs.RpcPlaceClownVent(PlayerControl.LocalPlayer, id, SavedPos!.Value, SavedPos.Value.z);
        TouAudio.PlaySound(TouAudio.MineSound);
        SavedPos = null;
    }

    public static int GetNextVentId()
    {
        var id = 0;

        while (true)
        {
            if (ShipStatus.Instance.AllVents.All(v => v.Id != id))
            {
                return id;
            }

            id++;
        }
    }
}