using MiraAPI.Modifiers;
using Reactor.Networking.Attributes;
using TownOfExtra.Modifiers.Excluded;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public class ObstructorRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.ObstructorTriggerObstruct)]
    public static void RpcTriggerObstruct(PlayerControl victim, bool fullRemoval)
    {
        if (LobbyBehaviour.Instance)
        {
            MiscUtils.RunAnticheatWarning(victim);
            return;
        }
        if (victim.TryGetModifier<ObstructorObstructedModifier>(out var obstructMod))
        {
            if (fullRemoval)
            {
                victim.RemoveModifier(obstructMod);
            }
            else
            {
                obstructMod.ShowObstructed();
            }
        }
    }
}