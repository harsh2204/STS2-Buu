using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class Blasphemer() : BuuCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
{
    private const decimal DamagePerStackBase = 4m;
    private const decimal DamagePerStackUpgraded = 6m;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BlasphemerPower>(1),
        new DynamicVar("BlasphemerDamage", DamagePerStackBase)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BlasphemerPower>(Owner.Creature, DynamicVars["BlasphemerPower"].IntValue, Owner.Creature, this);
        var power = Owner.Creature.Powers.OfType<BlasphemerPower>().FirstOrDefault();
        if (power != null)
        {
            var damageFromCard = (int)DynamicVars["BlasphemerDamage"].IntValue;
            var current = BlasphemerPower.DamagePerStack.Get(power);
            BlasphemerPower.DamagePerStack.Set(power, Math.Max(current, damageFromCard));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlasphemerPower"].UpgradeValueBy(1);
        DynamicVars["BlasphemerDamage"].UpgradeValueBy(DamagePerStackUpgraded - DamagePerStackBase);
    }
}
