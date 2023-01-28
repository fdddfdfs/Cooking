using System;

[Flags]
public enum ItemType
{
    Resource = 1,
    Boots = 2,
    Legs = 4,
    Chest = 8,
    Head = 16,
    Turret = 32,
    Trap = 64,
    Unspecified = Resource | Boots | Legs | Chest | Head,
}