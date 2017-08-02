using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using UnityEngine;

namespace Merlin.Profiles.Gatherer
{
	public partial class Gatherer
	{
		#region Static

		#endregion

		#region Fields

		private SimulationObjectView _currentTarget;

		#endregion

		#region Properties and Events

		#endregion

		#region Constructors and Cleanup

		#endregion

		#region Methods

		public void Search()
		{
		
            if (_localPlayerCharacterView.IsUnderAttack(out FightingObjectView attacker))
			{
			    Core.Log("[Attacked]");
				_state.Fire(Trigger.EncounteredAttacker);
				return;
			}

			if (_localPlayerCharacterView.GetLoadPercent() > CapacityForBanking)
			{
                Core.Log("Overweight");
				_state.Fire(Trigger.Overweight);
				return;
			}

			if (_currentTarget != null)
			{
			    Core.Log("[Blacklisting target]");

                Blacklist(_currentTarget, TimeSpan.FromMinutes(0.5f));

				_currentTarget = null;
				_harvestPathingRequest = null;

				return;
			}

			if (IdentifiedTarget(out SimulationObjectView target))
			{
			    Core.Log("[Checking Target]");
				_currentTarget = target;
			}

			if (_currentTarget != null && ValidateTarget(_currentTarget))
			{
			    Core.Log("[Identified]");
				_state.Fire(Trigger.DiscoveredResource);
				return;
			}
        }

        public bool IdentifiedTarget(out SimulationObjectView target)
		{
			var resources = _client.GetEntities<HarvestableObjectView>(ValidateHarvestable);
			var hostiles = _client.GetEntities<MobView>(ValidateMob);

			var views = new List<SimulationObjectView>();

		    foreach (var r in resources)
		    {
		        views.Add(r);
		    }
			//foreach (var h in hostiles) views.Add(h);

			target = views.OrderBy((view) =>
			{
				var playerPosition = _localPlayerCharacterView.transform.position;
				var resourcePosition = view.transform.position;

				var score = (resourcePosition - playerPosition).sqrMagnitude;
                
                if (view is HarvestableObjectView harvestable)
				{
                    var rareState = harvestable.GetRareState();
				
					if (harvestable.GetTier() >= 3) score /= 2;
					if (harvestable.GetCurrentCharges() == harvestable.GetMaxCharges()) score /= 2;
					if (rareState > 0) score /= rareState;
				}
				else if (view is MobView mob)
				{
				}

				var yDelta = Math.Abs(_landscape.GetLandscapeHeight(playerPosition.c()) - _landscape.GetLandscapeHeight(resourcePosition.c()));

				score += (yDelta * 10f);

				return (int)score;
			}).FirstOrDefault();

            if (target != null)
		        Core.Log($"Resource spotted: {target.name}");

            return (target != default(SimulationObjectView));
		}

		public bool IsBlocked(Vector2 location)
		{
			var vector = new Vector3(location.x, 0, location.y);

			if (_currentTarget != null)
			{
				var resourcePosition = new Vector2( _currentTarget.transform.position.x,
													_currentTarget.transform.position.z);
				var distance = (resourcePosition - location).magnitude;

				if (distance < (_currentTarget.GetColliderExtents() + _localPlayerCharacterView.GetColliderExtents()))
					return false;
			}

			if (_localPlayerCharacterView != null)
			{
				var playerLocation = new Vector2(   _localPlayerCharacterView.transform.position.x,
													_localPlayerCharacterView.transform.position.z);
				var distance = (playerLocation - location).magnitude;

				if (distance < 2f)
					return false;
			}

			return (_client.Collision.GetFlag(vector, 1.0f) > 0);
		}

		#endregion
	}
}
