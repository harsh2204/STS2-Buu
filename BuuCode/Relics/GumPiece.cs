using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class GumPiece() : BuuRelic
{
    private const decimal BlockPerTurn = 2m;
    private const int HealOnEveryThirdTurn = 1;
    private const int ThirdTurnInterval = 3;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        await CreatureCmd.GainBlock(Owner.Creature, BlockPerTurn, ValueProp.Unpowered, null);
        if (combatState.RoundNumber % ThirdTurnInterval == 0)
            await CreatureCmd.Heal(Owner.Creature, HealOnEveryThirdTurn, true);
        Flash();
    }
}
