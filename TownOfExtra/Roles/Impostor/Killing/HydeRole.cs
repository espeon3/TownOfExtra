using Il2CppInterop.Runtime.Attributes;
using System;
using AmongUs.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Hud;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using DivaniMods.Assets;
using DivaniMods.Modifiers.Impostor;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace DivaniMods.Roles.Impostor.ImpostorKilling;

public sealed class HydeRole(IntPtr cppPtr)
    : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ISpawnChange, IGuessable
{
    public bool CanBeGuessed =>
        RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<HydeRole>()) is ICustomRole hyde &&
        (int)hyde.GetCount()! > 0 && (int)hyde.GetChance()! > 0;

    public string RoleName => "Hyde";
    public string RoleDescription => "You were recruited! Pick your new role.";
    public string RoleLongDescription =>
        "Win by killing all\n" + "wait one emergency meeting to return to Jekyll form.";
    public Color RoleColor => Palette.ImpostorRed;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;

    public DoomableType DoomHintType => (DoomableType)ToExDoomHints.ToExTrickster;

    public bool NoSpawn => true;

    public string GetAdvancedDescription() => RoleLongDescription + MiscUtils.AppendOptionsText(GetType());

    public CustomRoleConfiguration Configuration => new(this)
    {
        IconTmp = MiraAPI.Utilities.Assets.TmpSpriteUtils.CreateSpriteAsset(TownOfExtraAssets.JeknHydIcon.LoadAsset(), "DivaniMod.Role.Impostor.Hyde", 1.45f),
        Icon = DivaniAssets.JeknHydIcon,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        DefaultRoleCount = 0,
        MaxRoleCount = 0,
        ShowInFreeplay = true,
    };

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        if (Player.AmOwner)
        {
            ButtonResetPatches.ResetCooldowns();
            Player.SetKillTimer(Player.GetKillCooldown());
        }
    }
    }
