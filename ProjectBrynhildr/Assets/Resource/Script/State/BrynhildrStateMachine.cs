using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Game
{
	/// <summary>
	/// State machine of the game it controls the other state its their main source of action
	/// -Gailbert Huang
	/// </summary>
	public class BrynhildrStateMachine
	{
		public delegate void OnStateSwitch(BrynhildrState nextState);
		public event OnStateSwitch OnStatePreSwitchEvent = null;

		private Dictionary<BrynhildrState, BrynhildrState_Base<BrynhildrState>> states = new Dictionary<BrynhildrState, BrynhildrState_Base<BrynhildrState>>();
		private BrynhildrState_Base<BrynhildrState> currentState = null;
		private List<BrynhildrState> prevGameState;

		public BrynhildrStateMachine (GameManager manager)
		{
			states = new Dictionary<BrynhildrState, BrynhildrState_Base<BrynhildrState>> ();

			Brynhildr_Loading loading = new Brynhildr_Loading (manager);
			Brynhildr_InGame inGame = new Brynhildr_InGame (manager);
			Brynhildr_Exit exit = new Brynhildr_Exit (manager);

			states.Add (loading.State, (BrynhildrState_Base<BrynhildrState>)loading);
			states.Add (inGame.State, (BrynhildrState_Base<BrynhildrState>)inGame);
			states.Add (exit.State, (BrynhildrState_Base<BrynhildrState>)exit);

			currentState = loading;
			currentState.Start ();

			prevGameState = new List<BrynhildrState> ();
			prevGameState.Add (currentState.State);
		}

		public void Update ()
		{
			if (currentState != null)
				currentState.Update ();				
		}

		public void Destroy ()
		{
			if (states != null)
			{
				foreach (BrynhildrState key in states.Keys)
				{
					states [key].Destroy ();
				}
				states.Clear ();
				states = null;
			}
		}

		public BrynhildrState_Base<BrynhildrState> GetCurrentState
		{
			get { return currentState; }
		}

		public string GetPreviousStateList ()
		{
			string prevStates = "PREVIOUS STATES: ";

			#if UNITY_EDITOR
			if(prevGameState != null)
			{
				for(int i = prevGameState.Count-1; i >= 0; i--)
				{
					prevStates += "\n-> " + prevGameState[i].ToString();
				}
			}
			#endif

			return prevStates;
		}


		public bool SwitchState (BrynhildrState newState)
		{
			bool switchSuccess = false;
			if (states != null && states.ContainsKey (newState))
			{
				if (currentState == null)
				{
					currentState = states [newState];
					currentState.Start ();
					switchSuccess = true;
				}
				else if (currentState.AllowTransition (newState))
				{
					currentState.End ();
					currentState = states [newState];
					currentState.Start ();
					switchSuccess = true;
				}
				else
				{
					Debug.Log (string.Format ("{0} does not allow transition to {1}", currentState.State, newState));
				}
			}

			if (switchSuccess)
			{
				// Updating state history
				#if UNITY_EDITOR
				if(prevGameState != null)
				{
					prevGameState.Add(newState);
					if(prevGameState.Count > 20)
					{
						prevGameState.RemoveAt(0);
					}
				}
				#endif

				if (this.OnStatePreSwitchEvent != null)
				{
					this.OnStatePreSwitchEvent (newState);
				}
			}
			else
			{
				Debug.Log ("States dictionary not ready for switching to " + newState);
			}

			return switchSuccess;
		}
	}
}
