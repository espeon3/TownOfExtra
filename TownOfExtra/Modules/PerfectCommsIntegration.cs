using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using TownOfUs.Utilities;
using TownOfExtra.Roles.Neutral.Killing;
using System.Linq;

namespace TownOfExtra.Modules;

public static class PerfectCommsCannibalIntegration
{
    public const string PerfectCommsPluginId = "com.edgetel.perfectcomms";
    private const string ModId = TownOfExtraPluginInfo.Id;

    private const string CannibalBellyVoice = "CannibalBellyVoice";
    private static bool registered;
    private static PerfectCommsBridge? bridge;

    public static void Register()
    {
        if (registered)
        {
            return;
        }

        bool loaded = IL2CPPChainloader.Instance.Plugins.ContainsKey(PerfectCommsPluginId);
        if (!loaded)
        {
            return;
        }

        try
        {
            bridge = PerfectCommsBridge.Create();
            bridge.Unregister(ModId);
            bridge.RegisterModTab(ModId, "Town of Extra");
            RegisterOptions();
            bridge.RegisterVoiceRule(ModId, ResolveRuleObject);
            bridge.RegisterVoiceChannel(ModId, ResolveCannibalChannelObject);

            registered = true;
        }
        catch (Exception ex)
        {
            try
            {
                bridge?.Unregister(ModId);
            }
            catch
            {
                // Registration already failed; cleanup is best-effort.
            }

            bridge = null;
        }
    }

    private static void RegisterOptions()
    {
        if (bridge == null)
        {
            return;
        }

        bridge.RegisterHostOption(ModId, CannibalBellyVoice, Label(TownOfExtraColours.CannibalRoleColour, "Cannibal", "Can Chat With Lunch"), true);
    }

    private static string Label(Color color, string roleName, string suffix)
    {
        return $"{RoleName(color, roleName)}: {suffix}";
    }

    private static string RoleName(Color color, string roleName)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{roleName}</color>";
    }

    private static object ResolveRuleObject(object context)
    {
        var ctx = new VoiceContext(context);

        if (ctx.Player == null || ctx.IsDead)
        {
            return bridge!.PassResult;
        }

        if (!IsLiveVoicePhase(ctx))
        {
            return bridge!.PassResult;
        }

        var local = PlayerControl.LocalPlayer;
        if (local != null)
        {
            byte localId = local.PlayerId;
            byte speakerId = ctx.Player.PlayerId;

            bool localSwallowed = CannibalSystem.IsSwallowed(localId);
            bool speakerSwallowed = CannibalSystem.IsSwallowed(speakerId);

            if (localSwallowed || speakerSwallowed)
            {
                bool canHear = false;
                if (ctx.GetOption(CannibalBellyVoice))
                {
                    byte? localCannibal = CannibalSystem.GetCannibalOf(localId);
                    byte? speakerCannibal = CannibalSystem.GetCannibalOf(speakerId);

                    canHear = (localCannibal.HasValue && localCannibal.Value == speakerId) ||
                              (speakerCannibal.HasValue && speakerCannibal.Value == localId) ||
                              (localCannibal.HasValue && speakerCannibal.HasValue && localCannibal.Value == speakerCannibal.Value);
                }
                if (!canHear)
                {
                    return bridge!.Mute("Swallowed");
                }
            }
        }

        return bridge!.PassResult;
    }

    private static object? ResolveCannibalChannelObject(object context)
    {
        var ctx = new VoiceContext(context);
        if (!ctx.GetOption(CannibalBellyVoice) || !IsLiveVoicePhase(ctx) || ctx.IsDead)
        {
            return null;
        }

        byte playerId = ctx.Player!.PlayerId;
        byte? CannibalId = CannibalSystem.GetCannibalOf(playerId);
        if (CannibalId.HasValue)
        {
            return Radio($"Cannibal:{CannibalId.Value}");
        }

        if (ctx.Player.IsRole<CannibalRole>() && CannibalSystem.GetSwallowedByCannibal(playerId).Count > 0)
        {
            return Radio($"Cannibal:{playerId}");
        }

        return null;
    }

    private static object Radio(string key)
    {
        return bridge!.CreateChannelResult(key);
    }

    private static bool IsLiveVoicePhase(VoiceContext ctx)
    {
        return ctx.Phase is VoicePhase.Tasks or VoicePhase.Meeting;
    }

    private enum VoicePhase
    {
        Lobby,
        Tasks,
        Meeting,
        Exile,
    }

    private readonly struct VoiceContext
    {
        private readonly Func<string, bool>? getOption;
        private readonly Func<string, int>? getEnumOption;

        public VoiceContext(object context)
        {
            var values = bridge!.ReadContext(context);
            Player = values.Player;
            Phase = values.Phase;
            IsDead = values.IsDead;
            getOption = values.GetOption;
            getEnumOption = values.GetEnumOption;
        }

        public PlayerControl? Player { get; }

        public VoicePhase Phase { get; }

        public bool IsDead { get; }

        public bool GetOption(string key)
        {
            return getOption?.Invoke(key) ?? false;
        }

        public int GetEnumOption(string key)
        {
            return getEnumOption?.Invoke(key) ?? 0;
        }
    }

    private sealed class PerfectCommsBridge
    {
        private readonly Type apiType;
        private readonly Type ruleContextType;
        private readonly Type ruleResultType;
        private readonly Type phaseType;
        private readonly Type channelResultType;
        private readonly Type audioShapeType;
        private readonly Type hostOptionType;
        private readonly Type hostEnumOptionType;
        private readonly Func<object, PlayerControl?> readPlayer;
        private readonly Func<object, int> readPhase;
        private readonly Func<object, bool> readIsDead;
        private readonly Func<object, Func<string, bool>?> readGetOption;
        private readonly Func<object, Func<string, int>?> readGetEnumOption;

        private PerfectCommsBridge(Assembly assembly)
        {
            apiType = GetRequiredType(assembly, "PerfectComms.Api.PerfectCommsApi");
            ruleContextType = GetRequiredType(assembly, "PerfectComms.Api.VoiceRuleContext");
            ruleResultType = GetRequiredType(assembly, "PerfectComms.Api.VoiceRuleResult");
            phaseType = GetRequiredType(assembly, "PerfectComms.Api.VoicePhaseKind");
            channelResultType = GetRequiredType(assembly, "PerfectComms.Api.VoiceChannelResult");
            audioShapeType = GetRequiredType(assembly, "PerfectComms.Api.VoiceAudioShape");
            hostOptionType = GetRequiredType(assembly, "PerfectComms.Api.VoiceHostOption");
            hostEnumOptionType = GetRequiredType(assembly, "PerfectComms.Api.VoiceHostEnumOption");
            readPlayer = BuildPropertyReader<PlayerControl?>(ruleContextType, "Player");
            readPhase = BuildEnumPropertyReader(ruleContextType, "Phase");
            readIsDead = BuildPropertyReader<bool>(ruleContextType, "IsDead");
            readGetOption = BuildPropertyReader<Func<string, bool>?>(ruleContextType, "GetOption");
            readGetEnumOption = BuildPropertyReader<Func<string, int>?>(ruleContextType, "GetEnumOption");
            PassResult = ruleResultType.GetField("Pass", BindingFlags.Public | BindingFlags.Static)?.GetValue(null)
                         ?? throw new MissingMemberException(ruleResultType.FullName, "Pass");
        }

        public object PassResult { get; }

        public static PerfectCommsBridge Create()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(candidate => candidate.GetName().Name == "PerfectComms")
                ?? Assembly.Load("PerfectComms");

            return new PerfectCommsBridge(assembly);
        }

        public void RegisterModTab(string modId, string tabLabel)
        {
            InvokeApi(nameof(RegisterModTab), modId, tabLabel);
        }

        public void RegisterHostOption(string modId, string key, string label, bool defaultValue)
        {
            var option = Activator.CreateInstance(hostOptionType, key, label, defaultValue);
            InvokeApi(nameof(RegisterHostOption), modId, option!);
        }

        public void RegisterHostEnumOption(string modId, string key, string label, int defaultValue, string[] choices)
        {
            var option = Activator.CreateInstance(hostEnumOptionType, key, label, defaultValue, choices);
            InvokeApi(nameof(RegisterHostEnumOption), modId, option!);
        }

        public void RegisterVoiceRule(string modId, Func<object, object> rule)
        {
            var delegateType = typeof(Func<,>).MakeGenericType(ruleContextType, ruleResultType);
            var callback = BuildObjectCallback(delegateType, ruleContextType, ruleResultType, rule);
            InvokeApi(nameof(RegisterVoiceRule), modId, callback);
        }

        public void RegisterVoiceChannel(string modId, Func<object, object?> channel)
        {
            var delegateType = typeof(Func<,>).MakeGenericType(ruleContextType, channelResultType);
            var callback = BuildObjectCallback(delegateType, ruleContextType, channelResultType, channel);
            InvokeApi(nameof(RegisterVoiceChannel), modId, callback);
        }

        public object Mute(string reason)
        {
            return ruleResultType.GetMethod("Mute", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, [reason])
                   ?? PassResult;
        }

        public object CreateChannelResult(string key)
        {
            object radioShape = Enum.Parse(audioShapeType, "Radio");
            return Activator.CreateInstance(channelResultType, key, true, radioShape, 1f, null)
                   ?? throw new InvalidOperationException("Could not create Perfect Comms channel result.");
        }

        public void Unregister(string modId)
        {
            InvokeApi(nameof(Unregister), modId);
        }

        public ContextValues ReadContext(object context)
        {
            var phase = (VoicePhase)readPhase(context);
            return new ContextValues(
                readPlayer(context),
                phase,
                readIsDead(context),
                readGetOption(context),
                readGetEnumOption(context));
        }

        private void InvokeApi(string methodName, params object[] args)
        {
            var method = apiType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
                         ?? throw new MissingMethodException(apiType.FullName, methodName);
            method.Invoke(null, args);
        }

        private static Delegate BuildObjectCallback(
            Type delegateType,
            Type parameterType,
            Type returnType,
            Delegate callback)
        {
            var parameter = Expression.Parameter(parameterType, "context");
            var invoke = Expression.Invoke(Expression.Constant(callback), Expression.Convert(parameter, typeof(object)));
            var body = Expression.Convert(invoke, returnType);
            return Expression.Lambda(delegateType, body, parameter).Compile();
        }

        private static Func<object, T> BuildPropertyReader<T>(Type declaringType, string propertyName)
        {
            var property = declaringType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                           ?? throw new MissingMemberException(declaringType.FullName, propertyName);
            var value = Expression.Parameter(typeof(object), "value");
            var typedValue = Expression.Convert(value, declaringType);
            var propertyValue = Expression.Property(typedValue, property);
            var converted = Expression.Convert(propertyValue, typeof(T));
            return Expression.Lambda<Func<object, T>>(converted, value).Compile();
        }

        private static Func<object, int> BuildEnumPropertyReader(Type declaringType, string propertyName)
        {
            var property = declaringType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                           ?? throw new MissingMemberException(declaringType.FullName, propertyName);
            var value = Expression.Parameter(typeof(object), "value");
            var typedValue = Expression.Convert(value, declaringType);
            var propertyValue = Expression.Property(typedValue, property);
            var converted = Expression.Convert(propertyValue, typeof(int));
            return Expression.Lambda<Func<object, int>>(converted, value).Compile();
        }

        private static Type GetRequiredType(Assembly assembly, string typeName)
        {
            return assembly.GetType(typeName)
                   ?? throw new TypeLoadException($"Perfect Comms API type not found: {typeName}");
        }

        public readonly record struct ContextValues(
            PlayerControl? Player,
            VoicePhase Phase,
            bool IsDead,
            Func<string, bool>? GetOption,
            Func<string, int>? GetEnumOption);
    }
}