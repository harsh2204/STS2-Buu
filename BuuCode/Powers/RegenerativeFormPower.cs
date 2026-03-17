using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Buu.BuuCode.Stances;

namespace Buu.BuuCode.Powers;

public sealed class RegenerativeFormPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "regeneration_power.png".PowerImagePath();
    public override string CustomBigIconPath => "regeneration_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player?.Creature.Side != side) return;
        if (!Owner.Powers.OfType<RegularStance>().Any()) return;

        await PowerCmd.Apply<KiPower>(Owner, Amount, Owner, null);
        Flash();
    }
}
