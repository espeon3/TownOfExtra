using System.Reflection;
using TownOfUs;
using UnityEngine;

namespace TownOfExtra;

public class TownOfExtraColours
{
    public static Color PoisonColour => new Color(46/255f, 82/255f, 53/255f);
    public static Color FreezeColour => new Color(0/255f, 200/255f, 255/255f);
    public static Color CannibalColour => new Color(180/255f, 90/255f, 50/255f);
    public static Color SwitcherRoleColour => new Color(255/255f, 239/255f, 168/255f);
    public static Color TricksterRoleColour => new Color(128 / 255f, 0 / 255f, 155 / 255f);
    public static Color ChiefRoleColour => new Color(0 / 255f, 118 / 255f, 1 / 255f);
    //public static Color RoleColour => new Color( / 255f, / 255f, / 255f);
    
    public static Color HeavyWorkloadModifierColour => new Color(216 / 255f, 108 / 255f, 2 / 255f);
    public static Color FragileModifierColour => new Color(71 / 255f, 102 / 255f, 125 / 255f);
    //public static Color ModifierColour => new Color( / 255f, / 255f, / 255f);
    
    public static Color GetTownOfExtraRoleColour(string name)
    {
        var pInfo = typeof(TownOfExtraColours).GetProperty(name, BindingFlags.Public | BindingFlags.Static);

        if (pInfo == null)
        {
            return TownOfUsColors.Impostor;
        }

        var colour = (Color)pInfo.GetValue(null)!;

        return colour;
    }
}