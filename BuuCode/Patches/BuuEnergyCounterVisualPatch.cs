using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Buu.BuuCode.Patches;

/// <summary>
/// Mod-packed energy counter scenes do not attach <see cref="NEnergyCounter"/> (cast to <see cref="Control"/> only).
/// Load the vanilla defect counter, then swap orb layers here — same layout as ironclad, avoids BaseLib's stale
/// ironclad clone when <see cref="CustomEnergyCounter"/> would trigger <c>ChangeIroncladEnergy</c>.
/// </summary>
[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter.Create))]
public static class BuuEnergyCounterVisualPatch
{
    private static readonly Color BuuBurstParticleColor = new(1f, 0.0784314f, 0.576471f, 0.6f);

    [HarmonyPostfix]
    public static void ApplyBuuOrbLayers(Player player, ref NEnergyCounter? __result)
    {
        if (__result == null || player.Character is not global::Buu.BuuCode.Character.Buu)
            return;

        try
        {
            static string LayerPath(int layer) =>
                global::Buu.BuuCode.Character.Buu.ResPrefix
                + "images/ui/combat/energy_counters/buu/buu_orb_layer_" + layer + ".png";

            for (var layer = 1; layer <= 5; layer++)
            {
                if (!ResourceLoader.Exists(LayerPath(layer)))
                    return;
            }

            __result.GetNode<TextureRect>("%Layers/Layer1").Texture =
                ResourceLoader.Load<Texture2D>(LayerPath(1));
            __result.GetNode<TextureRect>("%RotationLayers/Layer2").Texture =
                ResourceLoader.Load<Texture2D>(LayerPath(2));
            __result.GetNode<TextureRect>("%RotationLayers/Layer3").Texture =
                ResourceLoader.Load<Texture2D>(LayerPath(3));
            __result.GetNode<TextureRect>("%Layers/Layer4").Texture =
                ResourceLoader.Load<Texture2D>(LayerPath(4));
            __result.GetNode<TextureRect>("%Layers/Layer5").Texture =
                ResourceLoader.Load<Texture2D>(LayerPath(5));

            __result.GetNode<CpuParticles2D>("%BurstBack").Color = BuuBurstParticleColor;
            __result.GetNode<CpuParticles2D>("%BurstFront").Color = BuuBurstParticleColor;
        }
        catch
        {
            // Leave defect visuals if anything fails
        }
    }
}
