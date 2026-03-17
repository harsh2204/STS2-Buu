using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using Buu.BuuCode.Stances;

namespace Buu.BuuCode.Powers;

/// <summary>Uses nirvana_power.png. At end of turn in Regular stance, gain block.</summary>
public sealed class CalmAuraPower : BuuPower
{
    private const int BlockAmount = 3;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "nirvana_power.png".PowerImagePath();
    public override string CustomBigIconPath => "nirvana_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player?.Creature.Side != side) return;
        if (!Owner.Powers.OfType<RegularStance>().Any()) return;
        await CreatureCmd.GainBlock(Owner, BlockAmount * Amount, ValueProp.Unpowered, null);
        Flash();
    }
}
