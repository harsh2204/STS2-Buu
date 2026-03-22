using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public sealed class SweetVendorChit() : BuuRelic
{
    private const decimal DiscountPercent = 12m;

    public override RelicRarity Rarity => RelicRarity.Shop;

    public override decimal ModifyMerchantPrice(Player player, MerchantEntry entry, decimal originalPrice)
    {
        if (player != Owner)
            return originalPrice;
        return originalPrice * (1m - DiscountPercent / 100m);
    }
}
