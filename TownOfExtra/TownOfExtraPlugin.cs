using System.Reflection;
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
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public class TownOfExtraPlugin : BasePlugin, IMiraPlugin
{
    private Harmony Harmony { get; } = new(TownOfExtraPluginInfo.Id);

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
        
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToPoisoned", "Poisoned");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToCannibalised", "Cannibalised");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToShattered", "Shattered");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToTerminated", "Terminated");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToUnbound", "Unbound");
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToCrushed", "Crushed");

        TerminologyPatches.RegisterToExTerms();
        TerminologyIconRegistry.RegisterIcons();
    }
}