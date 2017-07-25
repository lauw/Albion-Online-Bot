using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    /// <summary>
    /// This function determines whether there is a obstacle (true) on a given point, or not.
    /// </summary>
    public delegate Boolean StopFunction<in TValue>(TValue location);
}
