using TownOfUs.Utilities;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace TownOfExtra.Networking;

public static class GlobalRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.SendNotification)]
    public static void RpcSendNotification(this PlayerControl p, string msg, string spriteName, string spriteType, int ppu = 100, Color flashColour = default)
    {
        if (p == null ||
            PlayerControl.LocalPlayer != p ||
            msg == null ||
            spriteName == null ||
            spriteType == null
           ) return;

        if (flashColour != default)
        {
            Coroutines.Start(MiscUtils.CoFlash(flashColour));
        }

        var path = spriteType switch
        {
            "CrewRoleIcon" => TownOfExtraAssets.CrewRoleIconPath,
            "CrewButton" => TownOfExtraAssets.CrewButtonPath,
            "CrewMisc" => TownOfExtraAssets.CrewMiscPath,
            "ImpRoleIcon" => TownOfExtraAssets.ImpRoleIconPath,
            "ImpButton" => TownOfExtraAssets.ImpButtonPath,
            "ImpMisc" => TownOfExtraAssets.ImpMiscPath,
            "NeutRoleIcon" => TownOfExtraAssets.NeutRoleIconPath,
            "NeutButton" => TownOfExtraAssets.NeutButtonPath,
            "NeutMisc" => TownOfExtraAssets.NeutMiscPath,
            "CrewModIcon" => TownOfExtraAssets.CrewModModIconPath,
            "CrewModButton" => TownOfExtraAssets.CrewModButtonPath,
            "CrewModMisc" => TownOfExtraAssets.CrewModMiscPath,
            "ImpModIcon" => TownOfExtraAssets.ImpModModIconPath,
            "ImpModButton" => TownOfExtraAssets.ImpModButtonPath,
            "ImpModMisc" => TownOfExtraAssets.ImpModMiscPath,
            "NeutModIcon" => TownOfExtraAssets.NeutModModIconPath,
            "NeutModButton" => TownOfExtraAssets.NeutModButtonPath,
            "NeutModMisc" => TownOfExtraAssets.NeutModMiscPath,
            "UniModIcon" => TownOfExtraAssets.UniModModIconPath,
            "UniModButton" => TownOfExtraAssets.UniModButtonPath,
            "UniModMisc" => TownOfExtraAssets.UniModMiscPath,
            "Misc" => TownOfExtraAssets.MiscPath,
            _ => "TownOfExtra.Resources"
        };

        var sprite = new LoadableResourceAsset($"{path}.{spriteName}.png", ppu);

        var notif = Helpers.CreateAndShowNotification(
            msg,
            Color.white, new Vector3(0f, 1f, -20f), spr: sprite.LoadAsset());
        notif.AdjustNotification();
    }
}