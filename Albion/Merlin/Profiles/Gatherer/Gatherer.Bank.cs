using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Merlin.Pathing.Worldmap;
using UnityEngine;
using WorldMap;

namespace Merlin.Profiles.Gatherer
{
	public partial class Gatherer
	{
		#region Static

		public static int CapacityForBanking = 99;

		#endregion

		#region Fields

		private WorldPathingRequest _worldPathingRequest;

		#endregion

		#region Properties and Events

		#endregion

		#region Constructors and Cleanup

		#endregion

		#region Methods

		public void Bank()
		{
			if (!_localPlayerCharacterView.IsMounted)
			{
				if (_localPlayerCharacterView.IsMounting())
					return;

				_localPlayerCharacterView.MountOrDismount();
				return;
			}

			if (_localPlayerCharacterView.GetLoadPercent() <= CapacityForBanking)
			{
				_localPlayerCharacterView.CreateTextEffect("[Restart]");
				_state.Fire(Trigger.Restart);
				return;
			}

			if (_worldPathingRequest != null)
			{
				if (_worldPathingRequest.IsRunning)
				{
					if (!HandleMounting(Vector3.zero))
						return;

					_worldPathingRequest.Continue();
				}
				else
				{
					_worldPathingRequest = null;
				}

				return;
			}

			var currentCluster = _world.CurrentCluster;
			var townCluster = _world.GetCluster("Fort Sterling");

			var path = new List<WorldmapCluster>();
			var pivotPoints = new List<WorldmapCluster>();

			var worldPathing = new WorldmapPathfinder();

			if (worldPathing.TryFindPath(currentCluster, townCluster, (cluster) => false, out path, out pivotPoints, true, false))
				_worldPathingRequest = new WorldPathingRequest(currentCluster, townCluster, path);


			// TODO: If not in town, get request to exit towards town.

			// TODO: Get current cluster, is it town?

			// TODO: Move to bank

			// TODO: Near the bank, bank is open?

			// TODO: Bank is open, move items
		}

		#endregion
	}
}
