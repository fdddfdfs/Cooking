using System;

[Flags]
public enum ItemType
{
    None = 0,
    Resource = 1,
    Boots = 2,
    Legs = 4,
    Chest = 8,
    Head = 16,
    Turret = 32,
    Trap = 64,
    Unspecified = 128 - 1,
}