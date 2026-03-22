using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Extensions;
using Buu.BuuCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Ancient;

[Pool(typeof(BuuCardPool))]
public sealed class EonCovenant() : BuuCard(2, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override string PortraitPath => "eon_covenant.png".BigCardImagePath();

    public override string CustomPortraitPath => "eon_covenant.png".BigCardImagePath();

    private const decimal BlockBase = 16m;
    private const decimal BlockUpgraded = 22m;
    private const decimal KiBase = 3m;
    private const decimal KiUpgraded = 5m;

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(BlockBase, ValueProp.Move),
        new PowerVar<KiPower>((int)KiBase)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        var amount = DynamicVars["KiPower"].IntValue;
        var kiPower = Owner.Creature.Powers.OfType<KiPower>().FirstOrDefault();
        if (kiPower != null)
            await PowerCmd.ModifyAmount(kiPower, amount, null, null);
        else
            await PowerCmd.Apply<KiPower>(Owner.Creature, amount, Owner.Creature, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(BlockUpgraded - BlockBase);
        DynamicVars["KiPower"].UpgradeValueBy((int)(KiUpgraded - KiBase));
    }
}
