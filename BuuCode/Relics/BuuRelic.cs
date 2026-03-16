using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Buu.BuuCode.Character;
using Buu.BuuCode.Extensions;

namespace Buu.BuuCode.Relics;

[Pool(typeof(BuuRelicPool))]
public abstract class BuuRelic : CustomRelicModel
{
    private string IconFileName => GetType().Name.ToSnakeCase();

    public override string PackedIconPath => $"{IconFileName}.png".RelicImagePath();

    protected override string PackedIconOutlinePath => $"{IconFileName}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{IconFileName}.png".BigRelicImagePath();
}