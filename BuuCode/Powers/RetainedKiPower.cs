using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Powers;

/// <summary>Uses establishment_power.png. Retained cards cost 1 less this combat.</summary>
public sealed class RetainedKiPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "establishment_power.png".PowerImagePath();
    public override string CustomBigIconPath => "establishment_power.png".BigPowerImagePath();

    public override async Task AfterCardRetained(CardModel card)
    {
        card.EnergyCost.AddThisCombat(-Amount);
        await Task.CompletedTask;
    }
}
