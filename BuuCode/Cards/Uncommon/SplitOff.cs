using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class SplitOff() : BuuCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private const int DrawBase = 1;
    private const int DrawUpgraded = 2;

    public override HashSet<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(DrawBase)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        Owner.PlayerCombatState?.GainEnergy(1);
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(DrawUpgraded - DrawBase);
}
