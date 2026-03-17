using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class AbsorptionDrain() : BuuCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    private const decimal DamageBase = 6m;
    private const decimal DamageUpgraded = 9m;
    private const decimal KiGainBase = 2m;
    private const decimal KiGainUpgraded = 3m;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(DamageBase, ValueProp.Move),
        new PowerVar<KiPower>((int)KiGainBase)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var amount = DynamicVars["KiPower"].IntValue;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, amount, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, amount, Owner.Creature, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
        DynamicVars["KiPower"].UpgradeValueBy((int)(KiGainUpgraded - KiGainBase));
    }
}
