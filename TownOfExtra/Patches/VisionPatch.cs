using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options;
using TownOfExtra.Options.Roles;

namespace TownOfExtra.Patches;

using HarmonyLib;

[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
public static class VisionPatch
{
    [HarmonyPriority(Priority.Low)] 
    public static void Postfix(ShipStatus __instance, NetworkedPlayerInfo player, ref float __result)
    {
        if (player == null || player.IsDead) return;

        var p = player.Object;
        var result = __result;
        
        if (p.HasModifier<SlippedModifier>()) result *= OptionGroupSingleton<SquidRoleOptions>.Instance.VisionDebuffMultiplier;
        if (p.HasModifier<ShockwavedModifier>()) result *= OptionGroupSingleton<ImpostorModifierOptions>.Instance.ShockwaveVisionDebuffMultiplier.Value;
        if (p.HasModifier<ScaredDebuffsModifier>()) result *= OptionGroupSingleton<PoltergeistRoleOptions>.Instance.ScaredVisDebuffMulti;
        if (p.HasModifier<PossessedDebuffsModifier>()) result *= OptionGroupSingleton<PoltergeistRoleOptions>.Instance.PossessedVisDebuffMulti;

        __result = result;
    }
}