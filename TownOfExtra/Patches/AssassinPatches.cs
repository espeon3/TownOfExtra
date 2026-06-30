using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfExtra.Modules;
using TownOfExtra.Options.Roles;
using TownOfExtra.Roles.Impostor.Killing;
using TownOfExtra.Roles.Impostor.Power;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Game.Impostor;

namespace TownOfExtra.Patches;

public class AssassinPatches
{
    [HarmonyPatch(typeof(ImpostorAssassinModifier), nameof(ImpostorAssassinModifier.IsModifierValidOn))]
    public static class ImpostorAssassinModifierValidRolesPatch
    {
        public static void Postfix(RoleBehaviour role, ref bool __result)
        {
            if (role is StrikerRole) __result = false;
            if (role is EraserRole && !OptionGroupSingleton<EraserRoleOptions>.Instance.CanBeAssassin) __result = false;
        }
    }
    
    [HarmonyPatch(typeof(AssassinModifier), "IsModifierValid")]
    public static class AssassinModifierGuessMenuPatch
    {
        public static void Postfix(BaseModifier modifier, ref bool __result)
        {
            if (modifier is IUnguessableModifier ium && !ium.IsGuessable)
                __result = false;
        }
    }
}