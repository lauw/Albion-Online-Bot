







using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using Merlin.API;

namespace Merlin
{
	public static class FightingObjectViewExtensions
	{
		private static MethodInfo _getEventHandler;

		static FightingObjectViewExtensions()
		{
			_getEventHandler = typeof(ar9).GetMethod("xm");
		}

		public static FightingObjectView GetAttackTarget(this FightingObjectView instance)
		{
			var attackTargetId = instance.FightingObject.v8();

			if (attackTargetId > 0L)
			{
				var client = Client.Instance;

				if (client.GetEntity(attackTargetId) is FightingObjectView target)
					return target;
			}

			return default(FightingObjectView);
		}

		public static string GetDisplayName(this FightingObjectView instance)
		{
			return instance.FightingObject.ir();
		}

		public static T GetEventHandler<T>(this FightingObjectView instance) where T : asa
		{
			return _getEventHandler.MakeGenericMethod(new Type[] { typeof(T) })
									.Invoke(instance.FightingObject, new object[0]) as T;
		}

		public static bool IsReadyToCast(this FightingObjectView instance, SpellSlotIndex slot)
		{
			var eventHandler = new CastSpellEventHandler(instance.GetEventHandler<auy>());

			if (eventHandler != null && eventHandler.IsReady((byte)slot))
				return true;

			return false;
		}

		public static float GetLoad(this LocalPlayerCharacterView instance)
		{
			return instance.LocalPlayerCharacter.t8();
		}
	}
}