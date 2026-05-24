using System.Reflection;
using MiraAPI.LocalSettings;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra;

public class TownOfExtraColours
{
    public static Color CreditsColour => new Color32(240, 70, 118, 255);
    
    public static bool UseBasic { get; set; } =
        LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;
    
    public static Color ChiefRoleColour => UseBasic ? Palette.CrewmateBlue : new Color32(0, 118, 1, 255);
    public static Color PoltergeistRoleColour => new Color32(122, 186, 168, 255);
    
    public static Color PoisonColour => new Color32(46, 82, 53, 255);
    public static Color FreezeColour => new Color32(0, 200, 255, 255);
    public static Color CannibalColour => new Color32(180, 90, 50, 255);
    
    public static Color SwitcherRoleColour => new Color32(255, 239, 168, 255);
    public static Color TricksterRoleColour => new Color32(128, 0, 155, 255);
    public static Color VultureRoleColour => new Color32(79, 24, 0, 255);
    
    public static Color PossessedColour => new Color32(255, 235, 171, 255);
    
    public static Color HeavyWorkloadModifierColour => new Color32(216, 108, 2, 255);
    public static Color FragileModifierColour => new Color32(71, 102, 125, 255);


    public static Color GetRoleColour(string name)
    {
        var touColour = 
            name != null
                ? MiscUtils.GetRoleColour(name)
                : Color.white;
        var roleColour = 
            touColour == TownOfUsColors.Impostor 
                ? GetTownOfExtraRoleColour(name)
                : touColour;

        return roleColour;
    }
    
    
    public static Color GetTownOfExtraRoleColour(string name)
    {
        var pInfo = typeof(TownOfExtraColours).GetProperty($"{name}RoleColour", BindingFlags.Public | BindingFlags.Static);

        if (pInfo == null)
        {
            return TownOfUsColors.Impostor;
        }

        var colour = (Color)pInfo.GetValue(null)!;

        return colour;
    }
}