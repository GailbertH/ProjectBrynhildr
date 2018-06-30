using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Game
{
	public class Brynhildr_Exit : BrynhildrState_Base<BrynhildrState>
	{
		public Brynhildr_Exit (GameManager manager) : base (BrynhildrState.EXIT, manager)
		{
		}
			
		public override void Start ()
		{ 
			//START UNLOADING
			LoadingManager.Instance.SetSceneToUnload (SceneNames.GAME_UI + "," + SceneNames.GAME_SCENE);
			LoadingManager.Instance.SetSceneToLoad (SceneNames.LOBBY_SCENE);
			LoadingManager.Instance.LoadGameScene ();
		}
		public override void End () 
		{ 
			//END EVERYTHING
		}
	}
}
