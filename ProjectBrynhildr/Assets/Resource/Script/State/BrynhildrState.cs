using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base game state
/// -Gailbert Huang
/// </summary>

namespace Brynhildr.Game
{
	
	public enum BrynhildrState
	{
		LOADING = 0,
		INGAME = 1,
		EXIT = 2
	}
			
	public class BrynhildrState_Base<BrynhildrState>
	{
		private BrynhildrState state;
		private GameManager manager;

		public BrynhildrState State { get { return state; } }
		public GameManager Manager { get { return manager; } }

		public BrynhildrState_Base(BrynhildrState state, GameManager manager)
		{
			this.state = state;
			this.manager = manager;
		}

		public virtual bool AllowTransition (BrynhildrState nextState)
		{
			return true;
		}

		public virtual void GoToNextState() {}
		public virtual void Start () {}
		public virtual void Update () {}
		public virtual void End () {}
		public virtual void Destroy () 
		{
			End ();
			manager = null;
		}
	}
}
