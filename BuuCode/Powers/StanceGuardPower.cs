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

/// <summary>Uses mental_fortress_power.png. When you change stance, gain block.</summary>
public sealed class StanceGuardPower : BuuPower
{
    private const int BlockPerChange = 4;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "mental_fortress_power.png".PowerImagePath();
    public override string CustomBigIconPath => "mental_fortress_power.png".BigPowerImagePath();

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
        await CreatureCmd.GainBlock(Owner, BlockPerChange * Amount, ValueProp.Unpowered, null);
        Flash();
    }
}
