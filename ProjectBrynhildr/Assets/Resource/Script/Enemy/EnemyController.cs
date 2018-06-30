using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Player;

namespace Brynhildr.Enemy
{
	public class EnemyController : MonoBehaviour 
	{
		public EnemyData enemyData = new EnemyData();

		private int enemyID = -1;
		private EnemyHandler handler = null; 
		private Dictionary<EnemyData.State, EnemyState> states = new Dictionary<EnemyData.State, EnemyState> ();
		private EnemyData.State currentStateType = EnemyData.State.NONE;
		private EnemyData.State nextDelayStateType = EnemyData.State.NONE;
		private EnemyState enemyState = null;
		private PlayerController targetPlayer = null;
		private Animator enemyAnim;

		public int EnemyID
		{
			set{ enemyID = value;}
			get{ return enemyID;}
		}

		public bool IsAlive 
		{
			get{ return enemyData.life > 0;}
		}

		public Vector3 GetEnemyPosition
		{
			get { return this.gameObject.transform.position; }
		}

		public Animator Anim
		{
			get { return enemyAnim; }
		}

		public EnemyHandler Handler
		{
			get { return handler; }
		}

		public EnemyData.State GetCurrentState
		{
			get { return enemyState.GetCurrentState(); }
		}

		public PlayerController TargetPlayer
		{
			set { targetPlayer = value; }
			get { return targetPlayer;  }
		}

		public void ForceChangeAggro(int ID, int forceValue = -1)
		{
			if (TargetPlayer != null && ID == TargetPlayer.GetPlayerID) 
			{
				enemyData.ForceSetAggro (ID, forceValue);
				if (TargetPlayer.GetPlayerID != enemyData.GetHighestAggroID () && TargetPlayer.IsDead) 
				{
					TargetPlayer = handler.GetPlayerList [enemyData.GetHighestAggroID ()];
				}
			}
		}

		public void AggroMeterUpdate(int playerId, int aggroAdd)
		{
			enemyData.AddAggro (playerId, aggroAdd);
			if (TargetPlayer != null && TargetPlayer.GetPlayerID != enemyData.GetHighestAggroID ())
			{
				TargetPlayer = handler.GetPlayerList [enemyData.GetHighestAggroID()];
			}
		}

		public void ReduceLife(int damage)
		{
			Debug.Log (enemyData.life);
			
			if (enemyData.life < 0) 
			{
				SwitchState (EnemyData.State.DEATH);
				enemyData.life = 10;
				EnemySpawnerManager.Instance.StartSpawning ();
			} 
			else if (GetCurrentState != EnemyData.State.DEATH)
			{
				enemyData.life = enemyData.life - damage;
				enemyState.Flinch (this);
			}
		}

		void Start () 
		{
			states.Add (EnemyData.State.IDLE, 		(EnemyState)(new EnemyState_Idle ()));
			states.Add (EnemyData.State.PURSUIT, 	(EnemyState)(new EnemyState_Pursuit ()));
			states.Add (EnemyData.State.ATTACKING,  (EnemyState)(new EnemyState_Attacking ()));
			states.Add (EnemyData.State.DEATH,  	(EnemyState)(new EnemyState_Death ()));
			enemyState = states [EnemyData.State.IDLE];


			if (handler == null) 
			{
				handler = GetComponentInParent<EnemyHandler> ();
				handler.AddEnemy (this);
				enemyData.SetAggroTracker (handler.GetPlayerList.Count);
			}
			enemyAnim = GetComponentInParent<Animator> ();
			Debug.Log ("Enemy Start");
			enemyState.Start(this);
		}

		public void EnemyUpdate()
		{
			if (states != null && enemyState != null) 
			{
				enemyState.Update (this);
			}
		}

		public void Destroy ()
		{
			if (states != null)
			{
				foreach (EnemyData.State key in states.Keys)
				{
					states [key].Destroy (this);
				}
				states.Clear ();
				states = null;
			}
		}

		public void SwitchState (EnemyData.State newStateType)
		{
			if (states != null && states.ContainsKey (newStateType) && enemyState.GetCurrentState () != newStateType) 
			{
				enemyState.End (this);
				enemyState = states [newStateType];
				enemyState.Start (this);

				currentStateType = newStateType;
			}
		}

		public void SetDelaySwithState(EnemyData.State newStateType, float delay = 0.5f)
		{
			nextDelayStateType = newStateType;
			Invoke ("DelaySwitchState", delay);
		}

		public void DelaySwitchState()
		{
			if (nextDelayStateType != EnemyData.State.NONE) 
			{
				SwitchState (nextDelayStateType);
				nextDelayStateType = EnemyData.State.NONE;
			}
		}

		public void SetDelayStart(float delay = 0.2f)
		{
			Invoke ("DelayStart", delay);
		}

		public void DelayStart()
		{
			enemyState.Start (this);
		}


		public void AnimationStart()
		{
		}
		public void AnimationEnd()
		{
			enemyState.AnimateReset (this);
		}
		public void DamageTarget()
		{
			if (TargetPlayer != null) 
			{
				Vector3 direction = TargetPlayer.GetPostion - GetEnemyPosition;
				if (direction.magnitude < 18) {
					TargetPlayer.ReduceLife (1);
				}
			}
		}
	}
}
