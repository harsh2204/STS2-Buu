using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Powers;

/// <summary>End of turn heal. Lose 1 stack at start of turn.</summary>
public sealed class BuuRegenPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "regeneration_power.png".PowerImagePath();
    public override string CustomBigIconPath => "regeneration_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player?.Creature.Side != side || Amount <= 0) return;
        await CreatureCmd.Heal(Owner, (int)Amount, true);
        Flash();
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner || Amount <= 0) return;
        await PowerCmd.ModifyAmount(this, -1m, null, null);
    }
}
