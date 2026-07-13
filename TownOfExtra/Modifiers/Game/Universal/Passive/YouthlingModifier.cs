using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfExtra.Options;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TownOfExtra.Modifiers.Game.Universal.Passive;

public class YouthlingModifier : TouGameModifier, IWikiDiscoverable, IColoredModifier, IVisualAppearance
{
    public override string ModifierName => "Youthling";
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;
    public override string IntroInfo => "No one can harm you until you grow up";
    public override LoadableAsset<Sprite> ModifierIcon => TownOfExtraAssets.YouthlingModifierIcon;
    public Color ModifierColor => TownOfExtraColours.YouthlingModifierColour;
    public override Color FreeplayFileColor => TownOfExtraColours.YouthlingModifierColour;
    public override bool PreventsOtherModifiers => true;

    public override string GetDescription()
    {
        return "You cannot be killed until you reach 18 years old.";
    }

    public string GetAdvancedDescription()
    {
        return $"You cannot be killed until you reach 18 years old. Your age increases by 1 every {OptionGroupSingleton<UniversalModifierOptions>.Instance.YouthlingTimeBetweenAge.Value} seconds.";
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.YouthlingAmount;
    }

    public override int GetAssignmentChance()
    {
        return OptionGroupSingleton<UniversalModifierOptions>.Instance.YouthlingChance;
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        if (!base.IsModifierValidOn(role) || role is HaunterRole || role is SpectreRole)
            return false;

        var player = role.Player;
        if (player == null || player.Data == null || player.Data.IsDead)
            return false;

        return true;
    }
    
    public VisualAppearance GetVisualAppearance()
    {
        var appearance = Player.GetDefaultAppearance();
        var scale = Mathf.Lerp(0.35f, 0.7f, Age / 18f);
        appearance.Size = new Vector3(scale, scale, 1f);
        return appearance;
    }
    
    private float _timer;
    public int Age;

    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
    }

    public override void FixedUpdate()
    {
        if (Age >= 18 || MeetingHud.Instance) return;

        _timer += Time.fixedDeltaTime;

        if (_timer >= OptionGroupSingleton<UniversalModifierOptions>.Instance.YouthlingTimeBetweenAge.Value)
        {
            _timer = 0f;
            Age++;
            Player.RawSetAppearance(this);
        }
    }
}