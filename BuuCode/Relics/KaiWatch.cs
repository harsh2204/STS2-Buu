using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class KaiWatch() : BuuRelic
{
    private const decimal KiPerTurn = 1m;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        await BuuStanceCmd.EnterSuper(Owner.Creature, null);

        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, KiPerTurn, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, KiPerTurn, Owner.Creature, null);
        Flash();
    }
}

