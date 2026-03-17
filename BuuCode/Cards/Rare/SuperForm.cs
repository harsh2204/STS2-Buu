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

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class SuperForm() : BuuCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private const decimal KiCost = 2m;

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12m, ValueProp.Move)];

    protected override bool IsPlayable =>
        Owner?.Creature != null && Owner.Creature.GetPowerAmount<KiPower>() >= KiCost;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        var kiPower = creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, -KiCost, null, null);

        await CreatureCmd.GainBlock(creature, DynamicVars.Block, cardPlay);
        Owner.PlayerCombatState?.GainEnergy(2);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(5m);
    }
}
