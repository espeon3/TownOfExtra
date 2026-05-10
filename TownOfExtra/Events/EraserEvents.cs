using AmongUs.GameOptions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Modifiers;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Events;

public class EraserEvents
{
    [RegisterEvent(100)]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.HasModifier<ErasedModifier>() && !p.Data.IsDead)
            {
                if (PlayerControl.LocalPlayer == p)
                {
                    Coroutines.Start(MiscUtils.CoFlash(Palette.ImpostorRed));
                    var notif = Helpers.CreateAndShowNotification(
                        $"Your role has been erased by the {Palette.ImpostorRed.ToTextColor()}eraser</color>!",
                        Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.EraserEraseButton.LoadAsset());
                    notif.AdjustNotification();
                }
                p.RpcSetRole(RoleTypes.Crewmate);
                p.RpcRemoveModifier<ErasedModifier>();
            }
        }
    }
}