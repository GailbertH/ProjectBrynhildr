using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Brynhildr.Game;
using Brynhildr.Enemy;

namespace Brynhildr.Player
{
	[Serializable]
	public class PlayerState  
	{
		public virtual PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.NONE;
		}

		public virtual void Start (PlayerController controller)
		{
			controller.SwitchState (PlayerData.State.IDLE);
		}

		public virtual void Update (PlayerController controller)
		{
		}

		public virtual void End (PlayerController controller)
		{
		}

		public virtual void Flinch(PlayerController controller)
		{

		}

		public virtual void AnimateReset(PlayerController controller)
		{
		}

		public virtual void Destroy (PlayerController controller)
		{
		}
	}

	[Serializable]
	public class PlayerState_Idle : PlayerState
	{
		private bool isSwitchingState = false;
		public override PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.IDLE;
		}

		public override void Start (PlayerController controller)
		{
			isSwitchingState = false;
		}

		public override void Update (PlayerController controller)
		{
			PlayerController mainCharacter = GameManager.Instance.Player.currChar ();
			EnemyController enemyTarget = null;
			bool isEnemyNearby = false;
			for (int i = 0; i < GameManager.Instance.Enemies.GetEnemyList.Count; i++) 
			{
				if (Vector3.Distance (GameManager.Instance.Enemies.GetEnemyList [i].GetEnemyPosition, controller.GetPostion) < 30f
					&& GameManager.Instance.Enemies.GetEnemyList [i].IsAlive) 
				{
					isEnemyNearby = true;
					enemyTarget = GameManager.Instance.Enemies.GetEnemyList [i];
					controller.TargetEnemy = GameManager.Instance.Enemies.GetEnemyList [i];
				}
			}

			if(isEnemyNearby)
			{
				Vector3 direction = enemyTarget.GetEnemyPosition - controller.GetPostion;
				direction.y = 0;
				controller.playerChar.transform.rotation = Quaternion.Slerp (controller.playerChar.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
				DelayToChangeState (controller, PlayerData.State.PURSUIT);
			}

			else if (Vector3.Distance (mainCharacter.GetPostion, controller.GetPostion) < 30f 
				|| enemyTarget == null) 
			{
				Vector3 direction = mainCharacter.GetPostion - controller.GetPostion;
				direction.y = 0;
				controller.playerChar.transform.rotation = Quaternion.Slerp (controller.playerChar.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
				DelayToChangeState (controller, PlayerData.State.FOLLOW);
			}
		}

		private void DelayToChangeState(PlayerController controller, PlayerData.State stateToChange)
		{
			if (isSwitchingState)
				return;

			isSwitchingState = true;
			controller.SetDelaySwithState (stateToChange, 0.5f);
		}

		public override void End (PlayerController controller)
		{
		}

		public override void Destroy (PlayerController controller)
		{
		}
	}

	[Serializable]
	public class PlayerState_Follow : PlayerState
	{
		private float startUpSpeed = 0.1f;
		public override PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.FOLLOW;
		}

		public override void Start (PlayerController controller)
		{
			startUpSpeed = 0.1f;
		}

		public override void Update (PlayerController controller)
		{
			Vector3 direction =  GameManager.Instance.Player.currChar().GetPostion - controller.GetPostion;
			direction.y = 0;
			controller.playerChar.transform.rotation = Quaternion.Slerp (controller.playerChar.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude > 17) 
			{
				startUpSpeed += 0.05f;
				startUpSpeed = Mathf.Clamp (startUpSpeed, 0.1f, 1);
				Vector3 move = controller.playerChar.transform.TransformDirection (0, 0, 1 * controller.playerData.SPD );
				controller.charController.Move (move);
			}
			else 
			{
				startUpSpeed = 0.1f;
				controller.SwitchState (PlayerData.State.IDLE);
			}
		}

	}

	[Serializable]
	public class PlayerState_Pursuit : PlayerState
	{
		private float startUpSpeed = 0.1f;
		public override PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.PURSUIT;
		}

		public override void Start (PlayerController controller)
		{
			startUpSpeed = 0.1f;
		}

		public override void Update (PlayerController controller)
		{
			if(controller.TargetEnemy == null 
				|| (controller.TargetEnemy != null && !controller.TargetEnemy.IsAlive))
				controller.SwitchState (PlayerData.State.IDLE);

			Vector3 direction = controller.TargetEnemy.GetEnemyPosition - controller.GetPostion;
			direction.y = 0;
			controller.playerChar.transform.rotation = Quaternion.Slerp (controller.playerChar.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude > 17) 
			{
				startUpSpeed += 0.05f;
				startUpSpeed = Mathf.Clamp (startUpSpeed, 0.1f, 1);
				Vector3 move = controller.playerChar.transform.TransformDirection (0, 0, 1 * controller.playerData.SPD );
				controller.charController.Move (move);
			}
			else 
			{
				controller.SwitchState (PlayerData.State.ATTACKING);
			}
		}
	}

	[Serializable]
	public class PlayerState_Attacking : PlayerState
	{
		bool isAttacking = false;
		public override PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.ATTACKING;
		}

		public override void Start (PlayerController controller)
		{
			isAttacking = false;
			AnimateReset (controller);
		}

		public override void AnimateReset(PlayerController controller)
		{
			isAttacking = false;
		}

		public override void Update (PlayerController controller)
		{
			Vector3 direction = controller.TargetEnemy.GetEnemyPosition - controller.GetPostion;
			direction.y = 0;
			controller.playerChar.transform.rotation = Quaternion.Slerp (controller.playerChar.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
			if (direction.magnitude < 17)// && !isAttacking) 
			{
				ButtonType buttType = GameManager.Instance.GameControls.GetButtonClick;
				controller.playerAnimtor.SetBool (AnimParam.PERFORMING_ATTACK, true);
				controller.playerAnimtor.SetInteger (AnimParam.ACTION_TRACKER, (int)buttType);
				//isAttacking = true;
			} 
			else
			{
				controller.SwitchState (PlayerData.State.PURSUIT);
			}
		}
	}

	[Serializable]
	public class PlayerState_Death : PlayerState
	{
		public override PlayerData.State GetCurrentState ()
		{
			return PlayerData.State.DEATH;
		}
	}
}
