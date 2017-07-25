// This bot auto passes :D
using UnityEngine;
using System.Collections.Generic;
using System;

namespace HearthGem
{	
	public class BotPass : HearthBot
	{
		string mulliganPhase;
		string turnPhase;
		Queue<HearthAction> actionQueue;

		DateTime lastAction;
		DateTime turnbeginTime;

		bool canHeropower;

		public BotPass()
		{
			actionQueue = new Queue<HearthAction>();
		}

		public override void Reset()
		{
			mulliganPhase = "begin";
			turnPhase = "end";
		}

		public override void MulliganUpdate()
		{
			switch (mulliganPhase)
			{
				case "begin":
					Card[] cards = GameState.Get().GetPlayer(0).GetHandZone().GetCards().ToArray();
					foreach (Card card in cards)
					{
						if (card.GetEntity().GetCardId() == "GAME_005")
							continue;
						ZConsole.LogLog(card.GetEntity().GetName());
						if (card.GetEntity().GetCost() > 3)
						{
							HearthGem.ClickCard(card);
						}
					}
					mulliganPhase = "click";
					break;
				case "click":
					MulliganManager.Get().GetMulliganButton().TriggerRelease();
					mulliganPhase = "end";
					break;
				case "end":
					break;
			}

		}

		public override void TurnBegin()
		{
			canHeropower = true;
			turnPhase = "begin";
			turnbeginTime = DateTime.Now;
		}

		public override void TurnUpdate()
		{
			if ((DateTime.Now - turnbeginTime).TotalMilliseconds < 5000)
				return;

			//ZConsole.LogLog(turnPhase);
			//ZConsole.LogLog(actionQueue.Count.ToString());
			switch (turnPhase)
			{
				case "begin":
					TurnAI();
					break;
				case "action":
					TurnAction();
					break;
				case "end":
					InputManager.Get().DoEndTurnButton();
					return;
			}
			if (EndTurnButton.Get().HasNoMorePlays())
				turnPhase = "end";
		}

		string[] priorityKill = {"John", "Paul", "Mary"};
		string[] priorityPlay = {"John", "Paul", "Mary"};

		void TurnAI()
		{
			Player player = GameState.Get().GetPlayer(0);
			Player enemy = GameState.Get().GetPlayer(1);

			Card playerHero = player.GetHeroCard();
			Card enemyHero = enemy.GetHeroCard();
			int mana = player.GetNumAvailableResources();
		
			List<Card> cards = player.GetHandZone().GetCards();
			List<Card> playerMinions = player.GetBattlefieldZone().GetCards();
			List<Card> enemyMinions = enemy.GetBattlefieldZone().GetCards();
			Card heropower = player.GetHeroPowerCard();

			Card coin = null;
			foreach (Card card in cards)
			{
				if (card.GetEntity().GetCardId() == "GAME_005")
				{
					coin = card;
					break;
				}
			}
			int minionsCount = playerMinions.Count;

			cards.Sort((a,b) => b.GetEntity().GetCost().CompareTo(a.GetEntity().GetCost()));

			foreach (Card card in cards)
			{
				if (card.GetEntity().GetCardId() == "GAME_005")
					continue;
				//if (card.GetEntity().IsSpell())
				//	continue;

				int cost = card.GetEntity().GetCost();

				int extra = 0;
				if (coin != null)
					extra = 1;

				if(cost > mana + extra)
					continue;

				if (card.GetEntity().IsMinion())
				{
					if (minionsCount >= 7)
						continue;
					minionsCount += 1;
				}
					
				if (cost > mana)
				{
					ZConsole.LogLog(coin.GetEntity().GetName());
					actionQueue.Enqueue(new PlayAction().AddCard(coin));
					mana += 1;
				}

				ZConsole.LogLog(card.GetEntity().GetName());
				actionQueue.Enqueue(new PlayAction().AddCard(card));

				mana -= cost;
			}
			if (mana >= 2 && canHeropower)
			{
				canHeropower = false;
				switch ( player.GetHeroCard().GetEntity().GetClass())
				{
					case TAG_CLASS.WARLOCK:
						if (cards.Count <= 8)
						{
							if (playerHero.GetEntity().GetRemainingHP() >= 12)
							{
								actionQueue.Enqueue(new PlayAction().AddCard(heropower));
							}
						}
						break;
					case TAG_CLASS.SHAMAN:
					case TAG_CLASS.PALADIN:
						if (minionsCount < 7)
							actionQueue.Enqueue(new PlayAction().AddCard(heropower));
						break;
					case TAG_CLASS.HUNTER:
					case TAG_CLASS.DRUID:
					case TAG_CLASS.ROGUE:
					case TAG_CLASS.WARRIOR:
						actionQueue.Enqueue(new PlayAction().AddCard(heropower));
						break;
					case TAG_CLASS.PRIEST:
						actionQueue.Enqueue(new AttackAction().AddCard(heropower).AddCard(playerHero));
						break;
					case TAG_CLASS.MAGE:
						actionQueue.Enqueue(new AttackAction().AddCard(heropower).AddCard(enemyHero));
						break;
					default:
						break;
				}
			}

			List<Card> enemyTaunts = new List<Card>();
			foreach (Card card in enemyMinions)
			{
				if (card.GetEntity().IsMinion() && card.GetEntity().HasTaunt())
				{
					enemyTaunts.Add(card);
				}
			}

			if (actionQueue.Count < 1)
			{
				foreach (Card card in playerMinions)
				{
					if (card.GetEntity().CanAttack() && !card.GetEntity().IsExhausted() && !card.GetEntity().IsFrozen() && !card.GetEntity().IsAsleep() && card.GetEntity().GetATK() > 0)
					{
						if (enemyTaunts.Count > 0)
						{
							actionQueue.Enqueue(new AttackAction().AddCard(card).AddCard(enemyTaunts[0]));
						} else 
						{
							actionQueue.Enqueue(new AttackAction().AddCard(card).AddCard(enemyHero));
						}
						break;
					}
				}
			}

			if (actionQueue.Count < 1)
			{
				if (playerHero.GetEntity().CanAttack() && !playerHero.GetEntity().IsExhausted() && !playerHero.GetEntity().IsFrozen() && playerHero.GetEntity().GetATK() > 0)
				{
					ZConsole.LogLog("Hero Atk");
					if (enemyTaunts.Count > 0)
					{
						actionQueue.Enqueue(new AttackAction().AddCard(playerHero).AddCard(enemyTaunts[0]));
					} else
					{
						actionQueue.Enqueue(new AttackAction().AddCard(playerHero).AddCard(enemyHero));
					}
				}
			}

			if (actionQueue.Count > 0)
			{
				turnPhase = "action";
				lastAction = DateTime.Now;
			} else
			{
				turnPhase = "end";
			}
		}

		void TurnAction()
		{
			if (actionQueue.Count < 1)
			{
				turnPhase = "begin";
				return;
			}

			if ((DateTime.Now - lastAction).TotalMilliseconds < 500)
				return;

			HearthAction action = actionQueue.Peek();
			if (!action.finished)
			{
				lastAction = DateTime.Now;
				action.OnAction();
			} else
			{
				actionQueue.Dequeue();
			}
		}
	}
}