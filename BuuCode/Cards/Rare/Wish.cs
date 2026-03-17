using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class Wish() : BuuCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private const decimal KiBase = 8m;
    private const decimal KiUpgraded = 12m;
    private const int DrawBase = 2;
    private const int DrawUpgraded = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<KiPower>((int)KiBase),
        new CardsVar(DrawBase)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var amount = DynamicVars["KiPower"].IntValue;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, amount, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, amount, Owner.Creature, null);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["KiPower"].UpgradeValueBy((int)(KiUpgraded - KiBase));
        DynamicVars.Cards.UpgradeValueBy(DrawUpgraded - DrawBase);
    }
}
