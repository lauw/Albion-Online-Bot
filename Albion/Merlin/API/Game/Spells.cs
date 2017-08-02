







using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Merlin.API
{
	public class Spell
	{
		#region Static

		#endregion

		#region Fields

		private LocalPlayerCharacterView _owner;
		private adr _internalSpell;
		private SpellSlotIndex _slot;

		#endregion

		#region Properties and Events

		public SpellSlotIndex SpellSlot => _slot;

		public LocalPlayerCharacterView Owner => _owner;

		public SpellConfiguration Configuration
		{
			get
			{
				if (_internalSpell != null)
					return new SpellConfiguration(_internalSpell.h());

				return default(SpellConfiguration);
			}
		}

		public string Name
		{
			get
			{
				var configuration = Configuration;

				if (configuration != null)
					return configuration.Name;

				return String.Empty;
			}
		}

		public gy.SpellCategory Category
		{
			get
			{
				var configuration = Configuration;

				if (configuration != null)
					return configuration.Category;

				return gy.SpellCategory.None;
			}
		}

		public gy.SpellTarget Target
		{
			get
			{
				var configuration = Configuration;

				if (configuration != null)
					return configuration.Target;

				return gy.SpellTarget.None;
			}
		}

		public int Cost
		{
			get
			{
				var configuration = Configuration;

				if (configuration != null)
					return configuration.Cost;

				return 0;
			}
		}

		public bool IsReady => _owner.IsReadyToCast(_slot);

		#endregion

		#region Constructors and Cleanup
		
		public Spell(LocalPlayerCharacterView owner, adr internalSpell, SpellSlotIndex slot)
		{
			_owner = owner;
			_internalSpell = internalSpell;
			_slot = slot;
		}

		#endregion

		#region Methods

		#endregion
	}

	public class SpellConfiguration
	{
		#region Static

		#endregion

		#region Fields

		private gz _internalConfiguration;

		#endregion

		#region Properties and Events

		public string Name
		{
			get
			{
				if (_internalConfiguration != null)
					return _internalConfiguration.js();

				return String.Empty;
			}
		}

		public gy.SpellCategory Category
		{
			get
			{
				if (_internalConfiguration != null)
					return _internalConfiguration.d4;

				return gy.SpellCategory.None;
			}
		}

		public gy.SpellTarget Target
		{
			get
			{
				if (_internalConfiguration != null)
					return _internalConfiguration.d1;

				return gy.SpellTarget.None;
			}
		}

		public int Cost
		{
			get
			{
				if (_internalConfiguration != null)
					return _internalConfiguration.dv;

				return 0;
			}
		}

		#endregion

		#region Constructors and Cleanup
		
		public SpellConfiguration(gz internalConfiguration)
		{
			_internalConfiguration = internalConfiguration;
		}

		#endregion

		#region Methods

		#endregion
	}

	public static class SpellFilter
	{
		public static IEnumerable<Spell> Slot(this IEnumerable<Spell> spells, SpellSlotIndex spellSlot)
		{
			return spells.Where<Spell>(spell => spell.SpellSlot == spellSlot);
		}

		public static IEnumerable<Spell> Category(this IEnumerable<Spell> spells, gy.SpellCategory category)
		{
			return spells.Where<Spell>(spell => spell.Category == category);
		}

		public static IEnumerable<Spell> Target(this IEnumerable<Spell> spells, gy.SpellTarget target)
		{
			return spells.Where<Spell>(spell => spell.Target == target);
		}

		public static IEnumerable<Spell> Ignore(this IEnumerable<Spell> spells, string name)
		{
			return spells.Where<Spell>(spell => !spell.Name.Contains(name));
		}

		public static IEnumerable<Spell> Ready(this IEnumerable<Spell> spells)
		{
			return spells.Where<Spell>(spell => spell.IsReady);
		}
	}

	public enum SpellSlotIndex : short
	{
		MainHand1,
		MainHand2,
		OffHandOrMainHand3,

		Armor,
		Head,
		Shoes,

		Potion,
		Food,

		Mount,

		EscapeDungeon,
		ChangePvpStance,
		CoupDeGrace,

		NumEntries,
		Invalid = -1
	}
}