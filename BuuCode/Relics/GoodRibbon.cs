using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class GoodRibbon() : BuuRelic
{
    private const decimal BlockPerTurn = 1m;
    private const int DexterityInRegular = 1;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (Owner.Creature.Powers.OfType<RegularStance>().Any())
            await PowerCmd.Apply<DexterityPower>(Owner.Creature, DexterityInRegular, Owner.Creature, null);
        else
            await CreatureCmd.GainBlock(Owner.Creature, BlockPerTurn, ValueProp.Unpowered, null);
        Flash();
    }
}

