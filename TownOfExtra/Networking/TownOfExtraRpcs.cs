namespace TownOfExtra.Networking;

public enum TownOfExtraRpcs : uint
{
    SendNotification = 400,
    AwardAchievement = 401,
    IncrementAchievement = 402,
    
    
    TricksterNotifyOfReport = 500,
    TricksterPlaceFakeBody = 501,
    TricksterDestroyFakeBodies = 502,
    CannibalNotifyDead = 503,
    CannibalReviveVictims = 504,
    BrittleTriggerModifier = 505,
    VinculatorEmpowerTeam = 506,
    VultureCleanBody = 507,
    VultureChangeToAmne = 508,
    ConjurerPlaceRock = 509,
    HolographerSyncFakePlayer = 510,
    SendJournalistChat = 511,
    SquidSpillInk = 512,
    SquidDestroyInk = 513,
    RebirthSendPopup = 514,
    BarbarianNotifyTargetDeath = 515,
    CommanderIncreaseAvengeUses = 516,
    ObstructorTriggerObstruct = 517,
}