using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using System.Collections;
using System;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfExtra.Roles.Neutral.Killing;
using TownOfExtra.Options.Roles;
using TownOfExtra.Modules;
using TownOfExtra.Networking;

namespace TownOfExtra.Buttons;

public sealed class CannibalEatButton : TownOfUsRoleButton<CannibalRole, PlayerControl>, IDiseaseableButton
{
    public override string Name => "Eat";
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => TownOfExtraColours.CannibalRoleColour;
    public override float Cooldown => Math.Clamp(
        OptionGroupSingleton<CannibalRoleOptions>.Instance.SwallowCooldown + MapCooldown, 5f, 120f);
    public override float EffectDuration => 0f;
    public override bool HasEffect => false;
    public override LoadableAsset<Sprite> Sprite => TownOfExtraAssets.CannibalEatButton;

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }


    public override int MaxUses =>
        Mathf.Max(1, (int)OptionGroupSingleton<CannibalRoleOptions>.Instance.MaxSwallowed);

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Reactor.Utilities.Coroutines.Start(CoMoveWithDelay());

        if (Button != null)
        {
            Button.usesRemainingSprite.sprite = TouAssets.AbilityCounterBodySprite.LoadAsset();
            Button.usesRemainingText.gameObject.SetActive(true);
            Button.usesRemainingSprite.gameObject.SetActive(true);
        }

        RefreshUses();
    }

    private IEnumerator CoMoveWithDelay()
    {
        yield return null;
        yield return MiscUtils.CoMoveButtonIndex(this, false);
    }

    private void RefreshUses()
    {
        var player = PlayerControl.LocalPlayer;
        if (player == null) return;

        int max = Mathf.Max(1, (int)OptionGroupSingleton<CannibalRoleOptions>.Instance.MaxSwallowed);
        int currentCount = CannibalSystem.GetSwallowedByCannibal(player.PlayerId).Count;
        int remaining = Mathf.Clamp(max - currentCount, 0, max);

        SetUses(remaining);
    }

    public override PlayerControl? GetTarget()
    {
        var player = PlayerControl.LocalPlayer;
        if (player == null) return null;

        PlayerControl? bestTarget = null;
        float bestDistance = float.MaxValue;
        var myPos = player.GetTruePosition();

        foreach (var other in PlayerControl.AllPlayerControls)
        {
            if (other == null) continue;
            if (other.PlayerId == player.PlayerId) continue;
            if (other.Data == null) continue;
            if (other.Data.Disconnected) continue;
            if (other.HasDied()) continue;
            if (CannibalSystem.IsSwallowed(other.PlayerId)) continue;

            float dist = Vector2.Distance(myPos, other.GetTruePosition());
            if (dist > Distance) continue;

            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestTarget = other;
            }
        }

        return bestTarget;
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        if (target == null || target.HasDied() || target.Data == null) return false;
        if (target.Data.Disconnected) return false;
        if (CannibalSystem.IsSwallowed(target.PlayerId)) return false;

        return base.IsTargetValid(target);
    }

    public override bool CanUse()
    {
        if (!base.CanUse()) return false;

        var player = PlayerControl.LocalPlayer;
        if (player == null) return false;

        var options = OptionGroupSingleton<CannibalRoleOptions>.Instance;
        var currentCount = CannibalSystem.GetSwallowedByCannibal(player.PlayerId).Count;
        return currentCount < (int)options.MaxSwallowed && Timer <= 0;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        base.FixedUpdate(playerControl);
        RefreshUses();

        if (playerControl != null)
        {
            var target = GetTarget();
            var max = Mathf.Max(1, (int)OptionGroupSingleton<CannibalRoleOptions>.Instance.MaxSwallowed);
            var currentCount = CannibalSystem.GetSwallowedByCannibal(playerControl.PlayerId).Count;
            bool hasRoom = currentCount < max;
            bool shouldBeBright = target != null && !playerControl.HasDied() && hasRoom;

            SetButtonState(shouldBeBright);
        }
    }

    private void SetButtonState(bool shouldBeBright)
    {
        if (Button == null) return;

        if (Button.cooldownTimerText != null && Button.cooldownTimerText.gameObject.activeSelf)
        {
            Button.cooldownTimerText.color = Color.white;
        }

        if (Button.buttonLabelText != null)
        {
            Button.buttonLabelText.color = shouldBeBright ? Color.white : new Color(1f, 1f, 1f, 0.5f);
        }

        if (Button.graphic != null)
        {
            float alpha = shouldBeBright ? 1f : 0.5f;
            Button.graphic.color = new Color(1f, 1f, 1f, alpha);
            if (Button.graphic.material != null)
            {
                Button.graphic.material.SetFloat("_Desat", shouldBeBright ? 0f : 1f);
            }
        }
    }

    public override void ClickHandler()
    {
        if (!CanClick()) return;
        if (Target == null) return;

        var player = PlayerControl.LocalPlayer;
        if (player == null) return;

        var beforeMurderEvent = new BeforeMurderEvent(player, Target, MeetingCheck.OutsideMeeting);
        MiraEventManager.InvokeEvent(beforeMurderEvent);

        var canSwallowThroughShields = OptionGroupSingleton<CannibalRoleOptions>.Instance.CanSwallowThroughShields;
        if (beforeMurderEvent.IsCancelled && !canSwallowThroughShields)
        {
            RefreshUses();
            return;
        }

        OnClick();
        Timer = Cooldown;
        RefreshUses();
    }

    protected override void OnClick()
    {
        var player = PlayerControl.LocalPlayer;
        if (player == null || Target == null) return;

        try
        {
            CannibalRpcs.RpcCannibalSwallow(player, Target.PlayerId);
        }
        catch (System.Exception ex)
        {
            Logger<TownOfExtraPlugin>.Error($"[PelicanSwallow] RPC error: {ex.Message}");
        }
    }
}














