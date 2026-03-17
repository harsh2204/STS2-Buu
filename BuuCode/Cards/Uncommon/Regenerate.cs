using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class Regenerate() : BuuCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private const decimal KiBase = 3m;
    private const decimal KiUpgraded = 5m;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<KiPower>((int)KiBase)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var amount = DynamicVars["KiPower"].IntValue;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, amount, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, amount, Owner.Creature, null);
    }

    protected override void OnUpgrade() => DynamicVars["KiPower"].UpgradeValueBy((int)(KiUpgraded - KiBase));
}
