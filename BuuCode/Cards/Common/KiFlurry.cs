using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Common;

[Pool(typeof(BuuCardPool))]
public sealed class KiFlurry() : BuuCard(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    private const decimal DamageBase = 8m;
    private const decimal DamageUpgraded = 12m;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(DamageBase, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
}
