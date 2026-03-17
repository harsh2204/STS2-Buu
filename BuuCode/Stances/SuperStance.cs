using Buu.BuuCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Stances;

public sealed class SuperStance : BuuStancePower
{
    protected override string? AuraScenePath => null;
    protected override string? StanceVisualScenePath => "res://Buu/animation/buu_node_super.tscn";

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (dealer == Owner)
            return 3m;
        return 1m;
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        CombatState combatState)
    {
        if (side != Owner.Side) return;
        await BuuStanceCmd.ExitStance(Owner, choiceContext);
        await base.BeforeSideTurnStart(choiceContext, side, combatState);
    }
}
