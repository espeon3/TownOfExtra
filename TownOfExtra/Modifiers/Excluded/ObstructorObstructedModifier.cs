using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers.Types;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
using TownOfUs.Buttons;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public sealed class ObstructorObstructedModifier(byte obstructorId) : TimedModifier
{
    public override string ModifierName => "Obstructed";
    public override float Duration => OptionGroupSingleton<ObstructorRoleOptions>.Instance.ObstructDuration;
    public override bool AutoStart => false;
    public override bool HideOnUi => ShouldHideObstructed;
    public byte ObstructorId { get; } = obstructorId;

    public bool ShouldHideObstructed { get; set; } = true;
    private GameObject ReportButtonObstructedSprite { get; set; }
    private GameObject KillButtonObstructedSprite { get; set; }
    private GameObject VentButtonObstructedSprite { get; set; }
    private GameObject UseButtonObstructedSprite { get; set; }
    private GameObject SabotageButtonObstructedSprite { get; set; }
    private List<GameObject> CustomButtonObstructedSprites { get; } = [];

    public override void OnActivate()
    {
        var obstructor = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == ObstructorId);
        if (Player.AmOwner)
        {
            ReportButtonObstructedSprite = HudManager.Instance.ReportButton.CreateObstructedIcon();
            KillButtonObstructedSprite = HudManager.Instance.KillButton.CreateObstructedIcon();
            VentButtonObstructedSprite = HudManager.Instance.ImpostorVentButton.CreateObstructedIcon();
            UseButtonObstructedSprite = HudManager.Instance.UseButton.CreateObstructedIcon();
            SabotageButtonObstructedSprite = HudManager.Instance.SabotageButton.CreateObstructedIcon();

            foreach (var button in CustomButtonManager.Buttons)
            {
                if (button is FakeVentButton)
                {
                    continue;
                }

                CustomButtonObstructedSprites.Add(button!.Button!.CreateObstructedIcon());
            }
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void OnMeetingStart()
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public void ShowObstructed()
    {
        if (!ShouldHideObstructed)
        {
            return;
        }

        ShouldHideObstructed = false;
        var obstructor = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == ObstructorId);

        if (Player.AmOwner)
        {
            ReportButtonObstructedSprite?.SetObstructActive(true);
            KillButtonObstructedSprite?.SetObstructActive(true);
            VentButtonObstructedSprite?.SetObstructActive(true);
            UseButtonObstructedSprite?.SetObstructActive(true);
            SabotageButtonObstructedSprite?.SetObstructActive(true);

            CustomButtonObstructedSprites.Do(x => x.SetObstructActive(true));

            StartTimer();
        }
    }

    public override void OnDeactivate()
    {
        if (Player.AmOwner)
        {
            ReportButtonObstructedSprite?.SetObstructActive(false);
            KillButtonObstructedSprite?.SetObstructActive(false);
            VentButtonObstructedSprite?.SetObstructActive(false);
            UseButtonObstructedSprite?.SetObstructActive(false);
            SabotageButtonObstructedSprite?.SetObstructActive(false);

            CustomButtonObstructedSprites.Do(x => x.SetObstructActive(false));
        }
    }
}