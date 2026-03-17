using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Cards.Uncommon;

[Pool(typeof(BuuCardPool))]
public sealed class SuperFocus() : BuuCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private const decimal KiBase = 2m;
    private const decimal KiUpgraded = 3m;
    private const int VigorBase = 5;
    private const int VigorUpgraded = 8;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<KiPower>((int)KiBase),
        new PowerVar<VigorPower>(VigorBase)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var amount = DynamicVars["KiPower"].IntValue;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, amount, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, amount, Owner.Creature, null);
        await PowerCmd.Apply<VigorPower>(Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        await BuuStanceCmd.EnterSuper(Owner.Creature, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["KiPower"].UpgradeValueBy((int)(KiUpgraded - KiBase));
        DynamicVars["VigorPower"].UpgradeValueBy(VigorUpgraded - VigorBase);
    }
}
