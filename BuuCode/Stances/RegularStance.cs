using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Buu.BuuCode.Stances;

public sealed class RegularStance : BuuStancePower
{
    protected override string? AuraScenePath => null;
    protected override string? StanceVisualScenePath => "res://Buu/animation/buu_node.tscn";
}
