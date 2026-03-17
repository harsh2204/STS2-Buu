using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Powers;

/// <summary>Uses omega_power.png. At end of turn deal damage to ALL enemies.</summary>
public sealed class OmegaPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "omega_power.png".PowerImagePath();
    public override string CustomBigIconPath => "omega_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Side != side) return;
        var damage = Amount;
        if (damage <= 0) return;
        var enemies = CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive);
        await CreatureCmd.Damage(choiceContext, enemies, damage, ValueProp.Unpowered, Owner);
        Flash();
    }
}
