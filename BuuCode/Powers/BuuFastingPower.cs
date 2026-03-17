using BaseLib.Abstracts;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Buu.BuuCode.Powers;

/// <summary>Uses fasting_power.png. At start of your turn gain Strength and Dexterity, lose 1 max draw.</summary>
public sealed class BuuFastingPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "fasting_power.png".PowerImagePath();
    public override string CustomBigIconPath => "fasting_power.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
        await PowerCmd.Apply<DexterityPower>(Owner, Amount, Owner, null);
        if (player.PlayerCombatState != null)
            player.PlayerCombatState.LoseEnergy(1);
        Flash();
    }
}
