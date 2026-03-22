using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class PrimordialSlime() : BuuRelic
{
    private const decimal ActEntryHeal = 5m;

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override async Task AfterActEntered()
    {
        Flash();
        await CreatureCmd.Heal(Owner.Creature, ActEntryHeal);
    }
}
