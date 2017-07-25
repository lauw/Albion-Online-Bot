using System;
using System.Collections.Generic;
using System.Reflection;
using Merlin;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar;

using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar
{
	public class ClusterPathfinder : AStarPathfinder
	{
		public ClusterPathfinder() : base()
		{
		}

		/// <summary>
		/// See <see cref="BaseGraphSearchPathfinder{TNode,TMap}.OnEnumerateNeighbors"/> for more details.
		/// </summary>
		protected override IEnumerable<Vector2> OnEnumerateNeighbors(AStarNode currentNode, StopFunction<Vector2> stopFunction)
		{
			List<Vector2> result = new List<Vector2>();

			bool enumerateNeightbors = true;

			if (enumerateNeightbors)
				result.AddRange(base.OnEnumerateNeighbors(currentNode, stopFunction));

			return result;
		}
	}
}
