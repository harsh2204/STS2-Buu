using BaseLib.Abstracts;
using BaseLib.Utils;
using Buu.BuuCode.Character;

namespace Buu.BuuCode.Potions;

[Pool(typeof(BuuPotionPool))]
public abstract class BuuPotion : CustomPotionModel;