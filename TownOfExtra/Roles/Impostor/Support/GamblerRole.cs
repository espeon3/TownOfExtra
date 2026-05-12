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

    public static IEnumerator ApplyRandomGambleEffect(PlayerControl player)
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
            yield break;
        }

        if (!options.TwiceInARow && validEffects.Count > 1 && LastEffects.TryGetValue(player.PlayerId, out var last))
        {
            validEffects.Remove(last);
        }

        int choice = validEffects[System.Random.Shared.Next(validEffects.Count)];
        LastEffects[player.PlayerId] = choice;

        switch (choice)
        {
            case 0: player.RpcAddModifier<NoAbilityModifier>(); break;
            case 1: player.RpcAddModifier<LongerCdModifier>(); break;
            case 2: player.RpcAddModifier<ShorterCdModifier>(); break;
            case 3: player.RpcAddModifier<RotBodyModifier>(); break;
            case 4: player.RpcAddModifier<TeleportBackModifier>(); break;
            case 5: player.RpcAddModifier<NoBodyModifier>(); break;
            case 6: player.RpcAddModifier<SelfReportModifier>(); break;
            case 7: player.RpcAddModifier<InvisibilityModifier>(); break;
        }
    }
}