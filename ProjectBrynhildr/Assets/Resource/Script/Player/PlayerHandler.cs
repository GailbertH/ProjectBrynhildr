using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;
using Brynhildr.Enemy;

namespace Brynhildr.Player
{
	public class AnimParam
	{
		public const string PERFORMING_ATTACK = "PerformingAttack";
		public const string ACTION_TRACKER = "ActionTracker";
		public const string ALLOW_COMBO = "ComboAllow";
		public const string COMBO = "Combo";
		public const string VERTICAL = "Vertical";
		public const string HORIZONTAL = "Horizontal";
	}
	
	public class PlayerHandler : MonoBehaviour 
	{
		//Player States
		[SerializeField] private List<PlayerHandler> PlayerList;
		[SerializeField] private CharacterController charController;
		[SerializeField] private Animator playerAnimtor;
		[SerializeField] private GameObject playerChar;

		private float vertical = 0;
		private float horizontal = 0;
		private float speed = 0.4f;
		private bool movementReset = true;
		private bool attackReset = true;
		private bool delayReseting = false;
		private bool attackCombo = true;
		private bool allowCombo = false;

		private Vector3 movement;

		public List<PlayerHandler> GetPlayerCharacters
		{
			get{ return  PlayerList; }
		}

		public Vector3 GetPostion
		{
			get{ return playerChar.transform.position; }
		}

		private void MovementReset()
		{
			vertical = 0;
			horizontal = 0;
			playerAnimtor.SetFloat (AnimParam.VERTICAL, vertical);
			playerAnimtor.SetFloat (AnimParam.HORIZONTAL, horizontal);
			movementReset = true;
		}
		private void AttackReset()
		{
			GameManager.Instance.GameControls.ButtonClick(0);
			playerAnimtor.SetInteger (AnimParam.ACTION_TRACKER, 0);
			playerAnimtor.SetBool (AnimParam.PERFORMING_ATTACK, false);
			attackReset = true;
			attackCombo = true;
			delayReseting = false;
			Debug.Log ("Attack Reset");
		}


		void FixedUpdate()
		{
			if(GameManager.Instance.GameControls!= null)
			{
				if (GameManager.Instance.GameControls.OnJoyStickDrag) 
				{
					vertical = GameManager.Instance.GameControls.GetJoyStickVertical;
					horizontal = GameManager.Instance.GameControls.GetJoyStickHorizontal;
					playerAnimtor.SetFloat (AnimParam.VERTICAL, vertical);
					playerAnimtor.SetFloat (AnimParam.HORIZONTAL, horizontal);
					movement = new Vector3 (horizontal * speed, 0, vertical * speed);
					CharacterRotater (movement);
					MoveCharacter (movement);
					movementReset = false;
				}
				else if(!GameManager.Instance.GameControls.OnJoyStickDrag && !movementReset)
				{
					this.MovementReset ();
				}
				if (GameManager.Instance.GameControls.GetButtonClick != ButtonType.NONE && attackReset && !delayReseting)
				{
					ButtonType buttType = GameManager.Instance.GameControls.GetButtonClick;
					playerAnimtor.SetBool (AnimParam.PERFORMING_ATTACK, true);
					playerAnimtor.SetInteger (AnimParam.ACTION_TRACKER, (int)buttType);
					attackReset = false;
					Debug.Log (buttType.ToString ());
				} 
				else if (GameManager.Instance.GameControls.GetButtonClick == ButtonType.NORMAL_ATTACK && allowCombo && attackCombo && !attackReset && !delayReseting) 
				{
					playerAnimtor.SetTrigger (AnimParam.COMBO);
					attackCombo = false; 
					Debug.Log ("Trigger");
				}
				else if(GameManager.Instance.GameControls.GetButtonClick != ButtonType.NONE && !attackReset && !delayReseting)
				{
					delayReseting = true;
					Invoke ("AttackReset", 0.5f);
				}
			}
		}

		public void AllowCombo()
		{
			allowCombo = true;
			playerAnimtor.SetBool (AnimParam.ALLOW_COMBO, allowCombo);
		}

		public void DoNotAllowCombo()
		{
			Invoke ("ComboDisable", 0.3f);
			//ComboDisable();
		}

		private void ComboDisable()
		{
			allowCombo = false;
			playerAnimtor.SetBool (AnimParam.ALLOW_COMBO, allowCombo);
		}

		public void DamageTarget()
		{
			Vector3 fwd = playerChar.transform.TransformDirection(Vector3.forward);
			RaycastHit hitInfo;
			if (Physics.Raycast (GetPostion, fwd, out hitInfo, 25)) 
			{
				if(hitInfo.collider.gameObject.GetComponent<EnemyController> () != null)
					hitInfo.collider.gameObject.GetComponent<EnemyController> ().ReduceLife(1);
				Debug.Log ("OUCH");
			}
		}

		private void MoveCharacter(Vector3 move)
		{
			charController.Move (move);
		}

		private void CharacterRotater(Vector3 direc)
		{
			playerChar.transform.rotation = Quaternion.LookRotation (direc);
		}
	}
}
