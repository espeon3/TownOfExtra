using MiraAPI.Hud;
using Reactor.Networking.Attributes;
using TownOfExtra.Buttons;

namespace TownOfExtra.Networking;

public static class CommanderRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.CommanderIncreaseAvengeUses)]
    public static void RpcIncreaseAvengeUses(this PlayerControl sendto)
    {
        if (PlayerControl.LocalPlayer != sendto) return;

        var btn = CustomButtonSingleton<CommanderAvengeButton>.Instance;
        btn.IncreaseUses();
    }
}