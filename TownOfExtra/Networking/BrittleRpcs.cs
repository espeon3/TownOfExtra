using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using TownOfExtra.Modifiers.Game.Crewmate.Passive;
using TownOfExtra.Options;
using TownOfUs.Networking;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public class BrittleRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.BrittleTriggerModifier, LocalHandling = RpcLocalHandling.None)]
    public static void RpcTriggerBrittleModifier(PlayerControl target)
    {
        if (target.AmOwner && target.HasModifier<BrittleModifier>() && !target.Data.IsDead)
        {
            Coroutines.Start(MiscUtils.CoFlash(TownOfExtraColours.BrittleModifierColour));
            
            if (!BrittleModifier.Interactions.ContainsKey(target))
            {
                BrittleModifier.Interactions.Add(target, 0);
            }
            BrittleModifier.Interactions[target] += 1;

            if (BrittleModifier.Interactions[target] >=
                OptionGroupSingleton<CrewmateModifierOptions>.Instance.BrittleMaxInteractions.Value)
            {
                target.RpcSpecialMurder(target, MeetingCheck.OutsideMeeting, true, true, causeOfDeath: "Shattered");
                BrittleModifier.Interactions.Remove(target);
            }
        }
    }
}