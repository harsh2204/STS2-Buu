using BaseLib.Config;

namespace Buu.BuuCode.Config;

public enum StarterDeckSelection
{
    Default,
    StanceDance,
    KiEngine,
    AggroMajin,
    BlockReform,
    SuperBurst,
    CommonPile,
    UncommonHeavy,
    RareShowcase,
    BasicsAndCommons,
    StanceOnly,
    FullSpread,
    AllCards
}

public sealed class BuuModConfig : SimpleModConfig
{
    public static StarterDeckSelection StarterDeckSelection { get; set; } = StarterDeckSelection.Default;

    public static bool StartWithAllModRelics { get; set; } = false;
}
