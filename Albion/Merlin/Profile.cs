using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using UnityEngine;

using Merlin.API;

namespace Merlin
{
	public abstract class Profile : MonoBehaviour
	{
		#region Static

		public static TimeSpan UpdateDelay = TimeSpan.FromMilliseconds(100);

		#endregion

		#region Fields

		protected Client _client;
		protected World _world;
		protected Landscape _landscape;
		protected LocalPlayerCharacterView _localPlayerCharacterView;

		private DateTime _nextUpdate;

		#endregion

		#region Properties and Events

		public abstract string Name { get; }

		#endregion

		#region Constructors and Cleanup

		#endregion

		#region Methods

		/// <summary>
		/// Called when this instance is enabled.
		/// </summary>
		void OnEnable()
		{
			_client = Client.Instance;
			_world = World.Instance;
			_landscape = Landscape.Instance;
			_localPlayerCharacterView = _client.LocalPlayerCharacter;

			_nextUpdate = DateTime.Now;

			OnStart();
		}

		/// <summary>
		/// Called when this instance is disabled.
		/// </summary>
		void OnDisable()
		{
			OnStop();

			_client = null;
		}

		/// <summary>
		/// Called when this instance is updated.
		/// </summary>
		void Update()
		{
			if (DateTime.Now < _nextUpdate)
				return;

			OnUpdate();

			_nextUpdate = DateTime.Now + UpdateDelay;
		}

		/// <summary>
		/// Called when the GUI is rendered.
		/// </summary>
		void OnGUI()
		{
		}


		/// <summary>
		/// Called when this instance is started.
		/// </summary>
		protected virtual void OnStart()
		{
		}

		/// <summary>
		/// Called when this instance is stopped.
		/// </summary>
		protected virtual void OnStop()
		{
		}


		protected virtual void OnUpdate()
		{
		}

		#endregion
	}

	public class RestartInstruction : CustomYieldInstruction
	{
		public override bool keepWaiting => false;
	}
}
