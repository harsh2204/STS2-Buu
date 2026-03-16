using BaseLib.Abstracts;
using Godot;

namespace Buu.BuuCode.Character;

public class BuuPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Buu.CharacterId;
    public override Color LabOutlineColor => Buu.Color;
}