using System.Collections;
using System.Diagnostics.CodeAnalysis;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfExtra.Events.Custom;
using TownOfExtra.Modifiers.Excluded;
using TownOfExtra.Options.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfExtra.Networking;

[SuppressMessage("ReSharper", "LoopVariableIsNeverChangedInsideLoop")]
public class SquidRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.SquidSpillInk)]
    public static void RpcSpillInk(PlayerControl sender, float x, float y)
    {
        Coroutines.Start(SpillInk(sender, x, y));
    }

    private static IEnumerator SpillInk(PlayerControl sender, float x, float y)
    {
        var ink = new GameObject($"SquidInkPuddle_{sender.Data.PlayerName}");
        ink.transform.position = new Vector3(x, y, y / 1000f + 1f);

        var renderer = ink.AddComponent<SpriteRenderer>();
        if (PlayerControl.LocalPlayer != sender)
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        renderer.sprite = TownOfExtraAssets.SquidInkPuddle.LoadAsset();

        float duration = OptionGroupSingleton<SquidRoleOptions>.Instance.InkDuration;
        Object.Destroy(ink, duration);

        var p = PlayerControl.LocalPlayer;
        
        while (ink != null)
        {
            if (!p.Data.IsDead && !p.inVent)
            {
                if (Vector2.Distance(p.transform.position, ink.transform.position) <= 0.60f)
                {
                    if (p == sender && !OptionGroupSingleton<SquidRoleOptions>.Instance.InkAffectsSquid)
                        yield break;

                    p.RpcAddModifier<SlippedModifier>();
                    var toexAbilityEvent = new TownOfExtraAbilityEvent(AbilityType.SquidInkDestroyed, sender, p);
                    MiraEventManager.InvokeEvent(toexAbilityEvent);
                    RpcDestroyInk(sender);
                    yield break;
                }
            }

            yield return null;
        }

        var thatOtherToexAbilityEvent = new TownOfExtraAbilityEvent(AbilityType.SquidInkDestroyed, sender);
        MiraEventManager.InvokeEvent(thatOtherToexAbilityEvent);
    }
    
    [MethodRpc((uint)TownOfExtraRpcs.SquidDestroyInk)]
    public static void RpcDestroyInk(PlayerControl sender)
    {
        var ink = GameObject.Find($"SquidInkPuddle_{sender.Data.PlayerName}");
        if (ink != null) Object.Destroy(ink);
    }
}