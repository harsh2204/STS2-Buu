using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Buu.BuuCode.Powers;

/// <summary>Uses foresight_power.png. At start of turn draw extra cards.</summary>
public sealed class BuuForesightPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "foresight_power.png".PowerImagePath();
    public override string CustomBigIconPath => "foresight_power.png".BigPowerImagePath();

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (Owner.Player != player) return;
        await CardPileCmd.Draw(choiceContext, Amount, player);
        Flash();
    }
}
