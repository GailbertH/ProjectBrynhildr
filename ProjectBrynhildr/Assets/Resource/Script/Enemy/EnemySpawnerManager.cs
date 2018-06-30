using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.MissionData;
using Brynhildr.Game;

public class EnemySpawnerManager : MonoBehaviour 
{
	[SerializeField] private MissionData missionData;
	private static EnemySpawnerManager instance;
	public static EnemySpawnerManager Instance { get { return instance; } }

	private int phaseController;

	void Awake ()
	{
		instance = this;
	}

	public void StartSpawning()
	{
		Debug.Log ("Spawning");
		if(missionData.enemyGrid.Count > phaseController)
			StartCoroutine (Spawner());
	}

	private IEnumerator Spawner()
	{
		for(int i = 0; i < missionData.enemyGrid[phaseController].enemyList.Count; i++)
		{
			GameObject enemy = Instantiate<GameObject> (missionData.enemyCache.Find
				(m => m.enemyData.enemyType == missionData.enemyGrid[phaseController].enemyList[i].enemyTyoe).gameObject,
				GameManager.Instance.Enemies.gameObject.transform) as GameObject;
		}
		phaseController++;
		yield return new WaitForEndOfFrame ();
	}
}
