using System;
using System.Collections;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
//todo: using TownOfExtra.Achievements;
using TownOfExtra.Options.Roles;
using TownOfUs.Networking;
using TownOfUs.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TownOfExtra.Networking;

public static class ConjurerRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.ConjurerPlaceRock)]
    public static void RpcPlaceRock(PlayerControl sender, float x, float y, bool fallen)
    {
        Coroutines.Start(SpawnRock(sender, x, y, fallen));
    }

    private static IEnumerator SpawnRock(PlayerControl sender, float x, float y, bool fallen)
    {
        var rock = new GameObject();
        rock.transform.position = new Vector3(x, y + 2.5f, y / 1000f + 1f);

        var renderer = rock.AddComponent<SpriteRenderer>();
        renderer.sprite = fallen ? TownOfExtraAssets.ConjurerRockSpriteFallen.LoadAsset() : TownOfExtraAssets.ConjurerRockSprite.LoadAsset();
        
        float velocity = 0f;
        var pos = rock.transform.position;

        while (pos.y > y)
        {
            velocity += 10f * Time.deltaTime;
            pos.y -= velocity * Time.deltaTime;
            pos.y = Mathf.Max(pos.y, y);
            rock.transform.position = pos;

            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.Data.IsDead) continue;
                
                var cantCrush = OptionGroupSingleton<ConjurerRoleOptions>.Instance.ConjurerCantCrush;
                
                if (cantCrush == ConjurerCantCrushOptions.Everyone) continue;
                if (cantCrush == ConjurerCantCrushOptions.Team && p.IsImpostor() && p != sender) continue;
                if (cantCrush == ConjurerCantCrushOptions.SelfAndTeam && p.IsImpostor()) continue;
                
                if (Vector2.Distance(p.transform.position, pos) < 0.5f)
                { 
                    sender.RpcSpecialMurder(p, true, true, teleportMurderer: false, showKillAnim: false, createDeadBody: true, causeOfDeath: "Crushed");
                    //todo: sender.RpcAwardAchievement(AApi.GetInstance()?.ConjurerDropRockOnPlayer);
                    
                    /*var body = new GameObject();
                    body.AddComponent<SquashedBody>();
                    var sRenderer = body.AddComponent<SpriteRenderer>();
                    sRenderer.sprite = TownOfExtraAssets.SquashedDeadBodySprite.LoadAsset();
                    sRenderer.color = Palette.PlayerColors[p.cosmetics.ColorId];

                    var loc = p.transform.position;
                    loc.z = loc.y / 1000f + 1f;
                    body.transform.position = loc;

                    var visor = new GameObject();
                    visor.transform.SetParent(body.transform);
                    visor.transform.localPosition = Vector3.zero;
                    var osRenderer = visor.AddComponent<SpriteRenderer>();
                    osRenderer.sprite = TownOfExtraAssets.SquashedDeadBodySpriteVisor.LoadAsset();*/
                    
                    break;
                }
            }
            
            yield return null;
        }

        try
        {
            var collider = rock.AddComponent<PolygonCollider2D>();
            var points = new Il2CppSystem.Collections.Generic.List<Vector2>();
            renderer.sprite.GetPhysicsShape(0, points);
            collider.SetPath(0, points);
        }
        catch {}

        float duration = OptionGroupSingleton<ConjurerRoleOptions>.Instance.ConjureDuration;
        yield return new WaitForSeconds(duration);

        if (rock != null) Object.Destroy(rock);
    }
}

[RegisterInIl2Cpp]
public class SquashedBody : MonoBehaviour
{
    public SquashedBody(IntPtr ptr) : base(ptr) { }
}