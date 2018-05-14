using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Game
{
	public class Brynhildr_Loading : BrynhildrState_Base<BrynhildrState>
	{
		private Coroutine cdNextState;

		public Brynhildr_Loading (GameManager manager) : base (BrynhildrState.LOADING, manager)
		{
		}

		public override void GoToNextState()
		{
			Manager.StateMachine.SwitchState (BrynhildrState.INGAME);
		}

		public override bool AllowTransition (BrynhildrState nextState)
		{
			return (nextState == BrynhildrState.INGAME);
		}

		public override void Start ()
		{
			//Loading hahahaha
			cdNextState = Manager.StartCoroutine (DelayedStateSwitch (2f));
		}

		public override void End () 
		{
			if (cdNextState != null && Manager != null)
				Manager.StopCoroutine (cdNextState);

			cdNextState = null;
		}

		public override void Destroy ()
		{
			End ();
			base.Destroy ();
		}


		private IEnumerator DelayedStateSwitch (float delay)
		{
			yield return new WaitForSeconds(delay);
			SoundManager.Instance.PlayBGM ();
			Manager.LoadingScreenPlay ();
			GoToNextState ();
		}
	}
}
