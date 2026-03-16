using BaseLib.Abstracts;
using BaseLib.Extensions;
using Buu.BuuCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Buu.BuuCode.Powers;

public sealed class KiPower : BuuPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "power.png".PowerImagePath();
    public override string CustomBigIconPath => "power.png".BigPowerImagePath();
}
