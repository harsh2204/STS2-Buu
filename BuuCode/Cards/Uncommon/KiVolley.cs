using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class KiVolley() : BuuCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    private const decimal DamageBase = 5m;
    private const decimal DamageUpgraded = 7m;

    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(DamageBase, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        int hitCount = ResolveEnergyXValue();
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard(this)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
}
