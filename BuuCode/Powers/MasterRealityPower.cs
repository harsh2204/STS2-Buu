using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Powers;

/// <summary>Uses master_reality_power.png. At end of turn gain 2 Ki.</summary>
public sealed class MasterRealityPower : BuuPower
{
    private const int KiPerStack = 2;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "master_reality_power.png".PowerImagePath();
    public override string CustomBigIconPath => "master_reality_power.png".BigPowerImagePath();

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player?.Creature.Side != side) return;
        await PowerCmd.Apply<KiPower>(Owner, KiPerStack * Amount, Owner, null);
        Flash();
    }
}
