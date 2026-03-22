using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Cards.Ancient;

[Pool(typeof(BuuCardPool))]
public sealed class SuperForm() : BuuCard(2, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    private const int EnergyGain = 3;

    /// <summary>
    /// Ancient cards use full-bleed art on <c>%AncientPortrait</c>; engine aspect is ~606×852 (see ModTemplate CharModCard).
    /// Point at <c>images/card_portraits/big/super_form.png</c> — not the circular 500×380 portrait — to avoid stretch.
    /// </summary>
    public override string PortraitPath => "super_form.png".BigCardImagePath();

    public override string CustomPortraitPath => "super_form.png".BigCardImagePath();

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(18m, ValueProp.Move),
        new CardsVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BuuStanceCmd.EnterSuper(Owner.Creature, choiceContext);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        Owner.PlayerCombatState?.GainEnergy(EnergyGain);
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(6m);
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}
