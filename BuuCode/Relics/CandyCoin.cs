using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class CandyCoin() : BuuRelic
{
    private const decimal KiOnOddTurns = 1m;
    private const int DexterityOnEvenTurns = 1;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (combatState.RoundNumber % 2 == 0)
        {
            await PowerCmd.Apply<DexterityPower>(Owner.Creature, DexterityOnEvenTurns, Owner.Creature, null);
        }
        else
        {
            var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
            if (kiPower != null)
                await PowerCmd.ModifyAmount(kiPower, KiOnOddTurns, null, null);
            else
                await PowerCmd.Apply<KiPower>(Owner.Creature, KiOnOddTurns, Owner.Creature, null);
        }
        Flash();
    }
}
