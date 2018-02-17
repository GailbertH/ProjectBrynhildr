using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load Asynce, it also does scene maangement hahaha.. sweats pinagsama <3
/// </summary>
public class LoadingManager : MonoBehaviour 
{
	[SerializeField] private LoadingMeter loadingMeter;
	[SerializeField] GameObject canvas;
	[SerializeField] Camera mainCamera;

	private AsyncOperation asyncLoading;
	private AsyncOperation asyncUnloading;
	private string sceneToLoad;
	private string sceneToUnload;
	private Coroutine loadingRoutine;
	private Coroutine unloadingRoutine;

	private static LoadingManager instance;

	public static LoadingManager Instance { get { return instance; } }

	void Awake()
	{
		instance = this;
		mainCamera.gameObject.SetActive (true);
	}

	private void SetUpLoadingMeter()
	{
		mainCamera.gameObject.SetActive (true);
		loadingMeter.OnLoadMeterChange (this.OnLoadBarChange);
		loadingMeter.OnLoadDone (this.OnLoadBarFull);
	}
		
	private void OnLoadBarChange(float value)
	{
		Debug.Log("LoadBar " + value);
	}

	private void OnLoadBarFull()
	{
		Debug.Log("Load Bar Full ");

		if (canvas != null)
			canvas.SetActive (false);

		mainCamera.gameObject.SetActive (false);
		asyncLoading.allowSceneActivation = true;

		if(loadingRoutine != null)
			StopCoroutine (loadingRoutine);
		
		if(unloadingRoutine != null)
			StopCoroutine (unloadingRoutine);
	}

	#region SceneManagement
	public void SetSceneToLoad(string sceneName)
	{
		sceneToLoad = sceneName;
	}

	public void SetSceneToUnload(string sceneName)
	{
		sceneToUnload = sceneName;
	}

	public void LoadMainMenuScene()
	{
		if (canvas != null)
			canvas.SetActive (true);
		
		this.SetUpLoadingMeter ();
		loadingMeter.Reset ();
		loadingRoutine = StartCoroutine (LoadAsynceScene(false));
	}

	public void LoadGameScene()
	{
		if (canvas != null)
			canvas.SetActive (true);

		this.SetUpLoadingMeter ();
		loadingMeter.Reset ();
		unloadingRoutine = StartCoroutine (UnLoadAsyncScene ());
		loadingRoutine = StartCoroutine (LoadAsynceScene(true));
	}

	private IEnumerator LoadAsynceScene(bool allowActivation)
	{
		string[] sceneToLoadQueue = this.sceneToLoad.Split (',');
		float loadingProgress = 0;
		for (int i = 0; sceneToLoadQueue.Length > i; i++) 
		{
			loadingProgress = i;
			Debug.Log ("LOADING SCENE " + sceneToLoadQueue [i]);
			asyncLoading = SceneManager.LoadSceneAsync (sceneToLoadQueue[i], LoadSceneMode.Additive);
			asyncLoading.allowSceneActivation = allowActivation;

			while (!asyncLoading.isDone) 
			{
				Debug.Log(asyncLoading.progress + " + " + loadingProgress + " / " + (0.9f * sceneToLoadQueue.Length));
				loadingMeter.MeterValue = Mathf.Clamp01 ((asyncLoading.progress + loadingProgress) / (0.9f * sceneToLoadQueue.Length));
				Debug.Log (loadingMeter.MeterValue);
				yield return null;
			}
		}
		sceneToLoad = "";
	}

	private IEnumerator UnLoadAsyncScene()
	{
		if (sceneToUnload != "") 
		{
			asyncUnloading = SceneManager.UnloadSceneAsync (this.sceneToUnload);

			while (!asyncUnloading.isDone) 
			{
				yield return null;
			}
			sceneToUnload = "";
		}
	}
	#endregion
}
