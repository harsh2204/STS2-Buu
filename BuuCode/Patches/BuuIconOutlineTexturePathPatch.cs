using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace Buu.BuuCode.Patches;

/// <summary>
/// BaseLib patches <c>IconTexturePath</c> for <see cref="BaseLib.Abstracts.CustomCharacterModel"/> but not
/// <c>IconOutlineTexturePath</c>. Vanilla resolves <c>character_icon_{id}_outline.png</c> (e.g. buu-buu) under
/// <c>ui/top_panel/</c>, which does not exist — multiplayer UI then errors and can contribute to checksum divergence.
/// </summary>
[HarmonyPatch(typeof(CharacterModel), "IconOutlineTexturePath", MethodType.Getter)]
public static class BuuIconOutlineTexturePathPatch
{
    [HarmonyPrefix]
    public static bool Prefix(CharacterModel __instance, ref string? __result)
    {
        if (__instance is not global::Buu.BuuCode.Character.Buu)
            return true;

        __result = global::Buu.BuuCode.Character.Buu.ResPrefix + "images/charui/character_icon_buu_outline.png";
        return false;
    }
}
