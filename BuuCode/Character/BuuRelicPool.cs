using BaseLib.Abstracts;
using Godot;

namespace Buu.BuuCode.Character;

public class BuuRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Buu.CharacterId;
    public override Color LabOutlineColor => Buu.Color;
}