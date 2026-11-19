using System;

namespace Cards
{
    [Flags]
    public enum CardPersonType
    {
        None = 0,
        Slime = 1 << 0,
        Fires = 1 << 1,
        Gnome = 1 << 2,
        Ogre = 1 << 3,
        Curse = 1 << 4,
        Brothers = 1 << 5
    }
}