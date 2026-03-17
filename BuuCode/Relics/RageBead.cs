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
public sealed class RageBead() : BuuRelic
{
    private const decimal BlockOutsideMajin = 2m;
    private const int StrengthInMajin = 1;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (Owner.Creature.Powers.OfType<MajinStance>().Any())
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, StrengthInMajin, Owner.Creature, null);
        else
            await CreatureCmd.GainBlock(Owner.Creature, BlockOutsideMajin, ValueProp.Unpowered, null);
        Flash();
    }
}

