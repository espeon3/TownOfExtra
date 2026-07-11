using System.Reflection;
using MiraAPI.LocalSettings;
using TownOfUs;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra;

public class TownOfExtraColours
{
    public static Color GlobalModColour => new Color32(240, 70, 118, 255);
    
    public static bool UseBasicCrew { get; set; } = LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;
    
    public static Color ChiefRoleColour => UseBasicCrew ? Palette.CrewmateBlue : new Color32(0, 118, 1, 255);
    public static Color JournalistRoleColour => UseBasicCrew ? Palette.CrewmateBlue : new Color32(218, 213, 197, 255);
    public static Color CommanderRoleColour => UseBasicCrew ? Palette.CrewmateBlue : new Color32(240, 148, 117, 255);
    
    public static Color PoisonColour => new Color32(46, 82, 53, 255);
    public static Color FreezeColour => new Color32(0, 200, 255, 255);
    
    public static Color ShifterRoleColour => new Color32(153, 153, 153, 255);
    public static Color TricksterRoleColour => new Color32(128, 0, 155, 255);
    public static Color PoltergeistRoleColour => new Color32(122, 186, 168, 255);
    public static Color VultureRoleColour => new Color32(79, 24, 0, 255);
    public static Color CannibalRoleColour => new Color32(146, 43, 5, 255);
    public static Color SquidRoleColour => new Color32(146, 84, 197, 255);
    public static Color ShadowWalkerRoleColour => new Color32(1, 1, 35, 255);
    public static Color BarbarianRoleColour => new Color32(190, 189, 173, 255);
    
    public static Color PossessedColour => new Color32(255, 235, 171, 255);
    
    public static Color HeavyWorkloadModifierColour => new Color32(216, 108, 2, 255);
    public static Color BrittleModifierColour => new Color32(71, 102, 125, 255);
    public static Color ApoliticalModifierColour => new Color32(159, 184, 113, 255);
    public static Color ObservantModifierColour => new Color32(62, 222, 105, 255);
    public static Color ClumsyModifierColour => new Color32(111, 255, 211, 255);
    public static Color ShockwaveModifierColour => new Color32(213, 167, 136, 255);
    public static Color MuteModifierColour => new Color32(96, 47, 94, 255);
    public static Color PanicShieldModifierColour => new Color32(116, 206, 84, 255);
    public static Color EmbrittlementModifierColour => new Color32(169, 198, 205, 255);


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