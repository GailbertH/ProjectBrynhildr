using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Brynhildr.Enemy;

namespace Brynhildr.MissionData
{
	public class MissionData : MonoBehaviour
	{
		[SerializeField] public int missionDataId;
		[SerializeField] public List<enemySpawnList> enemyGrid;
		[SerializeField] public List<EnemyController> enemyCache;
		[SerializeField] public List<GameObject> spawnPoints;
	}

	[Serializable]
	public class enemySpawnList
	{
		[SerializeField] public List<Enemy> enemyList;
		[SerializeField] public int phaseId;
		[SerializeField] public float timerSet;
	}

	[Serializable]
	public class Enemy 
	{
		[SerializeField] public EnemyData.EnemyType enemyTyoe;
		[SerializeField] public int enemyAmount;
		[SerializeField] public int spawnDelay;
		[SerializeField] public int designatedSpawnPoint;
	}
}



