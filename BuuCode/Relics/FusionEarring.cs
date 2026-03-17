using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class FusionEarring() : BuuRelic
{
    private const decimal KiFromRoundTwo = 2m;
    private const decimal BlockFromRoundTwo = 2m;
    private const int FirstRound = 1;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side || combatState.RoundNumber <= FirstRound)
            return;

        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, KiFromRoundTwo, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, KiFromRoundTwo, Owner.Creature, null);

        await CreatureCmd.GainBlock(Owner.Creature, BlockFromRoundTwo, ValueProp.Unpowered, null);
        Flash();
    }
}

