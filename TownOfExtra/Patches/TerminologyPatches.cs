using System.Collections.Generic;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers;
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
    }

    internal static void AppendIcons(ref string result, PlayerControl row)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || row == null || local.Data == null) return;

        foreach (var icon in Icons)
        {
            if (!icon.ShouldShow(local, row)) continue;
            var chunk = icon.RichChunk;
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
    public bool ShouldShow(PlayerControl local, PlayerControl row) => row.HasModifier<ScaredModifier>();
}

internal sealed class PossessedIcon : ITerminologyIcon
{
    public string RichChunk => $"{TownOfExtraColours.PossessedColour.ToTextColor()}유</color>";
    public bool ShouldShow(PlayerControl local, PlayerControl row) => row.HasModifier<PossessedModifier>();
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
            $"• Scared players are marked with <b>{TownOfExtraColours.PoltergeistRoleColour.ToTextColor()}⌇</color></b> " +
            $"• Possessed players are marked with <b>{TownOfExtraColours.PossessedColour.ToTextColor()}유</color></b>"
        );
    }
}