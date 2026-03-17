using System.Linq;
using System.Reflection;
using Buu.BuuCode.Stances;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;

namespace Buu.BuuCode.Patches;

[HarmonyPatch(typeof(LocString), nameof(LocString.GetRawText))]
public static class BuuStanceDisplayNamePatch
{
    [HarmonyPostfix]
    public static void Postfix(LocString __instance, ref string __result)
    {
        try
        {
            var type = __instance.GetType();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var tableField = type.GetField("Table", flags) ?? type.GetField("_table", flags);
            var keyField = type.GetField("Key", flags) ?? type.GetField("_key", flags);
            var table = tableField?.GetValue(__instance) as string
                ?? type.GetProperty("Table", flags)?.GetValue(__instance) as string;
            var key = keyField?.GetValue(__instance) as string
                ?? type.GetProperty("Key", flags)?.GetValue(__instance) as string;

            if (table != "characters" || key != "BUU-BUU.title" || __result != "Buu")
                return;

            var manager = CombatManager.Instance;
            if (manager == null) return;

            var combatType = manager.GetType();
            var combatProp = combatType.GetProperty("CurrentCombat", flags)
                ?? combatType.GetProperty("CombatState", flags)
                ?? combatType.GetProperty("Combat", flags)
                ?? combatType.GetProperty("State", flags);
            var combat = combatProp?.GetValue(manager);
            if (combat == null) return;

            var playersProp = combat.GetType().GetProperty("Players");
            if (playersProp?.GetValue(combat) is not System.Collections.IEnumerable playersEnum)
                return;

            foreach (var p in playersEnum)
            {
                if (p == null) continue;
                var creatureProp = p.GetType().GetProperty("Creature");
                var creature = creatureProp?.GetValue(p) as Creature;
                if (creature == null) continue;

                if (creature.Powers.OfType<MajinStance>().Any())
                {
                    __result = "Majin Buu";
                    return;
                }
                if (creature.Powers.OfType<SuperStance>().Any())
                {
                    __result = "Super Buu";
                    return;
                }
            }
        }
        catch
        {
            // Ignore; name stays default
        }
    }
}
