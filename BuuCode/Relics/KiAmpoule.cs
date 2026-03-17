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
public sealed class KiAmpoule() : BuuRelic
{
    private const decimal KiPerTurn = 1m;
    private const int DexterityAtCombatStart = 1;
    private const int FirstRound = 1;

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;

        if (combatState.RoundNumber == FirstRound)
            await PowerCmd.Apply<DexterityPower>(Owner.Creature, DexterityAtCombatStart, Owner.Creature, null);

        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, KiPerTurn, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, KiPerTurn, Owner.Creature, null);
        Flash();
    }
}

