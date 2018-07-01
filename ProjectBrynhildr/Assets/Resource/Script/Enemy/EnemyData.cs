using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EnemyData
{
	public enum EnemyType
	{
		NONE = 0,
		MELEE = 1,
		RANGE = 2,
		TANK = 3,
		BOSS = 4,
		SPECIAL = 5
	}

	public enum State
	{
		NONE = 0,
		IDLE = 1,
		PURSUIT = 2,
		ATTACKING = 3,
		DEATH = 4,
	}

	public EnemyType enemyType = EnemyType.MELEE;
	public int life = 10;
	public List<int> aggroTracker = new List<int>();



	#region Aggro System
	private const int MaxAggro = 20;
	private int aggroValueCount = 50;
	private bool aggroTrackerSet = false;

	public void ResetAggroSystem()
	{
		for (int i = 0; i < aggroTracker.Count; i++) 
		{
			aggroTracker [i] = 0;
		}
	}

	public void SetAggroTracker(int playerCount)
	{
		aggroTracker = new List<int>();
		for (int i = 0; i < playerCount; i++) 
		{
			aggroTracker.Add (-1);
		}
		aggroTrackerSet = true;
	}

	public void ForceSetAggro(int playerID, int setAggro)
	{
		if (!aggroTrackerSet)
			return;

		if(playerID < aggroTracker.Count)
		{
			aggroValueCount += aggroTracker [playerID];
			aggroTracker [playerID] = setAggro;
		}
	}

	public void AddAggro(int playerID, int aggroToAdd)
	{
		if (!aggroTrackerSet)
			return;
		
		if(playerID < aggroTracker.Count)
		{
			CheckAggroLimit (playerID, aggroToAdd);
		}
	}

	private void CheckAggroLimit(int playerID, int aggroToAdd)
	{
		if (aggroTracker [playerID] >= MaxAggro) 
		{
			for (int i = 0; i < aggroTracker.Count; i++) 
			{
				if (playerID != i)
				{
					aggroTracker[i] -= 1;
					aggroValueCount += 1;
				}
			}
		} 
		else 
		{
			if(aggroValueCount <= 0)
			{
				int lowestAggroID = -1;
				for (int i = 0; i < aggroTracker.Count; i++)
				{
					if(aggroTracker[i] > 0 && playerID != i)
					{
						if (lowestAggroID < 0) 
						{
							lowestAggroID = i;
						}
						else if (aggroTracker [i] < aggroTracker [lowestAggroID])
						{
							lowestAggroID = i;
						}
					}
				}
				aggroTracker [lowestAggroID] -= aggroToAdd;
				aggroValueCount += aggroToAdd;
			}

			aggroTracker [playerID] += aggroToAdd;
			aggroValueCount -= aggroToAdd;
		}
	}

	public int GetHighestAggroID()
	{
		int HighestAggroID = -1;
		for (int i = 0; i < aggroTracker.Count; i++) 
		{
			if(aggroTracker[i] >= 0)
			{
				if (HighestAggroID < 0) 
				{
					HighestAggroID = i;
				}
				else if (aggroTracker [i] > aggroTracker [HighestAggroID])
				{
					HighestAggroID = i;
				}
			}
		}
		if (HighestAggroID == -1)
			HighestAggroID = Random.Range (0, aggroTracker.Count);

		Debug.Log ("TARGET TARGET " + HighestAggroID);

		return Mathf.Clamp (HighestAggroID, 0, aggroTracker.Count - 1);
	}
	#endregion
}
