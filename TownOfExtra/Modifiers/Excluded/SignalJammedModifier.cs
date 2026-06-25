using System.IO;
using System.Linq;
using Il2CppInterop.Runtime;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Networking;
using TownOfExtra.Options.Roles;
using UnityEngine;

namespace TownOfExtra.Modifiers.Excluded;

public class SignalJammedModifier : TimedModifier
{
    public override string ModifierName => "Jammed";
    public override bool HideOnUi => false;
    public override float Duration => OptionGroupSingleton<SignalJammerRoleOptions>.Instance.JamDuration;
    public override bool AutoStart => true;
    public override bool RemoveOnComplete => true;
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.SignalJammerRoleIcon;

    public override string GetDescription()
    {
        return $"Your signals are jammed for {TimeRemaining:F1}s!";
    }
    
    //private Sprite _normSprite;

    public override void OnActivate()
    {
        if (!Player.AmOwner) return;

        if (Player.Data.IsDead)
        {
            Player.RpcSendNotification(
                $"Player's meeting signals have been {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
                "SignalJammerRoleIcon",
                "ImpRoleIcon",
                200
            );
        }
        else
        {
            Player.RpcSendNotification(
                $"Your meeting signals have been {Palette.ImpostorRed.ToTextColor()}jammed</color>!",
                "SignalJammerRoleIcon",
                "ImpRoleIcon",
                200,
                flashColour: Palette.ImpostorRed
            );
        }
    }

    /*var emergencyBtn = GameObject.Find("EmergencyConsole");
    if (emergencyBtn == null) return;

    _normSprite = emergencyBtn.GetComponent<SpriteRenderer>().sprite;
    emergencyBtn.GetComponent<SpriteRenderer>().sprite = TownOfExtraAssets.EmergencyConsoleBroken.LoadAsset();
}

public override void OnDeactivate()
{
    if (!Player.AmOwner) return;

    var emergencyBtn = GameObject.Find("EmergencyConsole");
    if (emergencyBtn == null) return;

    emergencyBtn.GetComponent<SpriteRenderer>().sprite = _normSprite;
}*/
}