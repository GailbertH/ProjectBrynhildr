using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Player;

namespace Brynhildr.Game
{
	/// <summary>
	/// Handler of the whole gameplay
	/// -Gailbert Huang
	/// </summary>
	public class GameManager : MonoBehaviour 
	{
		private GameObject loadingScreen;
		private static GameManager instance;
		private BrynhildrStateMachine stateMachine;
		private PlayerHandler player;

		public static GameManager Instance { get { return instance; } }

		public BrynhildrStateMachine StateMachine
		{
			get { return this.stateMachine; }
		}

		public GameObject LoadingScreen
		{
			get { return this.loadingScreen; }
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
	}
}
