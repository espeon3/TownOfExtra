using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers.Types;
using MiraAPI.Modifiers;
using TownOfUs.Interfaces;

namespace TownOfExtra.Modifiers.Excluded;

public sealed class CannibalSwallowedModifier : BaseModifier, IUntransportable
{
    public override string ModifierName => "Swallowed";
    public override bool HideOnUi => true;

    [HideFromIl2Cpp]
    public byte CannibalId { get; set; }

    public override void OnActivate()
    {
    }

    public override void OnDeactivate()
    {
        if (Player != null)
        {
            Player.Visible = true;
            Player.moveable = true;
        }
    }
}











