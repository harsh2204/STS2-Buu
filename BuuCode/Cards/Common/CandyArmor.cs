using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Common;

[Pool(typeof(BuuCardPool))]
public sealed class CandyArmor() : BuuCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    private const decimal KiCostOptional = 1m;
    private const decimal BlockBase = 8m;
    private const decimal BlockUpgraded = 12m;
    private const decimal BlockBonusWithKi = 5m;

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(BlockBase, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        var kiPower = creature.Powers.OfType<KiPower>().FirstOrDefault();
        var hasKi = kiPower != null && creature.GetPowerAmount<KiPower>() >= KiCostOptional;
        if (hasKi && kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, -KiCostOptional, null, null);

        await CreatureCmd.GainBlock(creature, DynamicVars.Block, cardPlay);
        if (hasKi)
            await CreatureCmd.GainBlock(creature, new BlockVar(BlockBonusWithKi, ValueProp.Move), cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(BlockUpgraded - BlockBase);
    }
}
