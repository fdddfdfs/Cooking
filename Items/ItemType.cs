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
    Defence,
    Seed = 128,
    Unspecified = 256 - 1,
}