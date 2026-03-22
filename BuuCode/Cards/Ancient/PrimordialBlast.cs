using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Ancient;

[Pool(typeof(BuuCardPool))]
public sealed class PrimordialBlast() : BuuCard(2, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    public override string PortraitPath => "primordial_blast.png".BigCardImagePath();

    public override string CustomPortraitPath => "primordial_blast.png".BigCardImagePath();

    private const decimal DamageBase = 18m;
    private const decimal DamageUpgraded = 26m;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(DamageBase, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(DamageUpgraded - DamageBase);
}
