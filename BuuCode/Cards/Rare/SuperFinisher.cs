using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class SuperFinisher() : BuuCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const decimal KiCost = 2m;
    private const decimal DamageBase = 32m;
    private const decimal DamageUpgraded = 44m;

    private decimal DamageAmount => IsUpgraded ? DamageUpgraded : DamageBase;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(DamageBase, ValueProp.Move)];

    protected override bool IsPlayable =>
        Owner?.Creature != null && Owner.Creature.GetPowerAmount<KiPower>() >= KiCost;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        var kiPower = creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, -KiCost, null, null);

        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DamageAmount).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
    }
}
