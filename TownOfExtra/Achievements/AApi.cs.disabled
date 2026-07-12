#nullable enable
using AchievementsAPI.API;
using TownOfExtra.Modules;

namespace TownOfExtra.Achievements;

public class AApi
{
    public static ToexAchievementsTab? GetInstance()
    {
        if (ModCompat.IsLoaded(ModCompat.AApiId, out _))
        {
            return AchievementsTabSingleton<ToexAchievementsTab>.Instance;
        }

        return null;
    }

    public static void AwardAchievement(BaseAchievement? achievement)
    {
        if (achievement == null) return;

        TownOfExtraPlugin.Logger.LogWarning($"Attempting to award achievement {achievement.Name}");
        
        if (!ModCompat.IsLoaded(ModCompat.AApiId, out _))
        {
            TownOfExtraPlugin.Logger.LogWarning("Achievements API not found, achievement will not be awarded");
            return;
        }

        var tab = GetInstance();
        if (tab == null) return;
        
        achievement.Unlock();
        TownOfExtraPlugin.Logger.LogInfo($"Achievement {achievement.Name} awarded");
    }
    
    public static void IncrementAchievement(CountAchievement? achievement, int increment)
    {
        if (achievement == null) return;

        TownOfExtraPlugin.Logger.LogWarning($"Attempting to increment achievement {achievement.Name} by {increment}");
        
        if (!ModCompat.IsLoaded(ModCompat.AApiId, out _))
        {
            TownOfExtraPlugin.Logger.LogWarning("Achievements API not found, achievement will not be incremented");
            return;
        }

        var tab = GetInstance();
        if (tab == null) return;
        
        achievement.Increment(increment);
        TownOfExtraPlugin.Logger.LogInfo($"Achievement {achievement.Name} incremented by {increment}");
    }
}