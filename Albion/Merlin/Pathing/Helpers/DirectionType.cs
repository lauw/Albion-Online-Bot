using System;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    /// <summary>
    /// Direction
    /// </summary>
    [Flags]
    public enum DirectionType
    {
        NorthEast = North | East, // 3
        SouthEast = South | East, // 6
        NorthWest = North | West, // 9
        SouthWest = South | West, // 12

        None = 0x0000,

        North = 0x0001,
        East  = 0x0002,
        South = 0x0004,
        West  = 0x0008,
    }
}
