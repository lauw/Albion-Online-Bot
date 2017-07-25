using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Merlin.API
{
	/* Internal Type: a3d */
	public class Client
	{
		#region Static

		public static Client Instance
		{
			get
			{
				var internalClient = a3d.s();

				if (internalClient != null)
					return new Client(internalClient);

				return default(Client);
			}
		} 

		#endregion

		#region Fields

		private readonly a3d _client;

		private readonly World _world;
		private readonly Collision _collision;

		#endregion

		#region Properties and Events

		public GameState State => (GameState)_client.w();

		public LocalPlayerCharacterView LocalPlayerCharacter => _client.v();

		public Collision Collision => _collision;

		public Cluster CurrentCluster => new Cluster(_world.CurrentCluster.Info);

		#endregion

		#region Constructors and Cleanup

		protected Client(a3d client)
		{
			_client = client;

			_world = World.Instance;
			_collision = Collision.Instance;
		}

		#endregion

		#region Methods

		public SimulationObjectView GetEntity(apd entity) => _client.a(entity);

		public SimulationObjectView GetEntity(long id)
		{
			if (id > 0L)
				return _client.a(id);

			return default(SimulationObjectView);
		}

		/// <summary>
		/// Gets the collection of entities of the specified.
		/// </summary>
		public List<T> GetEntities<T>(Func<T, bool> selector) where T : SimulationObjectView
		{
			var list = new List<T>();

			foreach (var entity in _world.GetEntities().Values)
			{
				if (GetEntity(entity) is T t && selector(t))
					list.Add(t);
			}

			return list;
		}

		#endregion
	}

	public enum GameState
	{
		Unknown,
		LoggingIn,
		Loading,
		Playing
	}
}