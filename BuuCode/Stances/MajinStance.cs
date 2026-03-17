using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Stances;

public sealed class MajinStance : BuuStancePower
{
    protected override string? AuraScenePath => null;
    protected override string? StanceVisualScenePath => "res://Buu/animation/buu_node_majin.tscn";

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (dealer == Owner || target == Owner)
            return 2m;
        return 1m;
    }
}
