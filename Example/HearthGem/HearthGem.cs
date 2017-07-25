using System.Reflection;
using UnityEngine;

namespace HearthGem
{
	public class HearthGem : MonoBehaviour
	{
		SceneMgr.Mode lastmode;
		string lastgamestate;

		HearthBot bot;

		void Awake()
		{
			bot = new BotPass();
			lastgamestate = "invalid";
		}

		void OnEnable()
		{
			Reflection.SetStaticField<ApplicationMgr>("s_mode", ApplicationMode.INTERNAL);

			gameObject.AddComponent<SceneDebugger>();
		}

		void OnDisable()
		{
			Reflection.SetStaticField<ApplicationMgr>("s_mode", ApplicationMode.PUBLIC);
  
			Object.Destroy(gameObject.GetComponent<SceneDebugger>());
		}

		void OnGUI()
		{
			GUI.Label(new Rect(5, 5, 100, 30), "HearthGem");
			if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 30), "Unload"))
				HearthGemLoader.UnLoad();

			Reflection.InvokeMethod(SceneDebugger.Get(), "LayoutLeftScreenControls", null);
		}

		bool clickplaygame;

		void Update()
		{
			Reflection.SetField(InactivePlayerKicker.Get(), "m_activityDetected", true);

			SceneMgr.Mode mode = SceneMgr.Get().GetMode();

			if (mode != lastmode)
			{
				if (lastmode != SceneMgr.Mode.GAMEPLAY && mode == SceneMgr.Mode.GAMEPLAY)
					bot.Reset();
				lastmode = mode;
				ZConsole.Get().HandleLog(mode.ToString(), "SceneMode", LogType.Log);
				clickplaygame = false;
			}
			if (mode == SceneMgr.Mode.TOURNAMENT)
			{
				if (SceneMgr.Get().IsInGame() || DeckPickerTrayDisplay.Get() == null || DeckPickerTrayDisplay.Get().GetSelectedDeckID() == 0)
					return;
				if (!clickplaygame)
				{
					Reflection.InvokeMethod(DeckPickerTrayDisplay.Get(), "PlayGame");
					clickplaygame = true;
				}
			}
			if (mode == SceneMgr.Mode.GAMEPLAY)
			{
				GameState state = GameState.Get();
				string gamestate = "invalid";
				if (state != null)
				{
					if (state.IsBlockingServer())
						return;
					if (state.IsMulliganPhase())
						gamestate = "mulligan";
					//else if (state.IsLocalPlayerTurn())
					//	gamestate = "turn";
					else if (state.IsGameOver())
						gamestate = "over";
					else
						gamestate = "invalid";
				}
				if (gamestate != lastgamestate)
				{
					if (gamestate == "turn")
						bot.TurnBegin();
					lastgamestate = gamestate;
				}
				ZConsole.LogLog(gamestate);
				switch (gamestate)
				{
					case "mulligan":
						if (GameState.Get().IsMulliganManagerActive() && MulliganManager.Get() != null &&
						    MulliganManager.Get().GetMulliganButton() != null &&
						    MulliganManager.Get().GetMulliganButton().IsEnabled() &&
						    (bool)Reflection.GetField(MulliganManager.Get(), "m_waitingForUserInput"))
							bot.MulliganUpdate();
						break;
					case "turn":
						if (GameState.Get().IsMainPhase() &&
						    ((GameEntity)Reflection.GetField(GameState.Get(), "m_gameEntity")).GetTag<TAG_STEP>(GAME_TAG.STEP) == TAG_STEP.MAIN_ACTION)
							bot.TurnUpdate();
						break;
					case "over":
						CleanUp();
						break;
					case "invalid":
						//CleanUp();
						break;

				}
			}
		}

		void CleanUp()
		{
			try
			{
				VictoryScreen.Get().ContinueEvents();
			} catch
			{
			}
			try
			{
				VictoryScreen.Get().m_hitbox.TriggerRelease();
			} catch
			{
			}
			try
			{
				EndGameScreen.Get().ContinueEvents();

			} catch
			{
			}
			try
			{
				EndGameScreen.Get().m_hitbox.TriggerRelease();

			} catch
			{
			}
			try
			{
				WelcomeQuests.Get().m_clickCatcher.TriggerRelease();
			} catch
			{

			}
		}

		public static void ClickCard(Card card)
		{
			Reflection.InvokeMethod(InputManager.Get(), "HandleClickOnCard", new object[]{ card.gameObject });
		}
	}
}