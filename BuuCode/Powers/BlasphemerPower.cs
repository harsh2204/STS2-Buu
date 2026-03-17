using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Powers;

/// <summary>Uses blasphemer_power.png. At start of turn deal damage to a random enemy.</summary>
public sealed class BlasphemerPower : BuuPower
{
    private const int DamagePerStackBase = 4;
    private const int DamagePerStackUpgraded = 6;

    public static readonly SpireField<BlasphemerPower, int> DamagePerStack = new(() => DamagePerStackBase);

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "blasphemer_power.png".PowerImagePath();
    public override string CustomBigIconPath => "blasphemer_power.png".BigPowerImagePath();

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (Owner.Player != player) return;
        var dmgPerStack = DamagePerStack.Get(this);
        var damage = dmgPerStack * Amount;
        if (damage <= 0) return;
        var enemies = CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive).ToList();
        if (enemies.Count == 0) return;
        var target = enemies[Random.Shared.Next(enemies.Count)];
        await CreatureCmd.Damage(choiceContext, [target], damage, ValueProp.Unpowered, Owner);
        Flash();
    }
}
