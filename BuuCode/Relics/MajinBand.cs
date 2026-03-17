using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class MajinBand() : BuuRelic
{
    private const int StrengthPerTurn = 1;
    private const int FirstRoundWithEffect = 2;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber < FirstRoundWithEffect)
            return;

        await BuuStanceCmd.EnterMajin(Owner.Creature, null);

        if (Owner.Creature.Powers.OfType<MajinStance>().Any())
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, StrengthPerTurn, Owner.Creature, null);
        Flash();
    }
}

