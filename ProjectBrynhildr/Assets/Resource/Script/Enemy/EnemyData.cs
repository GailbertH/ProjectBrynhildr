using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EnemyData
{
	public enum State
	{
		NONE = 0,
		IDLE = 1,
		PURSUIT = 2,
		ATTACKING = 3,
		DEATH = 4,
	}

	public int life = 10;
}
