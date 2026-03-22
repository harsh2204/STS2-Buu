using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Buu.BuuCode.Cards;

[Pool(typeof(BuuCardPool))]
public abstract class BuuCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    private string PortraitFileName => ResolvePortraitFileName();

    // Default: circular/small portrait in card_portraits/. Ancient full-bleed cards (e.g. SuperForm) override both to BigCardImagePath.
    public override string CustomPortraitPath => $"{PortraitFileName}.png".CardImagePath();

    public override string PortraitPath => $"{PortraitFileName}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{PortraitFileName}.png".CardImagePath();

    private string ResolvePortraitFileName()
    {
        var defaultName = GetType().Name.ToSnakeCase();
        return defaultName switch
        {
            "ki_strike" => "ki_blast",
            "ki_barrier" => "bubble_barrier",
            "ki_well" => "regenerate",
            "majin_fury" => "evil_emerges",
            "super_spirit" => "super_aura",
            "regenerative_form" => "reform",
            _ => defaultName
        };
    }
}