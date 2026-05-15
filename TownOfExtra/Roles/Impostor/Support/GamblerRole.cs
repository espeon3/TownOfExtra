using System;
using System.Collections;
using System.Collections.Generic;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfExtra.Modifiers.Gambler;
using TownOfExtra.Options.Roles;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Roles.Impostor.Support;

public sealed class GamblerRole : ImpostorRole, ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Gambler";
    public string RoleDescription => "Kill players for random effects";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorSupport;
    public DoomableType DoomHintType => DoomableType.Trickster;

    public string GetAdvancedDescription()
    {
        return
            "The Gambler is an Impostor Support role that gains a new buff or nerf every kill." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TownOfExtraAssets.GamblerRoleIcon
    };

    public override void OnRoleSet()
    {
        if (PlayerControl.LocalPlayer != Player) return;
        Player.RpcAddModifier<NoAbilityModifier>();
    }

    public static void ClearGambleEffect(PlayerControl player)
    {
        if (player.HasModifier<LongerCdModifier>()) player.RpcRemoveModifier<LongerCdModifier>();
        if (player.HasModifier<NoAbilityModifier>()) player.RpcRemoveModifier<NoAbilityModifier>();
        if (player.HasModifier<ShorterCdModifier>()) player.RpcRemoveModifier<ShorterCdModifier>();
        if (player.HasModifier<RotBodyModifier>()) player.RpcRemoveModifier<RotBodyModifier>();
        if (player.HasModifier<TeleportBackModifier>()) player.RpcRemoveModifier<TeleportBackModifier>();
        if (player.HasModifier<NoBodyModifier>()) player.RpcRemoveModifier<NoBodyModifier>();
        if (player.HasModifier<SelfReportModifier>()) player.RpcRemoveModifier<SelfReportModifier>();
        if (player.HasModifier<InvisibilityModifier>()) player.RpcRemoveModifier<InvisibilityModifier>();
    }
    
    public static readonly Dictionary<byte, int> LastEffects = new();

    public static IEnumerator ApplyRandomGambleEffect(PlayerControl player, Action<string> onMessage)
    {
        yield return new WaitForSeconds(0.01f);

        var options = OptionGroupSingleton<GamblerRoleOptions>.Instance;
        var validEffects = new List<int>();

        if (options.NothingEnabled) validEffects.Add(0);
        if (options.LongerCooldownEnabled) validEffects.Add(1);
        if (options.ShorterCooldownEnabled) validEffects.Add(2);
        if (options.ViperBodyEnabled) validEffects.Add(3);
        if (options.TeleportBackEnabled) validEffects.Add(4);
        if (options.NoBodyEnabled) validEffects.Add(5);
        if (options.SelfReportEnabled) validEffects.Add(6);
        if (options.InvisibilityEnabled) validEffects.Add(7);

        if (validEffects.Count == 0)
        {
            player.RpcAddModifier<NoAbilityModifier>();
            onMessage?.Invoke("ERROR - No gamble effects available.");
            yield break;
        }

        if (!options.TwiceInARow &&
            validEffects.Count > 1 &&
            LastEffects.TryGetValue(player.PlayerId, out var last))
        {
            validEffects.Remove(last);
        }

        int choice = validEffects[System.Random.Shared.Next(validEffects.Count)];
        LastEffects[player.PlayerId] = choice;

        string msg = "";

        switch (choice)
        {
            case 0:
                player.RpcAddModifier<NoAbilityModifier>();
                msg = $"Upon your next kill, {Palette.ImpostorRed.ToTextColor()}nothing will happen</color>";
                break;

            case 1:
                player.RpcAddModifier<LongerCdModifier>();
                msg = $"Upon your next kill, you will have a {Palette.ImpostorRed.ToTextColor()}Longer Cooldown</color>";
                break;

            case 2:
                player.RpcAddModifier<ShorterCdModifier>();
                msg = $"Upon your next kill, you will have a {Palette.ImpostorRed.ToTextColor()}Shorter Cooldown</color>";
                break;

            case 3:
                player.RpcAddModifier<RotBodyModifier>();
                msg =
                    $"Your next kill's body will {Palette.ImpostorRed.ToTextColor()}dissolve in {OptionGroupSingleton<GamblerRoleOptions>.Instance.ViperBodyDissolveDuration.Value} seconds</color>";
                break;

            case 4:
                player.RpcAddModifier<TeleportBackModifier>();
                msg =
                    $"You will {Palette.ImpostorRed.ToTextColor()}teleport back to your next kill after {OptionGroupSingleton<GamblerRoleOptions>.Instance.TeleportBackDelay.Value} seconds</color>";
                break;

            case 5:
                player.RpcAddModifier<NoBodyModifier>();
                msg = $"Your next kill will {Palette.ImpostorRed.ToTextColor()}not spawn a dead body</color>";
                break;

            case 6:
                player.RpcAddModifier<SelfReportModifier>();
                msg = $"You will {Palette.ImpostorRed.ToTextColor()}self report</color> your next kill's body";
                break;

            case 7:
                player.RpcAddModifier<InvisibilityModifier>();
                msg = $"You will {Palette.ImpostorRed.ToTextColor()}become invisible</color> for {OptionGroupSingleton<GamblerRoleOptions>.Instance.InvisibilityDuration.Value} seconds after your next kill";
                break;
        }

        onMessage?.Invoke(msg);
    }
}