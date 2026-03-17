using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class VanishingBall() : BuuCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const decimal DamageBase = 16m;
    private const decimal DamageUpgraded = 24m;
    private const int DrawBase = 1;
    private const int DrawUpgraded = 2;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(DamageBase, ValueProp.Move),
        new CardsVar(DrawBase)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
        DynamicVars.Cards.UpgradeValueBy(DrawUpgraded - DrawBase);
    }
}
