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
    private string PortraitFileName => GetType().Name.ToSnakeCase();

    public override string CustomPortraitPath => $"{PortraitFileName}.png".BigCardImagePath();

    public override string PortraitPath => $"{PortraitFileName}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{PortraitFileName}.png".CardImagePath();
}