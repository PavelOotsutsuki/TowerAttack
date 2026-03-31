using System;

namespace Cards.Views
{
    [Flags]
    public enum CardCapability
    {
        Attack = 1 << 0,
        Play = 1 << 1,
        Search = 1 << 2,
        Curse = 1 << 3,
        GnomeForging = 1 << 4,
        GnomeChoice = 1 << 5,
        Variants = 1 << 6,
        BrothersBonds = 1 << 7,
        HandTransfer = 1 << 8
    }
}