using BaseLib.Abstracts;
using Buu.BuuCode.Cards.Basic;
using Buu.BuuCode.Relics;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Buu.BuuCode.Character;

public class Buu : PlaceholderCharacterModel
{
    public const string CharacterId = "Buu";
    public const string ResPrefix = "res://Buu/";

    public static readonly Color Color = new("ff69b4"); // Buu pink
    public static readonly Color EnergyOutlineColor = new("ff69b4");
    public static readonly Color EnergyBurstColor = new("ff1493");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 72;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<KiBlast>(),
        ModelDb.Card<KiBlast>(),
        ModelDb.Card<KiBlast>(),
        ModelDb.Card<Punch>(),
        ModelDb.Card<Punch>(),
        ModelDb.Card<Guard>(),
        ModelDb.Card<Guard>(),
        ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(),
        ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(),
        ModelDb.Card<GoodForm>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CandyShell>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<BuuCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<BuuRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<BuuPotionPool>();

    public override string CustomIconTexturePath => ResPrefix + "images/charui/character_icon_buu.png";
    public override string CustomCharacterSelectBg => ResPrefix + "scenes/buu/char_select_bg_buu.tscn";
    public override string CustomCharacterSelectIconPath => ResPrefix + "images/packed/character_select/character_select_buu.png";
    public override string CustomCharacterSelectLockedIconPath => ResPrefix + "images/packed/character_select/character_select_locked_buu.png";
    public override string CustomMapMarkerPath => ResPrefix + "images/charui/map_marker_buu.png";
    public override string CustomIconPath => ResPrefix + "scenes/buu/buu_icon.tscn";
    public override string CustomVisualPath => ResPrefix + "scenes/buu/buu.tscn";

    public override string CustomArmPointingTexturePath => ResPrefix + "images/buu/hands/multiplayer_hand_buu_point.png";
    public override string CustomArmRockTexturePath => ResPrefix + "images/buu/hands/multiplayer_hand_buu_rock.png";
    public override string CustomArmPaperTexturePath => ResPrefix + "images/buu/hands/multiplayer_hand_buu_paper.png";
    public override string CustomArmScissorsTexturePath => ResPrefix + "images/buu/hands/multiplayer_hand_buu_scissors.png";

    public override CustomEnergyCounter? CustomEnergyCounter =>
        new CustomEnergyCounter(BuuEnergyLayerPath, EnergyOutlineColor, EnergyBurstColor);

    private static string BuuEnergyLayerPath(int layer) =>
        ResPrefix + "images/ui/combat/energy_counters/buu/buu_orb_layer_" + layer + ".png";

    public override CreatureAnimator? SetupCustomAnimationStates(MegaSprite controller)
    {
        // Idle, Attack, Cast (block/skills → "defend"), Relaxed (rest → "heal"). Hit/Dead fall back to idle.
        return SetupAnimationState(controller,
            idleName: "idle",
            deadName: "idle", deadLoop: true,
            hitName: "idle", hitLoop: true,
            attackName: "attack", attackLoop: false,
            castName: "defend", castLoop: false,
            relaxedName: "heal", relaxedLoop: false);
    }
}