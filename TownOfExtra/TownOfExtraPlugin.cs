using System.Reflection;
//todo: using AchievementsAPI;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Patches;
using TownOfUs.Modules.Localization;

namespace TownOfExtra;

[BepInPlugin(TownOfExtraPluginInfo.Id, TownOfExtraPluginInfo.Name, TownOfExtraPluginInfo.Version)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[BepInDependency("com.edgetel.perfectcomms", BepInDependency.DependencyFlags.SoftDependency)]
//[BepInDependency(AchievementsAPIPlugin.Id, BepInDependency.DependencyFlags.SoftDependency)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public class TownOfExtraPlugin : BasePlugin, IMiraPlugin
{
    public static Harmony Harmony { get; } = new(TownOfExtraPluginInfo.Id);

    public string OptionsTitleText => "Town Of Extra";
    public ConfigFile GetConfigFile() => Config;

    public static ManualLogSource Logger;
    
    public override void Load()
    {
        ReactorCredits.Register(TownOfExtraPluginInfo.Name, TownOfExtraPluginInfo.Version, TownOfExtraPluginInfo.IsPreRelease, ReactorCredits.AlwaysShow);
        MethodRpcAttribute.Register(Assembly.GetExecutingAssembly(), this);
        Harmony.PatchAll();
        ModNewsFetcher.CheckForNews();
        
        Logger = Log;

        ApplyTouLocaleEdits();
        TerminologyPatches.RegisterToExTerms();
        TerminologyIconRegistry.RegisterIcons();
        
        //todo: this
        /*if (ModCompat.IsLoaded(ModCompat.AApiId, out _))
        {
            Logger.LogInfo("AchievementsAPI found, achievements will be available!");
        }
        else
        {
            Logger.LogWarning("Failed to find AchievementsAPI, achievements will not be available.");
        }*/
    }

    public static void ApplyTouLocaleEdits()
    {
        // ------------------------
        // Death Messages
        // ------------------------
        
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToPoisoned", "Poisoned");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToCannibalised", "Cannibalised");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToShattered", "Shattered");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToTerminated", "Terminated");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToUnbound", "Unbound");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToCrushed", "Crushed");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToStruck", "Struck");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToMiscalculated", "Miscalculated");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToSlain", "Slain");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToPunished", "Punished");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToMassacred", "Massacred");

        // ------------------------
        // Wiki Edits
        // ------------------------
        
        TouLocale.TouLocalization[SupportedLangs.English]
                ["TouRoleClericCleanseWikiDescription"] =
            "Remove all negative effects on a player. (Douse, Hack, Infect, Blackmail, Blind, Flash, Hypnosis, Poisoned, Pending Shift, Doom, Lucid Dreaming, Pending Lucid Dream, Scared, Possessed, Pending Erase, Pending Embrittlement, Slipped, Shockwaved)";
        
        // ------------------------
        // Doomsayer Hints
        // ------------------------
        
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint101", "You observe that %player% is not from this town");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint102", "You observe that %player% has an altered perception of reality");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint103", "You observe that %player% has an insight for private information");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint104", "You observe that %player% has an unusual obsession with dead bodies");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint105", "You observe that %player% is well-trained in hunting down prey");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint106", "You observe that %player% spreads fear amongst the group");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint107", "You observe that %player% hides to guard themselves or others");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint108", "You observe that %player% has a trick up their sleeve");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("TouRoleDoomsayerRoleHint109", "You observe that %player% is capable of performing relentless attacks");
    }
}