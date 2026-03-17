using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Cards.Rare;

[Pool(typeof(BuuCardPool))]
public sealed class FullAbsorption() : BuuCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private const decimal KiBase = 6m;
    private const decimal KiUpgraded = 9m;
    private const int DrawBase = 2;
    private const int DrawUpgraded = 3;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ki = IsUpgraded ? KiUpgraded : KiBase;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, ki, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, ki, Owner.Creature, null);
        await CardPileCmd.Draw(choiceContext, IsUpgraded ? DrawUpgraded : DrawBase, Owner);
    }

    protected override void OnUpgrade() { }
}
