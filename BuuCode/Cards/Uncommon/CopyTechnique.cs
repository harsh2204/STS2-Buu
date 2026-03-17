using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class CopyTechnique() : BuuCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private const int KiGain = 1;
    private const int DrawBase = 2;
    private const int DrawUpgraded = 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(DrawBase)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var handCards = PileType.Hand.GetPile(Owner).Cards.Where(c => c != this).ToList();
        if (handCards.Count > 0)
        {
            var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
            var chosen = (await CardSelectCmd.FromSimpleGrid(choiceContext, handCards, Owner, prefs)).FirstOrDefault();
            if (chosen != null)
                await CardCmd.Exhaust(choiceContext, chosen);
        }
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        await PowerCmd.Apply<KiPower>(Owner.Creature, KiGain, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(DrawUpgraded - DrawBase);
}
