using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stateless;

using UnityEngine;

namespace Merlin.Profiles
{
	public class ClusterPathingRequest
	{
		#region Static

		#endregion

		#region Fields

		private bool _useCollider;

		private LocalPlayerCharacterView _player;
		private SimulationObjectView _target;

		private List<Vector3> _path;

		private StateMachine<State, Trigger> _state;

		#endregion

		#region Properties and Events

		public bool IsRunning => _state.State != State.Finish;

		#endregion

		#region Constructors and Cleanup

		public ClusterPathingRequest(LocalPlayerCharacterView player, SimulationObjectView target, List<Vector3> path, bool useCollider = true)
		{
			_player = player;
			_target = target;

			_path = path;

			_useCollider = useCollider;

			_state = new StateMachine<State, Trigger>(State.Start);

			_state.Configure(State.Start)
				.Permit(Trigger.ApprachTarget, State.Running);

			_state.Configure(State.Running)
				.Permit(Trigger.ReachedTarget, State.Finish);
		}

		#endregion

		#region Methods

		public void Continue()
		{
			switch (_state.State)
			{
				case State.Start:
				{
					if (_path.Count > 0)
						_state.Fire(Trigger.ApprachTarget);
					else
						_state.Fire(Trigger.ReachedTarget);

					break;
				}

				case State.Running:
				{
					var currentNode = _path[0];
					var minimumDistance = 3f;

					if (_path.Count < 2 && _useCollider)
					{
						minimumDistance = _target.GetColliderExtents() + _player.GetColliderExtents();

						var directionToPlayer = (_player.transform.position - _target.transform.position).normalized;
						var bufferDistance = directionToPlayer * minimumDistance;

						currentNode = _target.transform.position + bufferDistance;
					}

					var distanceToNode = (_player.transform.position - currentNode).sqrMagnitude;

					if (distanceToNode < minimumDistance)
					{
						_path.RemoveAt(0);
					}
					else
					{
						_player.RequestMove(currentNode);
					}

					if (_path.Count > 0)
						break;

					_state.Fire(Trigger.ReachedTarget);
					break;
				}
			}
		}

		#endregion

		private enum Trigger
		{
			ApprachTarget,
			ReachedTarget,
		}

		private enum State
		{
			Start,
			Running,
			Finish
		}
	}
}
