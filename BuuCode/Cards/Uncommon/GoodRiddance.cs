using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class GoodRiddance() : BuuCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BuuStanceCmd.ExitStance(Owner.Creature, choiceContext);
    }

    protected override void OnUpgrade() { }
}
