using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.MissionData;
using Brynhildr.Game;

public class EnemySpawnerManager : MonoBehaviour 
{
	[SerializeField] private MissionData missionData;
	[SerializeField] private List<GameObject> spawnSpot;
	private static EnemySpawnerManager instance;
	public static EnemySpawnerManager Instance { get { return instance; } }
	private Coroutine routine = null;

	private int phaseController;

	void Awake ()
	{
		instance = this;
	}

	void Destroy()
	{
		if (routine != null)
			StopCoroutine (routine);

		routine = null;
		instance = null;
	}

	public void StartSpawning()
	{
		if(missionData.enemyGrid.Count > phaseController && routine == null)
			routine = StartCoroutine (Spawner());
		Debug.Log ("Routine Start");
	}

	private IEnumerator Spawner()
	{
		for(int i = 0; i < missionData.enemyGrid[phaseController].enemyList.Count; i++)
		{
			yield return new WaitForSeconds (missionData.enemyGrid[phaseController].enemyList[i].spawnDelay);
			int random = Random.Range (0, spawnSpot.Count);
			GameObject enemy = Instantiate<GameObject> (missionData.enemyCache.Find
				(m => m.enemyData.enemyType == missionData.enemyGrid[phaseController].enemyList[i].enemyTyoe).gameObject, 
				spawnSpot[random].transform.position, 
				Quaternion.identity
				) as GameObject;
			enemy.transform.SetParent (GameManager.Instance.Enemies.gameObject.transform);
		}
		phaseController += 1;
		yield return new WaitForEndOfFrame ();
		routine = null;
		Debug.Log ("Routine End");

	}
}
