using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class AbsorptionCell() : BuuRelic
{
    private const decimal KiAtCombatStart = 5m;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber != 1)
            return;

        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, KiAtCombatStart, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, KiAtCombatStart, Owner.Creature, null);
        Flash();
    }
}
