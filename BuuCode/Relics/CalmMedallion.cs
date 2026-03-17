using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class CalmMedallion() : BuuRelic
{
    private const int HealPerTurn = 1;
    private const decimal BlockInRegular = 4m;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (Owner.Creature.Powers.OfType<RegularStance>().Any())
            await CreatureCmd.GainBlock(Owner.Creature, BlockInRegular, ValueProp.Unpowered, null);
        else
            await CreatureCmd.Heal(Owner.Creature, HealPerTurn, true);
        Flash();
    }
}

