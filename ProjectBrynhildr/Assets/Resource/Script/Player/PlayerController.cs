using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;
using Brynhildr.Enemy;

namespace Brynhildr.Player
{
	public enum CharacterMode
	{
		CONTROLLING = 0,
		AI = 1
	}

	public class PlayerController : MonoBehaviour 
	{
		[SerializeField] public CharacterController charController;
		[SerializeField] public Animator playerAnimtor;
		[SerializeField] public GameObject playerChar;
		[SerializeField] public GameObject cameraHolder;
		[SerializeField] public PlayerData playerData;
		private CharacterMode currentMode = CharacterMode.AI;

		private Dictionary<PlayerData.State, PlayerState> states = new Dictionary<PlayerData.State, PlayerState> ();
		private PlayerData.State currentStateType = PlayerData.State.NONE;
		private PlayerData.State nextDelayStateType = PlayerData.State.NONE;
		private PlayerState currentState = null;

		public void SetCharacterMode(CharacterMode mode)
		{
			currentMode = mode;
			Debug.Log (playerData.characterID + " " + mode.ToString ());
		}

		public CharacterMode CurrentMode
		{
			get { return currentMode; }
		}

		public int GetPlayerID
		{
			get{ return playerData.characterID; }
		}

		public Vector3 GetPostion
		{
			get{ return playerChar.transform.position; }
		}

		public void Revive()
		{
			playerData.HP = Mathf.Clamp (playerData.BaseHP, 0, playerData.BaseHP);
			playerAnimtor.SetBool (AnimParam.DEAD, false);
		}

		public bool IsDead
		{
			get{
				return playerData.HP > 0;
			}
		}

		public void ReduceLife(int damage)
		{
			playerData.HP -= damage;
			playerData.HP = Mathf.Clamp (playerData.HP, 0, playerData.BaseHP);
			if (playerData.HP <= 0) 
			{
				playerAnimtor.SetBool (AnimParam.DEAD, true);
				if (GameManager.Instance.Enemies != null) 
				{
					GameManager.Instance.Enemies.RemoveEnemyTarget (playerData.characterID);
				}
				Invoke ("Revive", 5);
			}
		}

		#region AI
		private EnemyController targetPlayer = null;
		public EnemyController TargetEnemy
		{
			set { targetPlayer = value; }
			get { return targetPlayer;  }
		}

		void Start () 
		{
			states.Add (PlayerData.State.IDLE, 		(PlayerState)(new PlayerState_Idle ()));
			states.Add (PlayerData.State.PURSUIT, 	(PlayerState)(new PlayerState_Pursuit ()));
			states.Add (PlayerData.State.ATTACKING, (PlayerState)(new PlayerState_Attacking ()));
			states.Add (PlayerData.State.DEATH,  	(PlayerState)(new PlayerState_Death ()));
			states.Add (PlayerData.State.FOLLOW,  	(PlayerState)(new PlayerState_Follow ()));
			currentState = states [PlayerData.State.IDLE];

			if (playerData.characterID == GameManager.Instance.Player.currChar ().playerData.characterID)
				this.currentMode = CharacterMode.CONTROLLING;

			currentState.Start (this);
		}

		void FixedUpdate()
		{         
			if (currentMode == CharacterMode.CONTROLLING)
				return;

			if (states != null && currentState != null) 
			{
				currentState.Update (this);
			}
		}

		public void SwitchState(PlayerData.State newStateType)
		{
			if (states != null && states.ContainsKey (newStateType) && currentState.GetCurrentState () != newStateType) 
			{
				currentState.End (this);
				currentState = states [newStateType];
				currentState.Start (this);

				currentStateType = newStateType;
			}
		}

		public void SetDelaySwithState(PlayerData.State newStateType, float delay = 0.5f)
		{
			nextDelayStateType = newStateType;
			Invoke ("DelaySwitchState", delay);
		}

		public void DelaySwitchState()
		{
			if (nextDelayStateType != PlayerData.State.NONE) 
			{
				SwitchState (nextDelayStateType);
				nextDelayStateType = PlayerData.State.NONE;
			}
		}

		#endregion
	}
}
