using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Merlin.API;

namespace Merlin.Profiles.Gatherer
{
	public partial class Gatherer
	{
		#region Static

		#endregion

		#region Fields

		#endregion

		#region Properties and Events

		#endregion

		#region Constructors and Cleanup

		#endregion

		#region Methods

		public bool HandleAttackers()
		{
			if (_localPlayerCharacterView.IsUnderAttack(out FightingObjectView attacker))
			{
				_localPlayerCharacterView.CreateTextEffect("[Attacked]");

				_state.Fire(Trigger.EncounteredAttacker);
				return true;
			}

			return false;
		}

		public void Fight()
		{
			var player = _localPlayerCharacterView;

			if (player.IsMounted)
			{
				player.MountOrDismount();
				return;
			}

			var spells = player.GetSpells().Ready()
								.Ignore("ESCAPE_DUNGEON").Ignore("PLAYER_COUPDEGRACE")
								.Ignore("AMBUSH");

			var attackTarget = player.GetAttackTarget();

			if (attackTarget != null)
			{
				var selfBuffSpells = spells.Target(gs.SpellTarget.Self).Category(gs.SpellCategory.Buff);
				if (selfBuffSpells.Any() && !player.IsCastingSpell())
				{
					player.CreateTextEffect("[Casting Buff Spell]");
					player.CastOnSelf(selfBuffSpells.FirstOrDefault().SpellSlot);
					return;
				}

				var selfDamageSpells = spells.Target(gs.SpellTarget.Self).Category(gs.SpellCategory.Damage);
				if (selfDamageSpells.Any() && !player.IsCastingSpell())
				{
					player.CreateTextEffect("[Casting Damage Spell]");
					player.CastOnSelf(selfDamageSpells.FirstOrDefault().SpellSlot);
					return;
				}

				var groundCCSpells = spells.Target(gs.SpellTarget.Ground).Category(gs.SpellCategory.CrowdControl);
				if (groundCCSpells.Any())
				{
					player.CreateTextEffect("[Casting Ground Spell]");
					player.CastAt(groundCCSpells.FirstOrDefault().SpellSlot, attackTarget.transform.position);
					return;
				}

				// TODO: If buffed, don't use channeled spells.

				/*
				var enemyDamageSpells = spells.Target(gs.SpellTarget.Enemy).Category(gs.SpellCategory.Damage);
				if (enemyDamageSpells.Any() && !player.IsCastingSpell())
				{
					player.CreateTextEffect("[Casting Damage Spell]");
					player.CastOn(enemyDamageSpells.FirstOrDefault().SpellSlot, player.GetAttackTarget());
					return;
				}
				*/

				/*
				var selfDamageSpells = spells.Target(gs.SpellTarget.Self).Category(gs.SpellCategory.Damage);
				if (selfDamageSpells.Any())
				{
				}


				
				*/
			}

			if (player.IsUnderAttack(out FightingObjectView attacker))
			{
				player.SetSelectedObject(attacker);
				player.AttackSelectedObject();
				return;
			}

			if (player.IsCasting())
				return;

			if (player.GetHealth() < (player.GetMaxHealth() * 0.8f))
			{
				var healSpell = spells.Target(gs.SpellTarget.Self).Category(gs.SpellCategory.Heal);

				if (healSpell.Any())
					player.CastOnSelf(healSpell.FirstOrDefault().SpellSlot);

				return;
			}

			_currentTarget = null;
			_harvestPathingRequest = null;

			_state.Fire(Trigger.EliminatedAttacker);

			#endregion
		}
	}
}
