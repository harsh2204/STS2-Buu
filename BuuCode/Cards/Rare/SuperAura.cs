using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class SuperAura() : BuuCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BuuStanceCmd.EnterSuper(Owner.Creature, choiceContext);
    }

    protected override void OnUpgrade() { }
}
