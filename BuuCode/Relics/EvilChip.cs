using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using Buu.BuuCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class EvilChip() : BuuRelic
{
    private const decimal KiPerTurnOutsideMajin = 1m;
    private const int StrengthPerTurnInMajin = 1;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (Owner.Creature.Powers.OfType<MajinStance>().Any())
        {
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, StrengthPerTurnInMajin, Owner.Creature, null);
        }
        else
        {
            var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
            if (kiPower != null)
                await PowerCmd.ModifyAmount(kiPower, KiPerTurnOutsideMajin, null, null);
            else
                await PowerCmd.Apply<KiPower>(Owner.Creature, KiPerTurnOutsideMajin, Owner.Creature, null);
        }
        Flash();
    }
}

