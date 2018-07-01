using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Brynhildr.Player;
using Brynhildr.Game;


namespace Brynhildr.Enemy
{
	[Serializable]
	public class EnemyState
	{	
		public virtual EnemyData.State GetCurrentState ()
		{
			return EnemyData.State.NONE;
		}

		public virtual void Start (EnemyController controller)
		{
			controller.SwitchState (EnemyData.State.IDLE);
		}

		public virtual void Update (EnemyController controller)
		{
		}

		public virtual void End (EnemyController controller)
		{
		}

		public virtual void Flinch(EnemyController controller)
		{
			controller.Anim.SetBool (EnemyAnimParam.ATTACK, false);
			controller.Anim.SetBool (EnemyAnimParam.FLINCH, true);
			controller.Anim.SetBool (EnemyAnimParam.MOVE, false);
			controller.Anim.SetBool (EnemyAnimParam.DEATH, false);
			controller.SetDelayStart ();
		}

		public virtual void AnimateReset(EnemyController controller)
		{
		}

		public virtual void Destroy (EnemyController controller)
		{
		}
	}

	[Serializable]
	public class EnemyState_Idle : EnemyState
	{
		private bool isSwitchingState = false;
		public override EnemyData.State GetCurrentState ()
		{
			return EnemyData.State.IDLE;
		}

		public override void Start (EnemyController controller)
		{
			isSwitchingState = false;
			controller.Anim.SetBool (EnemyAnimParam.ATTACK, false);
			controller.Anim.SetBool (EnemyAnimParam.FLINCH, false);
			controller.Anim.SetBool (EnemyAnimParam.MOVE, false);
			controller.Anim.SetBool (EnemyAnimParam.DEATH, false);
			controller.CheckAggro ();
		}

		public override void Update (EnemyController controller)
		{
			/*
			for (int i = 0; i < controller.Handler.GetPlayerList.Count; i++) 
			{
				PlayerController player = controller.Handler.GetPlayerList [i];
				if (Vector3.Distance (player.GetPostion, controller.GetEnemyPosition) < 30f) 
				{
					if (controller.TargetPlayer == null) 
					{
						controller.TargetPlayer = player;
						controller.AggroMeterUpdate (player.GetPlayerID, 2);
					}

					Vector3 direction = player.GetPostion - controller.GetEnemyPosition;
					direction.y = 0;
					controller.transform.rotation = Quaternion.Slerp (controller.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
					DelayToPursuit (controller);
				}
			}*/
			if (controller.TargetPlayer == null)
				controller.CheckAggro ();

			Vector3 direction = controller.TargetPlayer.GetPostion - controller.GetEnemyPosition;
			direction.y = 0;
			controller.transform.rotation = Quaternion.Slerp (controller.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			DelayToPursuit (controller);
		}

		private void DelayToPursuit(EnemyController controller)
		{
			if (isSwitchingState)
				return;
			
			isSwitchingState = true;
			controller.SetDelaySwithState (EnemyData.State.PURSUIT, 0.5f);
		}

		public override void End (EnemyController controller)
		{
		}

		public override void Destroy (EnemyController controller)
		{
		}
	}

	[Serializable]
	public class EnemyState_Pursuit : EnemyState
	{
		public override EnemyData.State GetCurrentState ()
		{
			return EnemyData.State.PURSUIT;
		}

		public override void Start (EnemyController controller)
		{
			controller.Anim.SetBool (EnemyAnimParam.ATTACK, false);
			controller.Anim.SetBool (EnemyAnimParam.FLINCH, false);
			controller.Anim.SetBool (EnemyAnimParam.MOVE, true);
			controller.Anim.SetBool (EnemyAnimParam.DEATH, false);
		}

		public override void Update (EnemyController controller)
		{
			Vector3 direction = controller.TargetPlayer.GetPostion - controller.GetEnemyPosition;
			direction.y = 0;
			controller.transform.rotation = Quaternion.Slerp (controller.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude > 17) 
			{
				controller.transform.Translate (0, 0, 0.2f);
			}
			else 
			{
				controller.SwitchState (EnemyData.State.ATTACKING);
			}
		
		}
		public override void End (EnemyController controller)
		{
		}

		public override void Destroy (EnemyController controller)
		{
		}
	}

	[Serializable]
	public class EnemyState_Attacking : EnemyState
	{
		bool isAttacking = false;
		public override EnemyData.State GetCurrentState ()
		{
			return EnemyData.State.ATTACKING;
		}

		public override void Start (EnemyController controller)
		{
			isAttacking = false;
			AnimateReset (controller);
		}

		public override void AnimateReset(EnemyController controller)
		{
			controller.Anim.SetBool (EnemyAnimParam.ATTACK, true);
			controller.Anim.SetBool (EnemyAnimParam.FLINCH, false);
			controller.Anim.SetBool (EnemyAnimParam.MOVE, false);
			controller.Anim.SetBool (EnemyAnimParam.DEATH, false);
			isAttacking = false;
			Debug.Log (controller.TargetPlayer.name);
		}

		public override void Update (EnemyController controller)
		{
			Vector3 direction = controller.TargetPlayer.GetPostion - controller.GetEnemyPosition;
			direction.y = 0;
			controller.transform.rotation = Quaternion.Slerp (controller.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude < 17 && !isAttacking) 
			{
				isAttacking = true;
			} 
			else if(!isAttacking)
			{
				controller.SwitchState (EnemyData.State.PURSUIT);
			}
		}

		public override void End (EnemyController controller)
		{
		}

		public override void Destroy (EnemyController controller)
		{
		}
	}

	[Serializable]
	public class EnemyState_Death : EnemyState
	{
		public override EnemyData.State GetCurrentState ()
		{
			return EnemyData.State.DEATH;
		}

		public override void Start (EnemyController controller)
		{
			controller.Anim.SetBool (EnemyAnimParam.ATTACK, false);
			controller.Anim.SetBool (EnemyAnimParam.FLINCH, false);
			controller.Anim.SetBool (EnemyAnimParam.MOVE, false);
			controller.Anim.SetBool (EnemyAnimParam.DEATH, true);

			/*
			controller.SetDelaySwithState (EnemyData.State.IDLE, 2);
			controller.enemyData.ResetAggroSystem ();
			controller.TargetPlayer = null;
			*/
			GameManager.Instance.Enemies.GetEnemyList.Remove (controller);
			if(GameManager.Instance.Enemies.GetEnemyList.Count <= 0)
				EnemySpawnerManager.Instance.StartSpawning ();
			
			controller.Destroy ();
		}

		public override void Update (EnemyController controller)
		{
		}

		public override void End (EnemyController controller)
		{
		}

		public override void Destroy (EnemyController controller)
		{
		}
	}
}
