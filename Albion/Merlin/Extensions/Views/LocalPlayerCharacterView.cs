using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar;

using Merlin.API;

namespace Merlin
{
	public static class LocalPlayerCharacterViewExtensions
	{
		private static MethodInfo _startCastInternalTarget;
		private static MethodInfo _startCastInternalPosition;
		private static MethodInfo _doActionStaticObjectInteraction;

		static LocalPlayerCharacterViewExtensions()
		{
			var inputHandlerType = typeof(LocalInputHandler);

			_startCastInternalTarget = inputHandlerType.GetMethod("StartCastInternal", BindingFlags.NonPublic | BindingFlags.Instance, 
											Type.DefaultBinder, new Type[] { typeof(byte), typeof(FightingObjectView) }, null);

			_startCastInternalPosition = inputHandlerType.GetMethod("StartCastInternal", BindingFlags.NonPublic | BindingFlags.Instance, 
											Type.DefaultBinder, new Type[] { typeof(byte), typeof(ahl) }, null);

			_doActionStaticObjectInteraction = inputHandlerType.GetMethod("DoActionStaticObjectInteraction", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static bool TryFindPath(this LocalPlayerCharacterView instance, AStarPathfinder pathfinder, SimulationObjectView target,
									StopFunction<Vector2> stopFunction, out List<Vector3> results)
		{
			results = new List<Vector3>();

			if (instance.TryFindPath(pathfinder, target.transform.position, stopFunction, out results))
			{
				/*
				var collider = target.GetComponent<Collider>();

				if (collider != null)
				{
					while (collider.bounds.Contains(results.Last()))
						results.RemoveAt(results.Count - 1);

					var lastNode = results.Last();
					var closestNode = collider.ClosestPointOnBounds(lastNode);
					var direction = (closestNode - lastNode).normalized / 2;

					results.Add(closestNode - direction);
				}

				results.Insert(0, instance.transform.position);
				*/
			}
			else
			{
				return false;
			}

			return true;
		}

		public static bool TryFindPath(this LocalPlayerCharacterView instance, AStarPathfinder pathfinder, Vector3 target,
									StopFunction<Vector2> stopFunction, out List<Vector3> results)
		{
			results = new List<Vector3>();

		    var pivotPoints = new List<Vector2>();
            var path = new List<Vector2>();
			
			var startLocation = new Vector2((int)instance.transform.position.x, (int)instance.transform.position.z);
			var endLocation = new Vector2((int)target.x, (int)target.z);

			var landscape = Landscape.Instance;

			if (pathfinder.TryFindPath(startLocation, endLocation, stopFunction, out path, out pivotPoints, true))
			{
				foreach (var point in path)
					results.Add(new Vector3(point.x, landscape.GetLandscapeHeight(point.b()) + 0.5f, point.y));
			}
			else
			{
				return false;
			}

			return true;
		}

		public static bool RequestMove(this LocalPlayerCharacterView instance, Vector3 position)
		{
			return instance.RequestMove(position.c());
		}

		public static bool IsUnderAttack(this LocalPlayerCharacterView instance, out FightingObjectView attacker)
		{
			var entities = Client.Instance.GetEntities<MobView>((entity) =>
			{
				var target = entity.GetAttackTarget();

				if (target != null && target.Equals(instance))
					return true;

				return false;
			});

			attacker = entities.FirstOrDefault();

			return (attacker != default(FightingObjectView));
		}

		public static void SetSelectedObject(this LocalPlayerCharacterView instance, SimulationObjectView simulation)
		{
			if (simulation == default(SimulationObjectView))
				instance.InputHandler.SetSelectedObjectId(-1L);
			else
				instance.InputHandler.SetSelectedObjectId(simulation.Id);
		}

		public static void AttackSelectedObject(this LocalPlayerCharacterView instance)
		{
			instance.InputHandler.AttackCurrentTarget();
		}

		public static Spell[] GetSpells(this LocalPlayerCharacterView instance)
		{
			var internalSpells = instance.LocalPlayerCharacter.th();
			var spells = new Spell[internalSpells.Length];

			for (int i = 0; i < spells.Length; i++)
			{
				if (internalSpells[i] != null)
					spells[i] = new Spell(instance, internalSpells[i], (SpellSlotIndex)i);
			}

			return spells;
		}

		public static void CastOnSelf(this LocalPlayerCharacterView instance, SpellSlotIndex slot)
		{
			instance.CastOn(slot, instance);
		}

		public static void CastOn(this LocalPlayerCharacterView instance, SpellSlotIndex slot, FightingObjectView target)
		{
			_startCastInternalTarget.Invoke(instance.InputHandler, new object[] { (byte)slot, target });
		}

		public static void CastAt(this LocalPlayerCharacterView instance, SpellSlotIndex slot, Vector3 target)
		{
			_startCastInternalPosition.Invoke(instance.InputHandler, new object[] { (byte)slot, target.c() });
		}

		public static void Interact(this LocalPlayerCharacterView instance, WorldObjectView target)
		{
			_doActionStaticObjectInteraction.Invoke(instance.InputHandler, new object[] { target, String.Empty });
		}

		public static float GetHealth(this LocalPlayerCharacterView instance)
		{
			return instance.LocalPlayerCharacter.v5().r();
		}

		public static float GetMaxHealth(this LocalPlayerCharacterView instance)
		{
			return instance.LocalPlayerCharacter.v5().l();
		}

		public static bool IsMounting(this LocalPlayerCharacterView instance)
		{
			if (instance.LocalPlayerCharacter.v9().f() != 21)
				return false;

			return true;
		}

		public static bool IsCastingSpell(this LocalPlayerCharacterView instance)
		{
			if (instance.LocalPlayerCharacter.v9().f() != 4)
				return false;

			return true;
		}

		public static bool GetMount(this LocalPlayerCharacterView instance, out MountObjectView mount)
		{
			var mounts = Client.Instance.GetEntities<MountObjectView>((m) =>
			{
				return m.MountObject.se();
			});

			mount = mounts.FirstOrDefault();

			return (mount != default(MountObjectView));
		}

		public static float GetMaxLoad(this LocalPlayerCharacterView instance)
		{
			return instance.LocalPlayerCharacter.wf();
		}

		public static float GetLoadPercent(this LocalPlayerCharacterView instance)
		{
			return instance.GetLoad() / instance.GetMaxLoad() * 100.0f * 2f;
		}
	}
}