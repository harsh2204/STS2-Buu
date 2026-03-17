using BaseLib.Abstracts;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Buu.BuuCode.Powers;

/// <summary>Uses duality_power.png. When you change stance, gain block and Ki.</summary>
public sealed class StanceDualityPower : BuuPower
{
    private const int BlockAmount = 2;
    private const int KiAmount = 2;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "duality_power.png".PowerImagePath();
    public override string CustomBigIconPath => "duality_power.png".BigPowerImagePath();

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        BuuStanceCmd.StanceChanged += OnStanceChanged;
    }

    public override async Task AfterRemoved(Creature owner)
    {
        await base.AfterRemoved(owner);
        BuuStanceCmd.StanceChanged -= OnStanceChanged;
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        await base.AfterCombatEnd(room);
        BuuStanceCmd.StanceChanged -= OnStanceChanged;
    }

    private async Task OnStanceChanged(Creature creature, PlayerChoiceContext? context)
    {
        if (creature != Owner) return;
        await CreatureCmd.GainBlock(Owner, BlockAmount * Amount, ValueProp.Unpowered, null);
        await PowerCmd.Apply<KiPower>(Owner, KiAmount * Amount, Owner, null);
        Flash();
    }
}
