using MiraAPI.LocalSettings;
using MiraAPI.Utilities.Assets;
using TownOfUs;
using UnityEngine;

namespace TownOfExtra;

public static class TownOfExtraAssets
{
    public static bool UseBasicCrew { get; set; } = LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance.UseCrewmateTeamColorToggle.Value;

    // ---- Crewmate Paths ----
    public static string CrewRoleIconPath => UseBasicCrew ? "TownOfExtra.Resources.BasicCrew.RoleIcons" : "TownOfExtra.Resources.Crew.RoleIcons";
    public static string CrewButtonPath => "TownOfExtra.Resources.Crew.Buttons";
    public static string CrewMiscPath => UseBasicCrew ? "TownOfExtra.Resources.BasicCrew.Misc" : "TownOfExtra.Resources.Crew.Misc";

    // ---- Impostor Paths ----
    public static string ImpRoleIconPath => "TownOfExtra.Resources.Imp.RoleIcons";
    public static string ImpButtonPath => "TownOfExtra.Resources.Imp.Buttons";
    public static string ImpMiscPath => "TownOfExtra.Resources.Imp.Misc";

    // ---- Neutral Paths ----
    public static string NeutRoleIconPath => "TownOfExtra.Resources.Neut.RoleIcons";
    public static string NeutButtonPath => "TownOfExtra.Resources.Neut.Buttons";
    public static string NeutMiscPath => "TownOfExtra.Resources.Neut.Misc";

    // ---- Modifier Paths ----
    public static string MiscModModIconPath => "TownOfExtra.Resources.Modifiers.Misc.ModifierIcons";
    
    public static string CrewModModIconPath => "TownOfExtra.Resources.Modifiers.Crew.ModifierIcons";
    public static string CrewModButtonPath => "TownOfExtra.Resources.Modifiers.Crew.Buttons";
    public static string CrewModMiscPath => "TownOfExtra.Resources.Modifiers.Crew.Misc";
    
    public static string ImpModModIconPath => "TownOfExtra.Resources.Modifiers.Imp.ModifierIcons";
    public static string ImpModButtonPath => "TownOfExtra.Resources.Modifiers.Imp.Buttons";
    public static string ImpModMiscPath => "TownOfExtra.Resources.Modifiers.Imp.Misc";
    
    public static string NeutModModIconPath => "TownOfExtra.Resources.Modifiers.Neut.ModifierIcons";
    public static string NeutModButtonPath => "TownOfExtra.Resources.Modifiers.Neut.Buttons";
    public static string NeutModMiscPath => "TownOfExtra.Resources.Modifiers.Neut.Misc";
    
    public static string UniModModIconPath => "TownOfExtra.Resources.Modifiers.Uni.ModifierIcons";
    public static string UniModButtonPath => "TownOfExtra.Resources.Modifiers.Uni.Buttons";
    public static string UniModMiscPath => "TownOfExtra.Resources.Modifiers.Uni.Misc";
    
    public static string NonCrewModModIconPath => "TownOfExtra.Resources.Modifiers.NonCrew.ModifierIcons";
    public static string NonCrewModButtonPath => "TownOfExtra.Resources.Modifiers.NonCrew.Buttons";
    public static string NonCrewModMiscPath => "TownOfExtra.Resources.Modifiers.NonCrew.Misc";

    // ---- General Misc Path ----
    public static string MiscPath => "TownOfExtra.Resources.Misc";



    // ===============================================================
    //                        PLACEHOLDERS
    // ===============================================================

    public static LoadableAsset<Sprite> Placeholder =>
        new LoadableResourceAsset($"{MiscPath}.Ph.png");
    public static LoadableAsset<Sprite> ProtectPh =>
        new LoadableResourceAsset($"{MiscPath}.PhProtect.png");
    public static LoadableAsset<Sprite> InfoPh =>
        new LoadableResourceAsset($"{MiscPath}.PhInfo.png");
    public static LoadableAsset<Sprite> AttackPh =>
        new LoadableResourceAsset($"{MiscPath}.PhAttack.png");
    public static LoadableAsset<Sprite> MiscPh =>
        new LoadableResourceAsset($"{MiscPath}.PhMisc.png");



    // ===============================================================
    //                         CREWMATE
    // ===============================================================

    // --- Role Icons ---

    // Power
    public static LoadableAsset<Sprite> ChiefRoleIcon =>
        new LoadableResourceAsset($"{CrewRoleIconPath}.ChiefRoleIcon.png", 200);
    public static LoadableAsset<Sprite> JournalistRoleIcon =>
        new LoadableResourceAsset($"{CrewRoleIconPath}.JournalistRoleIcon.png", 200);
    
    // Killing
    public static LoadableAsset<Sprite> CommanderRoleIcon =>
        new LoadableResourceAsset($"{CrewRoleIconPath}.CommanderRoleIcon.png", 200);

    // --- Modifiers ---

    // Passive
    public static LoadableAsset<Sprite> BrittleModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.BrittleModifierIcon.png");
    public static LoadableAsset<Sprite> HeavyWorkloadModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.HeavyWorkloadModifierIcon.png", 200);
    public static LoadableAsset<Sprite> RoutineModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.RoutineModifierIcon.png");
    public static LoadableAsset<Sprite> ObservantModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.ObservantModifierIcon.png", 200);
    public static LoadableAsset<Sprite> ClumsyModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.ClumsyModifierIcon.png", 200);
    public static LoadableAsset<Sprite> RelentlessModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.RelentlessModifierIcon.png", 200);
    
    // Passive
    public static LoadableAsset<Sprite> PanicShieldModifierIcon =>
        new LoadableResourceAsset($"{CrewModModIconPath}.PanicShieldModifierIcon.png", 200);

    // --- Buttons ---
    
    // Roles

    public static LoadableAsset<Sprite> ChiefRecruitButton =>
        new LoadableResourceAsset($"{CrewButtonPath}.ChiefRecruitButton.png");
    public static LoadableAsset<Sprite> ChiefShootButton =>
        new LoadableResourceAsset($"{CrewButtonPath}.ChiefShootButton.png");
    public static LoadableAsset<Sprite> JournalistInterviewButton =>
        new LoadableResourceAsset($"{CrewButtonPath}.JournalistInterviewButton.png");
    public static LoadableAsset<Sprite> CommanderCommandButton =>
        new LoadableResourceAsset($"{CrewButtonPath}.CommanderCommandButton.png", 200);
    public static LoadableAsset<Sprite> CommanderAvengeButton =>
        new LoadableResourceAsset($"{CrewButtonPath}.CommanderAvengeButton.png");
    
    // Modifiers
    
    public static LoadableAsset<Sprite> PanicShieldPanicShieldButton =>
        new LoadableResourceAsset($"{CrewModModIconPath}.PanicShieldModifierIcon.png", 225);
    
    // --- Misc ---
    
    public static LoadableAsset<Sprite> SpeedBoostModifierIcon =>
        new LoadableResourceAsset($"{CrewMiscPath}.SpeedBoostModifierIcon.png");
    
    // --- Chat ---
    
    
    
    // ===============================================================
    //                         IMPOSTOR
    // ===============================================================

    // --- Role Icons ---

    // Concealing
    public static LoadableAsset<Sprite> HolographerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.HolographerRoleIcon.png", 200);
    public static LoadableAsset<Sprite> SignalJammerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.SignalJammerRoleIcon.png", 200);

    // Killing
    public static LoadableAsset<Sprite> PoisonerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.PoisonerRoleIcon.png");
    public static LoadableAsset<Sprite> KnifeThrowerRoleIcon =
        new LoadableResourceAsset($"{ImpRoleIconPath}.KnifeThrowerRoleIcon.png");
    public static LoadableAsset<Sprite> StrikerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.StrikerRoleIcon.png");
    public static LoadableAsset<Sprite> TaggerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.TaggerRoleIcon.png");
    public static LoadableAsset<Sprite> BloodlustRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.BloodlustRoleIcon.png");

    // Power
    public static LoadableAsset<Sprite> ConjurerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.ConjurerRoleIcon.png", 200);
    public static LoadableAsset<Sprite> DreamCasterRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.DreamCasterRoleIcon.png", 200);
    public static LoadableAsset<Sprite> EraserRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.EraserRoleIcon.png", 200);
    public static LoadableAsset<Sprite> VinculatorRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.VinculatorRoleIcon.png", 356);

    // Support
    public static LoadableAsset<Sprite> FreezerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.FreezerRoleIcon.png", 356);
    public static LoadableAsset<Sprite> GamblerRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.GamblerRoleIcon.png", 200);
    public static LoadableAsset<Sprite> ObstructorRoleIcon =>
        new LoadableResourceAsset($"{ImpRoleIconPath}.ObstructorRoleIcon.png");

    // --- Modifiers ---
    
    // Utility
    public static LoadableAsset<Sprite> ShockwaveModifierIcon =>
        new LoadableResourceAsset($"{ImpModModIconPath}.ShockwaveModifierIcon.png");
    public static LoadableAsset<Sprite> EmbrittlementModifierIcon =>
        new LoadableResourceAsset($"{ImpModModIconPath}.EmbrittlementModifierIcon.png", 200);
    
    // Passive
    public static LoadableAsset<Sprite> RebirthModifierIcon =>
        new LoadableResourceAsset($"{ImpModModIconPath}.RebirthModifierIcon.png");
    
    // Buttons
    public static LoadableAsset<Sprite> ShockwaveShockwaveButton =>
        new LoadableResourceAsset($"{ImpModButtonPath}.ShockwaveShockwaveButton.png");

    // --- Buttons ---

    public static LoadableAsset<Sprite> TaggerMarkButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.TaggerMarkButton.png", 200);
    public static LoadableAsset<Sprite> HolographerHolographButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.HolographerHolographButton.png");
    public static LoadableAsset<Sprite> SignalJammerJamButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.SignalJammerJamButton.png");
    public static LoadableAsset<Sprite> PoisonerPoisonButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.PoisonerPoisonButton.png");
    public static LoadableAsset<Sprite> StrikerLocateButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.StrikerLocateButton.png", 683);
    public static LoadableAsset<Sprite> StrikerStrikeButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.StrikerStrikeButton.png");
    public static LoadableAsset<Sprite> ConjurerConjureButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.ConjurerConjureButton.png");
    public static LoadableAsset<Sprite> DreamCasterCastButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.DreamCasterCastButton.png");
    public static LoadableAsset<Sprite> EraserEraseButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.EraserEraseButton.png");
    public static LoadableAsset<Sprite> VinculatorChainButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.VinculatorChainButton.png");
    public static LoadableAsset<Sprite> VinculatorEmpowerButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.VinculatorEmpowerButton.png");
    public static LoadableAsset<Sprite> FreezerFreezeButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.FreezerFreezeButton.png");
    public static LoadableAsset<Sprite> ObstructorObstructButton =>
        new LoadableResourceAsset($"{ImpButtonPath}.ObstructorObstructButton.png");

    // --- Misc ---
    
    public static LoadableAsset<Sprite> ConjurerRockSprite =>
        new LoadableResourceAsset($"{ImpMiscPath}.ConjurerRockSprite.png");
    public static LoadableAsset<Sprite> ConjurerRockSpriteFallen =>
        new LoadableResourceAsset($"{ImpMiscPath}.ConjurerRockSpriteFallen.png");
    public static LoadableAsset<Sprite> SquashedDeadBodySprite =>
        new LoadableResourceAsset($"{ImpMiscPath}.SquashedDeadBodySprite.png");
    public static LoadableAsset<Sprite> SquashedDeadBodySpriteVisor =>
        new LoadableResourceAsset($"{ImpMiscPath}.SquashedDeadBodySpriteVisor.png");
    public static LoadableAsset<Sprite> EmergencyConsoleBroken =>
        new LoadableResourceAsset($"{ImpMiscPath}.EmergencyConsoleBroken.png");
    public static LoadableAsset<Sprite> PoisonedModifierIcon =>
        new LoadableResourceAsset($"{ImpMiscPath}.PoisonedModifierIcon.png");
    public static LoadableAsset<Sprite> ObstructedButtonOverlay =>
        new LoadableResourceAsset($"{ImpMiscPath}.ObstructedButtonOverlay.png");



    // ===============================================================
    //                          NEUTRAL
    // ===============================================================

    // --- Role Icons ---

    // Evil
    public static LoadableAsset<Sprite> PoltergeistRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.PoltergeistRoleIcon.png", 200);
    public static LoadableAsset<Sprite> TricksterRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.TricksterRoleIcon.png", 200);
    public static LoadableAsset<Sprite> VultureRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.VultureRoleIcon.png", 200);

    // Outlier
    public static LoadableAsset<Sprite> ShifterRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.ShifterRoleIcon.png");
    
    // Killing
    public static LoadableAsset<Sprite> SquidRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.SquidRoleIcon.png", 200);
    public static LoadableAsset<Sprite> ShadowWalkerRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.ShadowWalkerRoleIcon.png", 200);
    public static LoadableAsset<Sprite> CannibalRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.CannibalRoleIcon.png");
    public static LoadableAsset<Sprite> BarbarianRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.BarbarianRoleIcon.png", 200);
    public static LoadableAsset<Sprite> ClownRoleIcon =>
        new LoadableResourceAsset($"{NeutRoleIconPath}.ClownRoleIcon.png", 200);

    // --- Buttons ---

    public static LoadableAsset<Sprite> TricksterSampleButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.TricksterSampleButton.png");
    public static LoadableAsset<Sprite> TricksterPlaceButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.TricksterPlaceButton.png");
    public static LoadableAsset<Sprite> ShifterShiftButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.ShifterShiftButton.png", 400);
    public static LoadableAsset<Sprite> VultureEatButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.VultureEatButton.png");
    public static LoadableAsset<Sprite> PoltergeistPossessButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.PoltergeistPossessButton.png");
    public static LoadableAsset<Sprite> PoltergeistScareButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.PoltergeistScareButton.png");
    public static LoadableAsset<Sprite> CannibalEatButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.CannibalEatButton.png");
    public static LoadableAsset<Sprite> ShadowWalkerKillButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.ShadowWalkerKillButton.png");
    public static LoadableAsset<Sprite> ShadowWalkerEnshroudButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.ShadowWalkerEnshroudButton.png");
    public static LoadableAsset<Sprite> SquidKillButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.SquidKillButton.png");
    public static LoadableAsset<Sprite> SquidSpillButton =>
        new LoadableResourceAsset($"{NeutMiscPath}.SquidInkPuddle.png", 225);
    public static LoadableAsset<Sprite> BarbarianTargetButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.BarbarianTargetButton.png", 200);
    public static LoadableAsset<Sprite> BarbarianAttackButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.BarbarianAttackButton.png");
    public static LoadableAsset<Sprite> ClownPlaceButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.ClownPlaceButton.png");
    public static LoadableAsset<Sprite> ClownKillButton =>
        new LoadableResourceAsset($"{NeutButtonPath}.ClownKillButton.png");
    
    // --- Misc ---
    
    public static LoadableAsset<Sprite> SquidInkPuddle =>
        new LoadableResourceAsset($"{NeutMiscPath}.SquidInkPuddle.png", 230);
    public static LoadableAsset<Sprite> ClownJackInTheBox =>
        new LoadableResourceAsset($"{NeutMiscPath}.ClownJackInTheBox.png", 200);



    // ===============================================================
    //                         UNIVERSAL
    // ===============================================================

    // --- Modifiers ---

    // Passive
    public static LoadableAsset<Sprite> SoullessModifierIcon =>
        new LoadableResourceAsset($"{UniModModIconPath}.SoullessModifierIcon.png", 200);
    public static LoadableAsset<Sprite> ApoliticalModifierIcon =>
        new LoadableResourceAsset($"{UniModModIconPath}.ApoliticalModifierIcon.png", 200);
    public static LoadableAsset<Sprite> MuteModifierIcon =>
        new LoadableResourceAsset($"{UniModModIconPath}.MuteModifierIcon.png", 200);
    public static LoadableAsset<Sprite> YouthlingModifierIcon =>
        new LoadableResourceAsset($"{UniModModIconPath}.YouthlingModifierIcon.png", 200);
    
    
    
    // ===============================================================
    //                         NON CREW
    // ===============================================================

    // --- Modifiers ---

    // Passive
    public static LoadableAsset<Sprite> ScourgeModifierIcon =>
        new LoadableResourceAsset($"{NonCrewModModIconPath}.ScourgeRoleIcon.png");
    


    // ===============================================================
    //                           MISC
    // ===============================================================

    public static LoadableAsset<Sprite> TownOfExtraIcon =>
        new LoadableResourceAsset($"{MiscPath}.TownOfExtraIcon.png", 250);
}