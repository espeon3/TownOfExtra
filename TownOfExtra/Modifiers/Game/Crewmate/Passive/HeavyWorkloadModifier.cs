using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Crewmate.Passive;

public class HeavyWorkloadModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier
{
    public override string ModifierName => "Heavy Workload";
    public override ModifierFaction FactionType => ModifierFaction.CrewmatePassive;
    public override string IntroInfo => "You have extra tasks to finish.";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.HeavyWorkloadModifierIcon;
    public Color ModifierColor => TownOfExtraColours.HeavyWorkloadModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.HeavyWorkloadModifierColour;

    public static float ExtraCommonTasks => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadExtraCommonTasks.Value;
    public static float ExtraLongTasks => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadExtraLongTasks.Value;
    public static float ExtraShortTasks => OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadExtraShortTasks.Value;

    public override string GetDescription()
    {
        return "You have an increased amount of tasks!";
    }

    public string GetAdvancedDescription()
    {
        return "You have more tasks than the regular crewmates.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<CrewmateModifierOptions>.Instance.HeavyWorkloadChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is SnitchRole || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead || !role.IsCrewmate())
            return false;

        return true;
    }

    public override void OnActivate()
    {
        if (PlayerControl.LocalPlayer != Player) return;

        var tasks = Player.Data.Tasks;
        if (tasks == null) return;

        var commonTasks = ShipStatus.Instance.CommonTasks;
        var longTasks = ShipStatus.Instance.LongTasks;
        var shortTasks = ShipStatus.Instance.ShortTasks;

        var toAdd = new System.Collections.Generic.List<NormalPlayerTask>();

        for (int i = 0; i < (int)ExtraCommonTasks; i++)
            toAdd.Add(commonTasks[Random.Range(0, commonTasks.Length)]);
        for (int i = 0; i < (int)ExtraLongTasks; i++)
            toAdd.Add(longTasks[Random.Range(0, longTasks.Length)]);
        for (int i = 0; i < (int)ExtraShortTasks; i++)
            toAdd.Add(shortTasks[Random.Range(0, shortTasks.Length)]);

        foreach (var taskPrefab in toAdd)
        {
            var taskObj = Object.Instantiate(taskPrefab, Player.transform);
            taskObj.Id = (uint)tasks.Count;
            taskObj.Owner = Player;
            taskObj.Initialize();
            TownOfExtraPlugin.Logger.LogInfo($"Added task {taskPrefab.name} to {Player.Data.PlayerName}");
            Player.myTasks.Add(taskObj);
            tasks.Add(CreateTaskInfo(taskObj.Id));
        }
    }

    private static NetworkedPlayerInfo.TaskInfo CreateTaskInfo(uint id)
    {
        var ptr = Il2CppInterop.Runtime.IL2CPP.il2cpp_object_new(
            Il2CppInterop.Runtime.Il2CppClassPointerStore<NetworkedPlayerInfo.TaskInfo>.NativeClassPtr
        );
        var taskInfo = new NetworkedPlayerInfo.TaskInfo(ptr);
        taskInfo.Id = id;
        taskInfo.Complete = false;
        return taskInfo;
    }
}