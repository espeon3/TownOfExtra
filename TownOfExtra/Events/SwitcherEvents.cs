using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfExtra.Buttons;
using TownOfExtra.Modifiers;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Events;

public class SwitcherEvents
{
    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent e)
    {
        var switcher = GetSwitcher();

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (switcher == null || p == null || p.Data.IsDead || switcher.Data.IsDead)
            {
                if (switcher != null && PlayerControl.LocalPlayer == switcher)
                {
                    Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.SwitcherRoleColour));
                    var notif = Helpers.CreateAndShowNotification(
                        $"Your {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color> target is no longer alive!",
                        Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SwitcherRoleIcon.LoadAsset());
                    notif.AdjustNotification();
                }
                SwitcherSwitchButton.ButtonDisabled = false;
                return;
            }

            if (p.HasModifier<SwitchedModifier>())
            {
                var pRole = p.Data.Role.Role;
                var switcherRole = switcher.Data.Role.Role;

                p.RpcSetRole(switcherRole);
                switcher.RpcSetRole(pRole);

                if (PlayerControl.LocalPlayer == p)
                {
                    Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.SwitcherRoleColour));
                    var notif = Helpers.CreateAndShowNotification(
                        $"Your role has been switched by the {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switcher</color>!",
                        Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SwitcherRoleIcon.LoadAsset());
                    notif.AdjustNotification();
                }
                else if (PlayerControl.LocalPlayer == switcher)
                {
                    Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.SwitcherRoleColour));
                    var notif = Helpers.CreateAndShowNotification(
                        $"You have {TownOfExtraColours.SwitcherRoleColour.ToTextColor()}switched</color> your role with {p.name}!",
                        Color.white, new Vector3(0f, 1f, -20f), spr: TownOfExtraAssets.SwitcherRoleIcon.LoadAsset());
                    notif.AdjustNotification();
                }
            }
        }
    }

    public static PlayerControl GetSwitcher()
    {
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role is SwitcherRole)
            {
                return p;
            }
        }

        return null;
    }
}