using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Powers;

/// <summary>Uses block_return_power.png. When you gain block, gain 1 Ki (once per stack per trigger).</summary>
public sealed class BlockToKiPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "block_return_power.png".PowerImagePath();
    public override string CustomBigIconPath => "block_return_power.png".BigPowerImagePath();

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (creature != Owner || amount <= 0) return;
        var kiToGain = Amount;
        if (kiToGain <= 0) return;
        await PowerCmd.Apply<KiPower>(Owner, kiToGain, Owner, null);
        Flash();
    }
}
