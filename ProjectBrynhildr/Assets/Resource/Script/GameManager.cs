using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Player;
using Brynhildr.UI;
using Brynhildr.Enemy;

namespace Brynhildr.Game
{
	public enum ButtonType
	{
		NONE = 0,
		NORMAL_ATTACK = 1,
		SKILL_ONE = 2,
		SKILL_TWO = 3,
		CHANGE_CHAR = 4
	}
	/// <summary>
	/// Handler of the whole gameplay
	/// -Gailbert Huang
	/// </summary>
	public class GameManager : MonoBehaviour 
	{
		[SerializeField] private PlayerHandler player;
		[SerializeField] private EnemyHandler enemies;

		private static GameManager instance;
		private BrynhildrStateMachine stateMachine;

		public static GameManager Instance { get { return instance; } }

		public BrynhildrStateMachine StateMachine
		{
			get { return this.stateMachine; }
		}

		public PlayerHandler Player
		{
			get { return player; }
		}

		public EnemyHandler Enemies
		{
			get { return enemies; }
		}

		/// <summary>
		/// Null Checker Req
		/// </summary>
		/// <value>The game controls.</value>
		public BrynhildrGameControls GameControls
		{
			get { return BrynhildrGameControls.Instance != null ? BrynhildrGameControls.Instance : null; }
		}

		public void LoadingScreenPlay()
		{
			if (BrynhildrGameControls.Instance != null)
				BrynhildrGameControls.Instance.AnimateLoadingScreen ();
		}
			
		#region Monobehavior Function
		void Awake()
		{
			instance = this;
		}

		void Start()
		{
			stateMachine = new BrynhildrStateMachine (this);
		}

		void OnDestroy()
		{
			if (stateMachine != null)
			{
				stateMachine.Destroy ();
				stateMachine = null;
			}
		}

		void Update()
		{
			if (stateMachine != null) 
			{
				stateMachine.Update ();
			}
		}
		#endregion

		public void ExitGame()
		{
			if (StateMachine.GetCurrentState.State == BrynhildrState.INGAME) 
			{
				StateMachine.SwitchState (BrynhildrState.EXIT);
			}
		}

		public ButtonType ConvertToButtonType(int value)
		{
			ButtonType buttType = ButtonType.NONE;
			switch (value) 
			{
				case 1:
					buttType = ButtonType.NORMAL_ATTACK;
					break;
				case 2:
					buttType = ButtonType.SKILL_ONE;
					break;
				case 3:
					buttType = ButtonType.SKILL_TWO;
					break;
				case 4:
					buttType = ButtonType.CHANGE_CHAR;
					break;
				default:
					buttType = ButtonType.NONE;
					break;
			}
			return buttType;
		}
	}
}
