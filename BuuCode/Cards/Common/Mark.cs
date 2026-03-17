using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Common;

[Pool(typeof(BuuCardPool))]
public sealed class Mark() : BuuCard(1, CardType.Power, CardRarity.Common, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MarkPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<MarkPower>(Owner.Creature, DynamicVars["MarkPower"].IntValue, Owner.Creature, this);

    protected override void OnUpgrade()
    {
        DynamicVars["MarkPower"].UpgradeValueBy(1);
        EnergyCost.UpgradeBy(-1);
    }
}
