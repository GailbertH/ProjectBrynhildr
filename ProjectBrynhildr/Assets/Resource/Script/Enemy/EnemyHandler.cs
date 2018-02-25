using System.Collections;
using System.Collections.Generic;
using Brynhildr.Player;
using Brynhildr.Game;
using UnityEngine;

namespace Brynhildr.Enemy
{
	public class EnemyAnimParam
	{
		public const string MOVE = "Move";
		public const string ATTACK = "Attack";
		public const string FLINCH = "Flinch";
		public const string IDLE = "Idle";
		public const string DEATH = "Death";
	}

	public class EnemyHandler : MonoBehaviour 
	{
		List<EnemyController> enemyList = new List<EnemyController> ();
		int enemyIDTracker = 0;

		public GameManager GetGameManager
		{
			get { return GameManager.Instance; }
		}

		public List<PlayerHandler> GetPlayerList
		{
			get { return GetGameManager != null ? GetGameManager.Player.GetPlayerCharacters : null;}
		}

		public void AddEnemy(EnemyController enemy)
		{
			enemyList.Add (enemy);
			enemy.EnemyID = enemyIDTracker;
			enemyIDTracker++;
		}

		void FixedUpdate()
		{
			if (GetGameManager != null && GetPlayerList != null && enemyList != null) 
			{
				for (int i = 0; i < enemyList.Count; i++) 
				{
					enemyList [i].EnemyUpdate ();
				}
			}
		}
	}
}
