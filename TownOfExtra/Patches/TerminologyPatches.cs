using System.Collections.Generic;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Roles.Crewmate.Power;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfExtra.Roles.Impostor.Power;
using TownOfExtra.Roles.Neutral.Evil;
using TownOfExtra.Roles.Neutral.Outlier;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Patches;

public interface ITerminologyIcon
{
    bool ShouldShow(PlayerControl local, PlayerControl row);
    string RichChunk { get; }
    Color? OverrideColor(PlayerControl local, PlayerControl row) => null;
}

internal static class TerminologyIconRegistry
{
    private static readonly List<ITerminologyIcon> Icons = [];

    public static void Register(ITerminologyIcon icon) => Icons.Add(icon);

    internal static void RegisterIcons()
    {
        Register(new ScaredIcon());
        Register(new PossessedIcon());
        Register(new ErasedIcon());
        Register(new PendingSwitchIcon());
        Register(new TaggedIcon());
        Register(new RecruitedIcon());
        Register(new InterviewingIcon());
    }

    internal static void AppendIcons(ref string result, PlayerControl row)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || row == null || local.Data == null) return;

        foreach (var icon in Icons)
        {
            if (!icon.ShouldShow(local, row)) continue;
            var chunk = $" {icon.RichChunk}";
            if (!string.IsNullOrEmpty(chunk) && !result.Contains(chunk)) result += chunk;
        }
    }

    internal static Color? ResolveColor(PlayerControl row)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || row == null || local.Data == null) return null;

        foreach (var icon in Icons)
        {
            if (!icon.ShouldShow(local, row)) continue;
            var c = icon.OverrideColor(local, row);
            if (c.HasValue) return c;
        }

        return null;
    }
}

internal sealed class ScaredIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}⌇</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        row.HasModifier<ScaredModifier>() &&
        (local.GetTownOfUsRole() is PoltergeistRole || local.Data.IsDead);
}

internal sealed class PossessedIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.PossessedColour.ToTextColor()}유</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        row.HasModifier<PossessedModifier>() &&
        (local.GetTownOfUsRole() is PoltergeistRole || local.Data.IsDead);
}

internal sealed class ErasedIcon : ITerminologyIcon
{
    public string RichChunk => $"{Palette.ImpostorRed.ToTextColor()}▧</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        row.HasModifier<ErasedModifier>() &&
        (local.GetTownOfUsRole() is EraserRole || local.Data.IsDead);
}

internal sealed class PendingSwitchIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.ShifterRoleColour.ToTextColor()}⇆</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        row.HasModifier<ShiftedModifier>() &&
        (local.GetTownOfUsRole() is ShifterRole || local.Data.IsDead);
}

internal sealed class TaggedIcon : ITerminologyIcon
{
    public string RichChunk => $"{Palette.ImpostorRed.ToTextColor()}▣</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        TaggerRole.MarkedPlayers.Contains(row) &&
        (local.GetTownOfUsRole() is TaggerRole || local.Data.IsDead);
}

internal sealed class RecruitedIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.ChiefRoleColour.ToTextColor()}❖</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        ChiefRole.Recruits.Contains(row) &&
        (local.GetTownOfUsRole() is ChiefRole || local.Data.IsDead);
}

internal sealed class InterviewingIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.JournalistRoleColour.ToTextColor()}ⓘ</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) =>
        row.HasModifier<InterviewModifier>() &&
        (local.GetTownOfUsRole() is JournalistRole || local.Data.IsDead);
}

[HarmonyPatch(typeof(PlayerRoleTextExtensions), nameof(PlayerRoleTextExtensions.UpdateTargetSymbols), new[] { typeof(string), typeof(PlayerControl), typeof(bool) })]
public static class TerminologySymbolPatch
{
    public static void Postfix(ref string __result, PlayerControl player)
    {
        TerminologyIconRegistry.AppendIcons(ref __result, player);
    }
}

[HarmonyPatch(typeof(PlayerRoleTextExtensions), nameof(PlayerRoleTextExtensions.UpdateTargetSymbols), new[] { typeof(string), typeof(PlayerControl), typeof(DataVisibility) })]
public static class TerminologySymbolDataVisibilityPatch
{
    public static void Postfix(ref string __result, PlayerControl player)
    {
        TerminologyIconRegistry.AppendIcons(ref __result, player);
    }
}

[HarmonyPatch(typeof(PlayerRoleTextExtensions), nameof(PlayerRoleTextExtensions.UpdateTargetColor), new[] { typeof(Color), typeof(PlayerControl), typeof(DataVisibility) })]
public static class TerminologyColorPatch
{
    public static void Postfix(ref Color __result, PlayerControl player)
    {
        var c = TerminologyIconRegistry.ResolveColor(player);
        if (c.HasValue) __result = c.Value;
    }
}

[HarmonyPatch(typeof(IngameWikiMinigame), nameof(IngameWikiMinigame.AddNewTerms))]
public static class WikiTermsPatch
{
    public static void Postfix(IngameWikiMinigame instance)
    {
        instance._activeTerms.Add(new TermWikiInfo(
            "ToExTermsTitle",
            "ToExTermsDesc",
            TownOfExtraAssets.TownOfExtraIcon
        ));
    }
}

public static class TerminologyPatches
{
    public static void RegisterToExTerms()
    {
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("ToExTermsTitle", "ToEx Symbols");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("ToExTermsDesc",
            "These symbols are the custom symbols from Town of Extra. " +
            $"• Scared players are marked with <b>{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}⌇</color></b>\n" +
            $"• Possessed players are marked with <b>{TownOfExtraColours.PossessedColour.ToTextColor()}유</color></b>\n" +
            $"• Erased players are marked with <b>{Palette.ImpostorRed.ToTextColor()}▧</color></b>\n" +
            $"• Pending switches are marked with <b>{TownOfExtraColours.ShifterRoleColour.ToTextColor()}⇆</color></b>\n" +
            $"• Tagged players are marked with <b>{Palette.ImpostorRed.ToTextColor()}▣</color></b>\n" +
            $"• Recruited players are marked with <b>{TownOfExtraColours.ChiefRoleColour.ToTextColor()}❖</color></b>\n" +
            $"• Players waiting for/in interviews are marked with <b>{TownOfExtraColours.JournalistRoleColour.ToTextColor()}</color>ⓘ</b>\n"
        );
    }
}