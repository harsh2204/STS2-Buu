using System.Collections.Generic;
using System.Linq;
using BaseLib.Abstracts;
using Buu.BuuCode.Cards.Ancient;
using Buu.BuuCode.Cards.Uncommon;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace Buu.BuuCode.Character;

public class BuuCardPool : CustomCardPoolModel
{
    public override string Title => Buu.CharacterId; //This is not a display name.

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness
    
    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load Buu/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("ffffff");

    public override string? BigEnergyIconPath => Buu.ResPrefix + "images/ui/combat/buu_energy_icon.png";
    public override string? TextEnergyIconPath => Buu.ResPrefix + "images/ui/combat/text_buu_energy_icon.png";

    public override bool IsColorless => false;

    /// <summary>
    /// Once the ancients timeline (<see cref="DarvEpoch"/>) is revealed, <see cref="SuperForm"/> replaces
    /// <see cref="EvilEmerges"/> in the Buu reward pool (Super Buu fantasy supersedes the baseline Majin gate card).
    /// </summary>
    protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
    {
        List<CardModel> list = cards.ToList();
        if (unlockState.IsEpochRevealed<DarvEpoch>())
            list.RemoveAll(c => c.Id == ModelDb.Card<EvilEmerges>().Id);
        else
            list.RemoveAll(c => c.Id == ModelDb.Card<SuperForm>().Id);
        return list;
    }
}