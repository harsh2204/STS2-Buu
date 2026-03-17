using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using Buu.BuuCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class PinkOrbFragment() : BuuRelic
{
    private const decimal BlockPerTurn = 1m;
    private const decimal KiInSuper = 2m;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (Owner.Creature.Powers.OfType<SuperStance>().Any())
        {
            var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
            if (kiPower != null)
                await PowerCmd.ModifyAmount(kiPower, KiInSuper, null, null);
            else
                await PowerCmd.Apply<KiPower>(Owner.Creature, KiInSuper, Owner.Creature, null);
        }
        else
        {
            await CreatureCmd.GainBlock(Owner.Creature, BlockPerTurn, ValueProp.Unpowered, null);
        }
        Flash();
    }
}

