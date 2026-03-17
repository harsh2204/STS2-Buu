using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Powers;

/// <summary>Uses wave_of_the_hand_power.png. At end of turn deal damage to a random enemy.</summary>
public sealed class PinkWavePower : BuuPower
{
    private const int DamagePerStack = 3;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "wave_of_the_hand_power.png".PowerImagePath();
    public override string CustomBigIconPath => "wave_of_the_hand_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player?.Creature.Side != side) return;
        var damage = DamagePerStack * Amount;
        if (damage <= 0) return;
        var enemies = CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive).ToList();
        if (enemies.Count == 0) return;
        var target = enemies[Random.Shared.Next(enemies.Count)];
        await CreatureCmd.Damage(choiceContext, [target], damage, ValueProp.Unpowered, Owner);
        Flash();
    }
}
