using System.Collections.Generic;
using System;
using System.Reflection;

namespace HearthGem
{
	public abstract class HearthAction
	{
		protected List<Card> cards;
		protected int phase;
		public bool finished;

		public HearthAction()
		{
			cards = new List<Card>();
			phase = 0;
			finished = false;
		}

		public virtual HearthAction AddCard(Card card)
		{
			cards.Add(card);
			return this;
		}

		public abstract void OnAction();
	}

	public class PlayAction: HearthAction
	{
		public override void OnAction()
		{
			switch (phase)
			{
				case 0:
					HearthGem.ClickCard(cards [0]);
					phase = 1;
					break;
				case 1:
					//Reflection.InvokeMethod(InputManager.Get(), "DropHeldCard");
					typeof(InputManager).GetMethod("DropHeldCard",BindingFlags.NonPublic | BindingFlags.Instance,null,new Type[]{typeof(bool)},null)
						.Invoke(InputManager.Get(), new object[]{false});
					phase = -1;
					break;
				case -1:
					finished = true;
					break;
			}
		}
	}

	public class AttackAction: HearthAction
	{
		public override void OnAction()
		{
			switch (phase)
			{
				case 0:
					HearthGem.ClickCard(cards [0]);
					phase = 1;
					break;
				case 1:
					HearthGem.ClickCard(cards [1]);
					phase = -1;
					break;
				case -1:
					finished = true;
					break;
			}
		}
	}
}

