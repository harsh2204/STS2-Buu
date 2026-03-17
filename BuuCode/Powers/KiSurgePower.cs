using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Powers;

/// <summary>Uses collect_power.png. At start of turn gain Ki and draw.</summary>
public sealed class KiSurgePower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "collect_power.png".PowerImagePath();
    public override string CustomBigIconPath => "collect_power.png".BigPowerImagePath();

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (Owner.Player != player) return;
        await PowerCmd.Apply<KiPower>(Owner, 1, Owner, null);
        await CardPileCmd.Draw(choiceContext, Amount, player);
        Flash();
    }
}
