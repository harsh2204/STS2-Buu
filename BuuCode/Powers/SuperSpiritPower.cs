using BaseLib.Abstracts;
using Buu.BuuCode.Commands;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using Buu.BuuCode.Stances;

namespace Buu.BuuCode.Powers;

public sealed class SuperSpiritPower : BuuPower
{
    private const int KiOnEnterSuper = 2;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "super_stance.png".PowerImagePath();
    public override string CustomBigIconPath => "super_stance.png".BigPowerImagePath();

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
        if (!Owner.Powers.OfType<SuperStance>().Any()) return;

        await PowerCmd.Apply<KiPower>(Owner, KiOnEnterSuper * Amount, Owner, null);
        Flash();
    }
}
