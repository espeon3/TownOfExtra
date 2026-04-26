using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Modules.Localization;

namespace TownOfExtra;

[BepInAutoPlugin("me.mehzxzz.townOfExtra", "Town Of Extra")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class TownOfExtraPlugin : BasePlugin, IMiraPlugin
{
    private Harmony Harmony { get; } = new(Id);

    public string OptionsTitleText => "Town Of Extra";

    public ConfigFile GetConfigFile() => Config;

    public override void Load()
    {
        ReactorCredits.Register("Town Of Extra", "1.0.0", false, ReactorCredits.AlwaysShow);
        MethodRpcAttribute.Register(Assembly.GetExecutingAssembly(), this);
        Harmony.PatchAll();
        
        TouLocale.TouLocalization.TryAdd(SupportedLangs.English, null);
        TouLocale.TouLocalization[SupportedLangs.English].TryAdd("DiedToPoisoned", "Poisoned");
    }
}