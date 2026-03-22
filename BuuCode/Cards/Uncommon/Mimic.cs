using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class Mimic() : BuuCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private const int DrawBase = 2;
    private const int DrawUpgraded = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(DrawBase)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(DrawUpgraded - DrawBase);
}
