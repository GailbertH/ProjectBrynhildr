using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour 
{
	public void StartGameClicked()
	{
		LoadingManager.Instance.SetSceneToUnload (SceneNames.MAIN_MENU);
		LoadingManager.Instance.SetSceneToLoad (SceneNames.LOBBY_SCENE);
		LoadingManager.Instance.LoadGameScene ();
	}
}
