using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;

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
	}
}
