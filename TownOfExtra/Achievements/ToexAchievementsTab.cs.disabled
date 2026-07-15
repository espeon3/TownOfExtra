using AchievementsAPI.API;
using UnityEngine;

namespace TownOfExtra.Achievements;

public class ToexAchievementsTab : AchievementsTab
{
    public override string Name => "Town of Extra";
    public override bool IsSelectable => true;

    public override Color GetTabColor() => TownOfExtraColours.GlobalModColour;
    public override Sprite GetIcon() => TownOfExtraAssets.TownOfExtraIcon.LoadAsset();
    
    // ------------------------------
    //         achievements
    // ------------------------------
    
    // global
    public BaseAchievement LaunchGame { get; set; } = new BaseAchievement(
        "Welcome to Town of Extra!", "Launch the game with Town of Extra & Achievements API installed", "TownOfExtra.Resources.Misc.TownOfExtraIcon.png"
    );
    
    // conjurer
    public BaseAchievement ConjurerDropRockOnPlayer { get; set; } = new BaseAchievement(
        "Splat", "Conjure up a rock and drop it on someone", "TownOfExtra.Resources.Imp.Misc.ConjurerRockSprite.png"
    );
    
    // rebirth
    public BaseAchievement RebirthUse { get; set; } = new BaseAchievement(
        "Reborn", "Rebirth into a dead teammate's role", "TownOfExtra.Resources.Modifiers.Imp.ModifierIcons.RebirthModifierIcon.png"
    );
    
    // chief
    public BaseAchievement ChiefUseRecruit { get; set; } = new BaseAchievement(
        "Recruiter", "Recruit someone as the chief", "TownOfExtra.Resources.Crew.Buttons.ChiefRecruitButton.png"
    );
    
    // freezer
    public BaseAchievement FreezerUseAbility { get; set; } = new BaseAchievement(
        "Chilly", "Use the freezer's ability to stop everyone in their tracks", "TownOfExtra.Resources.Imp.Buttons.FreezerFreezeButton.png"
    );
    
    // trickster
    public BaseAchievement TricksterBodyReported { get; set; } = new BaseAchievement(
        "Tricked", "Report a trickster's fake body", "TownOfExtra.Resources.Neut.Buttons.TricksterPlaceButton.png"
    );
    
    // signal jammer
    public BaseAchievement SignalJammerMeetingWhileJammed { get; set; } = new BaseAchievement(
        "No Wifi", "Try and report a body/call a meeting while signals are jammed", "TownOfExtra.Resources.Imp.RoleIcons.SignalJammerRoleIcon.png"
    );
    
    // vulture
    public BaseAchievement VultureEatBody { get; set; } = new BaseAchievement(
        "Crunchy", "Eat a body as the vulture", "TownOfExtra.Resources.Neut.RoleIcons.VultureRoleIcon.png"
    );
    
    // vinculator
    public BaseAchievement VinculatorLinkedVotedOut { get; set; } = new BaseAchievement(
        "Broken Link", "Eliminate 2 players at once by having one of your linked players voted out", "TownOfExtra.Resources.Imp.Buttons.VinculatorChainButton.png"
    );
    
    // shifter
    public BaseAchievement ShifterShiftWithSomeone { get; set; } = new BaseAchievement(
        "I'll be taking that", "Take a player's role as the shifter", "TownOfExtra.Resources.Neut.Buttons.ShifterShiftButton.png"
    );
    public BaseAchievement ShifterBeShiftedWith { get; set; } = new BaseAchievement(
        "Hey thats mine!", "Get your role stolen by the shifter", "TownOfExtra.Resources.Neut.Buttons.ShifterShiftButton.png"
    );
}