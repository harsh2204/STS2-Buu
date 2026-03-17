using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Powers;

/// <summary>Uses deva_power.png. At end of round gain 1 Energy next turn (stacking).</summary>
public sealed class BuuDevaPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "deva_power.png".PowerImagePath();
    public override string CustomBigIconPath => "deva_power.png".BigPowerImagePath();

    public override async Task AfterEnergyReset(Player player)
    {
        if (player.Creature != Owner) return;
        await PlayerCmd.GainEnergy(Amount, player);
        Flash();
    }
}
