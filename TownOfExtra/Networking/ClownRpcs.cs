using System.Linq;
using MiraAPI.Events;
using Reactor.Networking.Attributes;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfUs;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public static class ClownRpcs
{
    [MethodRpc((uint)TownOfUsRpc.PlaceVent)]
    public static void RpcPlaceClownVent(PlayerControl player, int ventId, Vector2 position, float zAxis)
    {
        if (player.Data.Role is not ClownRole clown) return;

        var ventPrefab = ShipStatus.Instance.AllVents[0];
        var vent = Object.Instantiate(ventPrefab, ventPrefab.transform.parent);
        vent.EnterVentAnim = null!;
        vent.ExitVentAnim = null!;
        if (vent.myAnim)
        {
            vent.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            var collider = vent.transform.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.75f, 0.34f);
            collider.offset = new Vector2(-0.005f, 0);
            vent.Offset = new Vector3(0, 0.15f, 0);
            vent.myAnim.Stop();
            Object.Destroy(vent.myAnim);
            vent.myAnim = null!;
        }

        vent.numFramesUntilPlayerDisappearsOnEnter = 0;
        vent.numFramesUntilPlayerReappearsOnExit = 0;
        vent.myRend.sprite = TownOfExtraAssets.ClownJackInTheBox.LoadAsset();
        vent.name = $"ClownJackInTheBox-{player.PlayerId}-{ventId}";

        if (!player.AmOwner) vent.gameObject.SetActive(false);

        vent.Id = ventId;
        vent.transform.position = new Vector3(position.x, position.y, zAxis + 0.001f);

        if (clown == null)
        {
            return;
        }

        if (clown.Vents.HasAny())
        {
            var leftVent = clown.Vents[^1];
            vent.Left = leftVent;
            leftVent.Right = vent;
        }
        else
        {
            vent.Left = null;
        }

        vent.Right = null;
        vent.Center = null;

        var allVents = ShipStatus.Instance.AllVents.ToList();
        allVents.Add(vent);
        ShipStatus.Instance.AllVents = allVents.ToArray();

        clown.Vents.Add(vent);

        if (ModCompatibility.SubLoaded)
        {
            vent.gameObject.layer = 12;
            vent.gameObject.AddSubmergedComponent("ElevatorMover"); // just in case elevator vent is not blocked
            if (vent.gameObject.transform.position.y > -7)
            {
                vent.gameObject.transform.position = new Vector3(vent.gameObject.transform.position.x,
                    vent.gameObject.transform.position.y, 0.02f);
            }
            else
            {
                vent.gameObject.transform.position = new Vector3(vent.gameObject.transform.position.x,
                    vent.gameObject.transform.position.y, 0.0009f);
                vent.gameObject.transform.localPosition = new Vector3(vent.gameObject.transform.localPosition.x,
                    vent.gameObject.transform.localPosition.y, -0.003f);
            }
        }
    }
}