using BaseLib.Abstracts;
using BaseLib.Extensions;
using Buu.BuuCode.Extensions;

namespace Buu.BuuCode.Powers;

public abstract class BuuPower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}