using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class SplitCell() : BuuRelic
{
    private const int HealPerTurn = 1;
    private const decimal BlockFromRoundTwo = 1m;
    private const int FirstRound = 1;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        await CreatureCmd.Heal(Owner.Creature, HealPerTurn, true);
        if (combatState.RoundNumber > FirstRound)
            await CreatureCmd.GainBlock(Owner.Creature, BlockFromRoundTwo, ValueProp.Unpowered, null);
        Flash();
    }
}

