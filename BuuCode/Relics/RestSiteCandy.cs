using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class RestSiteCandy() : BuuRelic
{
    private const decimal BonusRestHeal = 4m;

    public override RelicRarity Rarity => RelicRarity.Event;

    public override decimal ModifyRestSiteHealAmount(Creature creature, decimal amount)
    {
        if (creature.Player != Owner && creature.PetOwner != Owner)
            return amount;
        return amount + BonusRestHeal;
    }
}
