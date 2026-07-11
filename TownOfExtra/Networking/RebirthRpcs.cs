using Reactor.Networking.Attributes;
using TownOfExtra.Achievements;
using TownOfExtra.Modifiers.Game.Impostor.Passive;
using TownOfExtra.Modules;
using TownOfExtra.Networking.Global;
using TownOfUs.Modules;
using TownOfUs.Utilities;

namespace TownOfExtra.Networking;

public static class RebirthRpcs
{
    [MethodRpc((uint)TownOfExtraRpcs.RebirthSendPopup)]
    public static void RpcSendRebirthPopup(PlayerControl sendto, PlayerControl rolefrom)
    {
        if (PlayerControl.LocalPlayer != sendto) return;
        var role = rolefrom.GetRoleWhenAlive();
        
        var confirm = AmbassadorConfirmMinigame.Create();
        confirm.TitleString = "Rebirth";
        confirm.BodyText = 
            $"<b>Your teammate, {rolefrom.Data.PlayerName}, has died!</b>\n" + 
            "You now have the choice to take their role, or stick with yours.\n" + 
            $"This change {Palette.ImpostorRed.ToTextColor()}cannot be undone</color>.\n" + 
            $"Would you like to become their role of {TownOfExtraColours.GetRoleColour(rolefrom.Data.Role.name).ToTextColor()}{{role}}</color>?";
        
        confirm.Open(role, accepted => {
            if (accepted)
            {
                RebirthModifier.Used = true;
                sendto.RpcChangeRole((ushort)role.Role);
                sendto.RpcSendNotification(
                    $"You have {Palette.ImpostorRed.ToTextColor()}rebirthed</color> into {rolefrom.Data.PlayerName}'s role: {TownOfExtraColours.GetRoleColour(rolefrom.Data.Role.name).ToTextColor()}{role.NiceName}",
                    "RebirthModifierIcon",
                    "ImpModIcon"
                );

                PlayerControl.LocalPlayer.RpcAwardAchievement(AApi.GetInstance()?.RebirthUse);
            }
            else
            {
                sendto.RpcSendNotification(
                    $"You have {Palette.ImpostorRed.ToTextColor()}denied</color> the rebirth offer of {rolefrom.Data.PlayerName}'s role: {TownOfExtraColours.GetRoleColour(rolefrom.Data.Role.name).ToTextColor()}{role.NiceName}",
                    "RebirthModifierIcon",
                    "ImpModIcon"
                );
            }
            confirm.Close();
        });
    }
}