using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class MajinDraw() : BuuCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MajinDrawPower>(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        await PowerCmd.Apply<MajinDrawPower>(Owner.Creature, DynamicVars["MajinDrawPower"].IntValue, Owner.Creature, this);

    protected override void OnUpgrade() => DynamicVars["MajinDrawPower"].UpgradeValueBy(1);
}
