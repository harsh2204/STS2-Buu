using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Buu.BuuCode.Stances;

namespace Buu.BuuCode.Commands;

public static class BuuStanceCmd
{
    public static event Func<Creature, PlayerChoiceContext?, Task>? StanceChanged;

    private static async Task Execute(Creature creature, BuuStancePower? newStance, PlayerChoiceContext? context)
    {
        var currentStance = GetCurrentStance(creature);
        if (!ShouldChangeStance(currentStance, newStance))
            return;

        await ExitCurrentStance(currentStance, creature);
        await EnterNewStance(newStance, creature);

        if (StanceChanged != null)
            await StanceChanged.Invoke(creature, context);
    }

    public static Task EnterRegular(Creature creature, PlayerChoiceContext? context) =>
        Execute(creature, ModelDb.Power<RegularStance>(), context);

    public static Task EnterMajin(Creature creature, PlayerChoiceContext? context) =>
        Execute(creature, ModelDb.Power<MajinStance>(), context);

    public static Task EnterSuper(Creature creature, PlayerChoiceContext? context) =>
        Execute(creature, ModelDb.Power<SuperStance>(), context);

    public static Task ExitStance(Creature creature, PlayerChoiceContext? context) =>
        Execute(creature, null, context);

    private static async Task ExitCurrentStance(BuuStancePower? currentStance, Creature creature)
    {
        if (currentStance == null) return;
        await currentStance.OnExitStance(creature);
        currentStance.RemoveInternal();
    }

    private static BuuStancePower? GetCurrentStance(Creature creature) =>
        creature.Powers.OfType<BuuStancePower>().FirstOrDefault();

    private static bool ShouldChangeStance(BuuStancePower? currentStance, BuuStancePower? newStance)
    {
        if (currentStance == null && newStance == null) return false;
        return currentStance == null || newStance == null ||
               currentStance.GetType() != newStance.GetType();
    }

    private static async Task EnterNewStance(BuuStancePower? newStance, Creature creature)
    {
        if (newStance == null) return;
        var mutableStance = newStance.ToMutable();
        mutableStance.ApplyInternal(creature, 1);
        await ((BuuStancePower)mutableStance).OnEnterStance(creature);
    }
}
