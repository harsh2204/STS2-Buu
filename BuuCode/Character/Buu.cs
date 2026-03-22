using BaseLib.Abstracts;
using Buu.BuuCode.Cards.Basic;
using Buu.BuuCode.Cards.Ancient;
using Buu.BuuCode.Cards.Common;
using Buu.BuuCode.Cards.Rare;
using Buu.BuuCode.Cards.Uncommon;
using Buu.BuuCode.Config;
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

    private static IEnumerable<CardModel> StarterDeck => BuuModConfig.StarterDeckSelection switch
    {
        StarterDeckSelection.Default => BuuStarterDecks.Default,
        StarterDeckSelection.StanceDance => BuuStarterDecks.StanceDance,
        StarterDeckSelection.KiEngine => BuuStarterDecks.KiEngine,
        StarterDeckSelection.AggroMajin => BuuStarterDecks.AggroMajin,
        StarterDeckSelection.BlockReform => BuuStarterDecks.BlockReform,
        StarterDeckSelection.SuperBurst => BuuStarterDecks.SuperBurst,
        StarterDeckSelection.CommonPile => BuuStarterDecks.CommonPile,
        StarterDeckSelection.UncommonHeavy => BuuStarterDecks.UncommonHeavy,
        StarterDeckSelection.RareShowcase => BuuStarterDecks.RareShowcase,
        StarterDeckSelection.BasicsAndCommons => BuuStarterDecks.BasicsAndCommons,
        StarterDeckSelection.StanceOnly => BuuStarterDecks.StanceOnly,
        StarterDeckSelection.FullSpread => BuuStarterDecks.FullSpread,
        StarterDeckSelection.AllCards => BuuStarterDecks.AllCards,
        _ => BuuStarterDecks.Default
    };

    public override IEnumerable<CardModel> StartingDeck => StarterDeck;

    private static class BuuStarterDecks
    {
        public static IEnumerable<CardModel> Default => [
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(),
            ModelDb.Card<Punch>(), ModelDb.Card<Punch>(),
            ModelDb.Card<Guard>(), ModelDb.Card<Guard>(),
            ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(), ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(),
            ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<GoodRiddance>(), ModelDb.Card<SuperAura>(),
            ModelDb.Card<KiWell>()
        ];
        public static IEnumerable<CardModel> StanceDance => [
            ModelDb.Card<GoodForm>(), ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>(),
            ModelDb.Card<GoodRiddance>(), ModelDb.Card<SuperAura>(),
            ModelDb.Card<Guard>(), ModelDb.Card<Guard>(), ModelDb.Card<Reform>(),
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>()
        ];
        public static IEnumerable<CardModel> KiEngine => [
            ModelDb.Card<Regenerate>(), ModelDb.Card<Regenerate>(), ModelDb.Card<FullAbsorption>(),
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(),
            ModelDb.Card<KiStrike>(), ModelDb.Card<KiBarrier>(),
            ModelDb.Card<Guard>(), ModelDb.Card<Reform>(), ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>()
        ];
        public static IEnumerable<CardModel> AggroMajin => [
            ModelDb.Card<EvilEmerges>(), ModelDb.Card<Punch>(), ModelDb.Card<Punch>(),
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<VanillaBeam>(),
            ModelDb.Card<FlurryPunch>(), ModelDb.Card<MajinBurst>(),
            ModelDb.Card<Guard>(), ModelDb.Card<GoodForm>()
        ];
        public static IEnumerable<CardModel> BlockReform => [
            ModelDb.Card<Guard>(), ModelDb.Card<Guard>(), ModelDb.Card<BubbleBarrier>(), ModelDb.Card<CandyArmor>(),
            ModelDb.Card<Reform>(), ModelDb.Card<Reform>(), ModelDb.Card<GoodForm>(), ModelDb.Card<SuperForm>(),
            ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>()
        ];
        public static IEnumerable<CardModel> SuperBurst => [
            ModelDb.Card<SuperAura>(), ModelDb.Card<SuperForm>(), ModelDb.Card<VanillaBeam>(),
            ModelDb.Card<Punch>(), ModelDb.Card<Punch>(), ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(),
            ModelDb.Card<Guard>(), ModelDb.Card<GoodRiddance>(), ModelDb.Card<GoodForm>()
        ];
        public static IEnumerable<CardModel> CommonPile => [
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<CandyBeam>(), ModelDb.Card<FlurryPunch>(),
            ModelDb.Card<Guard>(), ModelDb.Card<BubbleBarrier>(), ModelDb.Card<StretchArm>(), ModelDb.Card<ExplosiveBall>(),
            ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>()
        ];
        public static IEnumerable<CardModel> UncommonHeavy => [
            ModelDb.Card<Reform>(), ModelDb.Card<PinkBlast>(), ModelDb.Card<VanillaBeam>(),
            ModelDb.Card<Regenerate>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<GoodRiddance>(),
            ModelDb.Card<Guard>(), ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>(), ModelDb.Card<GoodForm>()
        ];
        public static IEnumerable<CardModel> RareShowcase => [
            ModelDb.Card<FullAbsorption>(), ModelDb.Card<MajinBurst>(), ModelDb.Card<SuperForm>(), ModelDb.Card<SuperAura>(),
            ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<Guard>(), ModelDb.Card<Guard>(),
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>()
        ];
        public static IEnumerable<CardModel> BasicsAndCommons => [
            ModelDb.Card<KiBlast>(), ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>(), ModelDb.Card<Guard>(),
            ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(), ModelDb.Card<GoodForm>(),
            ModelDb.Card<CandyBeam>(), ModelDb.Card<FlurryPunch>(), ModelDb.Card<BubbleBarrier>(), ModelDb.Card<StretchArm>(),
            ModelDb.Card<ExplosiveBall>(), ModelDb.Card<CandyArmor>(), ModelDb.Card<KiStrike>(), ModelDb.Card<KiBarrier>()
        ];
        public static IEnumerable<CardModel> StanceOnly => [
            ModelDb.Card<GoodForm>(), ModelDb.Card<GoodForm>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<EvilEmerges>(),
            ModelDb.Card<GoodRiddance>(), ModelDb.Card<SuperAura>(), ModelDb.Card<SuperAura>(),
            ModelDb.Card<Guard>(), ModelDb.Card<Guard>(), ModelDb.Card<KiBlast>()
        ];
        public static IEnumerable<CardModel> FullSpread => [
            ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>(), ModelDb.Card<Guard>(), ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(), ModelDb.Card<GoodForm>(),
            ModelDb.Card<CandyBeam>(), ModelDb.Card<FlurryPunch>(), ModelDb.Card<BubbleBarrier>(), ModelDb.Card<StretchArm>(), ModelDb.Card<ExplosiveBall>(), ModelDb.Card<CandyArmor>(), ModelDb.Card<KiStrike>(), ModelDb.Card<KiBarrier>(),
            ModelDb.Card<Reform>(), ModelDb.Card<PinkBlast>(), ModelDb.Card<VanillaBeam>(), ModelDb.Card<Regenerate>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<GoodRiddance>(),
            ModelDb.Card<FullAbsorption>(), ModelDb.Card<MajinBurst>(), ModelDb.Card<SuperForm>(), ModelDb.Card<SuperAura>()
        ];
        /// <summary>One copy of every card in the pool (113 cards). 100% coverage for testing.</summary>
        public static IEnumerable<CardModel> AllCards => [
            ModelDb.Card<KiBlast>(), ModelDb.Card<Punch>(), ModelDb.Card<Guard>(), ModelDb.Card<global::Buu.BuuCode.Cards.Basic.Headbutt>(), ModelDb.Card<GoodForm>(),
            ModelDb.Card<Absorb>(), ModelDb.Card<BubbleTrap>(), ModelDb.Card<CalmGuard>(), ModelDb.Card<CandyBeam>(), ModelDb.Card<CandyArmor>(), ModelDb.Card<CandyCrush>(), ModelDb.Card<CandyHeal>(), ModelDb.Card<CandyPrison>(),
            ModelDb.Card<CandyRain>(), ModelDb.Card<CandyWhip>(), ModelDb.Card<ChocolateBeam>(), ModelDb.Card<CunningStrike>(), ModelDb.Card<EnergyWave>(), ModelDb.Card<EvilGrin>(), ModelDb.Card<ExplosiveBall>(), ModelDb.Card<FatThrow>(),
            ModelDb.Card<FleetingBlast>(), ModelDb.Card<FlurryPunch>(), ModelDb.Card<GoodWill>(), ModelDb.Card<GutPunch>(), ModelDb.Card<KiBarrier>(), ModelDb.Card<KiFlurry>(), ModelDb.Card<KiStrike>(), ModelDb.Card<KiWell>(),
            ModelDb.Card<CandyChorus>(), ModelDb.Card<KiSurge>(), ModelDb.Card<BuuStudy>(), ModelDb.Card<BlockToKi>(), ModelDb.Card<BuuForesight>(), ModelDb.Card<RetainedKi>(), ModelDb.Card<Mark>(),
            ModelDb.Card<MadStrike>(), ModelDb.Card<PinkBarrage>(), ModelDb.Card<RagePunch>(),
            ModelDb.Card<RollAttack>(), ModelDb.Card<RubberBounce>(), ModelDb.Card<StretchArm>(), ModelDb.Card<StretchGuard>(), ModelDb.Card<StretchSnap>(), ModelDb.Card<Tantrum>(), ModelDb.Card<ViciousBite>(), ModelDb.Card<BubbleBarrier>(),
            ModelDb.Card<AbsorbAndGrow>(), ModelDb.Card<AbsorptionDrain>(), ModelDb.Card<AntennaBlast>(), ModelDb.Card<global::Buu.BuuCode.Cards.Uncommon.BodySlam>(), ModelDb.Card<CopyTechnique>(), ModelDb.Card<EvilAura>(), ModelDb.Card<EvilEmerges>(), ModelDb.Card<EvilRest>(),
            ModelDb.Card<EvilRiddance>(), ModelDb.Card<EvilStare>(), ModelDb.Card<EvilWill>(), ModelDb.Card<EonCovenant>(), ModelDb.Card<GoodRiddance>(), ModelDb.Card<HeavySlam>(), ModelDb.Card<InnocentAura>(), ModelDb.Card<InnocentHeal>(), ModelDb.Card<InnocentRest>(),
            ModelDb.Card<InnocentStare>(), ModelDb.Card<KaiBlessing>(), ModelDb.Card<KidBuuSpark>(), ModelDb.Card<KiVolley>(), ModelDb.Card<MajinMark>(), ModelDb.Card<MajinRage>(), ModelDb.Card<global::Buu.BuuCode.Cards.Uncommon.Mimic>(), ModelDb.Card<PinkBlast>(), ModelDb.Card<PrimordialBlast>(), ModelDb.Card<Reform>(),
            ModelDb.Card<ReformAgain>(), ModelDb.Card<ReformSmall>(), ModelDb.Card<Regenerate>(), ModelDb.Card<RubberBody>(), ModelDb.Card<SplitOff>(), ModelDb.Card<SuperFocus>(), ModelDb.Card<SuperSmug>(), ModelDb.Card<SuperStare>(),
            ModelDb.Card<SuperWill>(), ModelDb.Card<TurnToCandy>(), ModelDb.Card<VanillaBeam>(),
            ModelDb.Card<MajinFury>(), ModelDb.Card<SuperSpirit>(), ModelDb.Card<RegenerativeForm>(),
            ModelDb.Card<MajinDraw>(), ModelDb.Card<StanceGuard>(), ModelDb.Card<CalmBlock>(), ModelDb.Card<CalmAura>(), ModelDb.Card<PinkWave>(), ModelDb.Card<StanceDuality>(), ModelDb.Card<BuuFasting>(), ModelDb.Card<RegularForm>(),
            ModelDb.Card<BuuFusion>(), ModelDb.Card<ChocolateRain>(), ModelDb.Card<EternalBuu>(), ModelDb.Card<FullAbsorption>(), ModelDb.Card<MajinBurst>(), ModelDb.Card<MajinLegacy>(), ModelDb.Card<PinkNova>(), ModelDb.Card<SuperAura>(),
            ModelDb.Card<SuperFinisher>(), ModelDb.Card<SuperForm>(), ModelDb.Card<VanishingBall>(), ModelDb.Card<global::Buu.BuuCode.Cards.Rare.Wish>(),
            ModelDb.Card<global::Buu.BuuCode.Cards.Rare.Rage>(), ModelDb.Card<Omega>(), ModelDb.Card<Blasphemer>(), ModelDb.Card<BuuDeva>(), ModelDb.Card<MasterReality>()
        ];
    }

    public override IReadOnlyList<RelicModel> StartingRelics =>
        BuuModConfig.StartWithAllModRelics ? BuuStartingRelics.All : BuuStartingRelics.Default;

    private static class BuuStartingRelics
    {
        public static IReadOnlyList<RelicModel> Default =>
        [
            ModelDb.Relic<CandyShell>()
        ];

        public static IReadOnlyList<RelicModel> All =>
        [
            ModelDb.Relic<CandyShell>(),
            ModelDb.Relic<MajinCrest>(),
            ModelDb.Relic<AbsorptionCell>(),
            ModelDb.Relic<RegenerationPod>(),
            ModelDb.Relic<BuuAntennaCharm>(),
            ModelDb.Relic<Capsule>(),
            ModelDb.Relic<PrimordialSlime>(),
            ModelDb.Relic<PinkCrystal>(),
            ModelDb.Relic<SupremeKaiEarring>(),
            ModelDb.Relic<GumPiece>(),
            ModelDb.Relic<ChocolateBar>(),
            ModelDb.Relic<KiAmpoule>(),
            ModelDb.Relic<SplitCell>(),
            ModelDb.Relic<BuuMedal>(),
            ModelDb.Relic<CandyWrapper>(),
            ModelDb.Relic<AntennaRing>(),
            ModelDb.Relic<KaiWatch>(),
            ModelDb.Relic<RegenerationSeed>(),
            ModelDb.Relic<AbsorptionOrb>(),
            ModelDb.Relic<MajinBand>(),
            ModelDb.Relic<PinkOrbFragment>(),
            ModelDb.Relic<CandyCoin>(),
            ModelDb.Relic<BuuBadge>(),
            ModelDb.Relic<StretchGel>(),
            ModelDb.Relic<SweetVendorChit>(),
            ModelDb.Relic<EvilChip>(),
            ModelDb.Relic<GoodRibbon>(),
            ModelDb.Relic<FusionEarring>(),
            ModelDb.Relic<RageBead>(),
            ModelDb.Relic<CalmMedallion>(),
            ModelDb.Relic<ReformShard>(),
            ModelDb.Relic<RestSiteCandy>(),
            ModelDb.Relic<BuuEssence>()
        ];
    }

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
    public override string CustomMerchantAnimPath => ResPrefix + "scenes/buu/buu_merchant.tscn";

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