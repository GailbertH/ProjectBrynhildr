using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMeter : MonoBehaviour {

	[SerializeField] private Transform fillMeter;

	private float meterValue;
	private bool isMeterFull;
	Vector3 scale = Vector3.one;

	// Use this for initialization
	void Start () 
	{
		this.Reset ();
		LoadingManager.Instance.SetSceneToLoad (SceneNames.MAIN_MENU);
		LoadingManager.Instance.LoadMainMenuScene ();
	}

	public void Reset()
	{
		Vector3 scale = fillMeter.localScale;
		scale.x = 0;
		fillMeter.localScale = scale;
		isMeterFull = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isMeterFull) 
		{
			if (this.OnLoadDoneEvent != null)
				this.OnLoadDoneEvent ();

			this.OnLoadDoneEvent = null;
			this.OnLoadMeterChangeEvent = null;
			return;
		}

		if (OnLoadMeterChangeEvent != null)
			OnLoadMeterChangeEvent (this.meterValue);

		scale.x = meterValue;
		fillMeter.localScale = scale;
		Debug.Log (scale.x);
		isMeterFull = scale.x >= 1;
	}

	public float MeterValue
	{
		set{meterValue = value;}
		get{return meterValue;}
	}


	public void OnLoadMeterChange(ValueChange method)
	{
		this.OnLoadMeterChangeEvent += method;
	}
	public void OnLoadDone(LoadingDone method)
	{
		this.OnLoadDoneEvent += method;
	}

	public delegate void ValueChange(float value);
	private event ValueChange OnLoadMeterChangeEvent;

	public delegate void LoadingDone();
	private event LoadingDone OnLoadDoneEvent;
}
