using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Powers;

/// <summary>Uses mark_power.png. At start of turn apply 1 Weak to a random enemy.</summary>
public sealed class MarkPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "mark_power.png".PowerImagePath();
    public override string CustomBigIconPath => "mark_power.png".BigPowerImagePath();

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (Owner.Player != player) return;
        var enemies = CombatState.GetOpponentsOf(Owner).Where(c => c.IsAlive).ToList();
        if (enemies.Count == 0) return;
        var target = enemies[Random.Shared.Next(enemies.Count)];
        await PowerCmd.Apply<WeakPower>(target, Amount, Owner, null);
        Flash();
    }
}
