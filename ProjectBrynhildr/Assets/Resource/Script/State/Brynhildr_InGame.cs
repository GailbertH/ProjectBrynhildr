using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Game
{
	public class Brynhildr_InGame : BrynhildrState_Base<BrynhildrState>
	{
		public Brynhildr_InGame (GameManager manager) : base (BrynhildrState.INGAME, manager)
		{
		}

		public override void GoToNextState()
		{
			Manager.StateMachine.SwitchState (BrynhildrState.EXIT);
		}

		public override bool AllowTransition (BrynhildrState nextState)
		{
			return (nextState == BrynhildrState.EXIT);
		}
		public override void Start () {}
		public override void Update () {}
		public override void End () {}
	}
}
