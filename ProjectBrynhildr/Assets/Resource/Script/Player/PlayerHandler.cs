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
		public const string DEAD = "Dead";
	}
	
	public class PlayerHandler : MonoBehaviour 
	{
		//Player States
		[SerializeField] private PlayerHandler PlayerList;
		[SerializeField] public List<PlayerController> charControler;

		private int characterInControl = 0;
		private float vertical = 0;
		private float horizontal = 0;
		private bool movementReset = true;
		private bool attackReset = true;
		private bool delayReseting = false;
		private bool attackCombo = true;
		private bool allowCombo = false;

		private Vector3 movement;

		private PlayerController currChar()
		{
			int charId = Mathf.Clamp (characterInControl, 0, charControler.Count);
			return charControler [charId];
		}

		public List<PlayerController> GetPlayerCharacters
		{
			get{ return  charControler; }
		}

		public void CharacterSwitch(int IDofCharacter = 0, bool isForced = true)
		{
			if (isForced) {
				characterInControl += 1;
			} else
				characterInControl = IDofCharacter;
			
			if (charControler.Count <= characterInControl) 
			{
				characterInControl = 0;
			}
			for (int i = 0; i < charControler.Count; i++) 
			{
				if (characterInControl == currChar().playerData.characterID) 
				{
					if (currChar().cameraHolder != null) {
						currChar().cameraHolder.transform.SetParent (currChar().gameObject.transform);
						currChar().cameraHolder.transform.localPosition = Vector3.zero;
					}
				}
			}
		}

		private void MovementReset()
		{
			vertical = 0;
			horizontal = 0;
			currChar().playerAnimtor.SetFloat (AnimParam.VERTICAL, vertical);
			currChar().playerAnimtor.SetFloat (AnimParam.HORIZONTAL, horizontal);
			movementReset = true;
		}
		private void AttackReset()
		{
			GameManager.Instance.GameControls.ButtonClick(0);
			currChar().playerAnimtor.SetInteger (AnimParam.ACTION_TRACKER, 0);
			currChar().playerAnimtor.SetBool (AnimParam.PERFORMING_ATTACK, false);
			attackReset = true;
			attackCombo = true;
			delayReseting = false;
			Debug.Log ("Attack Reset");
		}


		void FixedUpdate()
		{
			if(GameManager.Instance.GameControls!= null 
				&& currChar().playerData != null 
				&& currChar().playerData.characterID == characterInControl)
			{
				ButtonType buttPressed = GameManager.Instance.GameControls.GetButtonClick;
				if (GameManager.Instance.GameControls.OnJoyStickDrag) 
				{
					vertical = GameManager.Instance.GameControls.GetJoyStickVertical;
					horizontal = GameManager.Instance.GameControls.GetJoyStickHorizontal;
					currChar().playerAnimtor.SetFloat (AnimParam.VERTICAL, vertical);
					currChar().playerAnimtor.SetFloat (AnimParam.HORIZONTAL, horizontal);
					movement = new Vector3 (horizontal * currChar().playerData.SPD, 0, vertical * currChar().playerData.SPD);
					CharacterRotater (movement);
					MoveCharacter (movement);
					movementReset = false;
				}
				else if(!GameManager.Instance.GameControls.OnJoyStickDrag && !movementReset)
				{
					this.MovementReset ();
				}
				Debug.Log (buttPressed.ToString ());
				if (buttPressed != ButtonType.NONE && attackReset && !delayReseting) 
				{
					ButtonType buttType = GameManager.Instance.GameControls.GetButtonClick;
					currChar ().playerAnimtor.SetBool (AnimParam.PERFORMING_ATTACK, true);
					currChar ().playerAnimtor.SetInteger (AnimParam.ACTION_TRACKER, (int)buttType);
					attackReset = false;
					Debug.Log (buttType.ToString ());
				} 
				else if (buttPressed == ButtonType.NORMAL_ATTACK && allowCombo && attackCombo && !attackReset && !delayReseting) {
					currChar ().playerAnimtor.SetTrigger (AnimParam.COMBO);
					attackCombo = false; 
					Debug.Log ("Trigger");
				} 
				else if ((int)buttPressed >= (int)ButtonType.CHARACTER_CHANGE_1 &&
					(int)buttPressed <= (int)ButtonType.CHARACTER_CHANGE_4) 
				{
					CharacterSwitch ((((int)buttPressed) - (int)ButtonType.CHARACTER_CHANGE_1), false);
				}
				else if (buttPressed != ButtonType.NONE && !attackReset && !delayReseting) 
				{
					delayReseting = true;
					Invoke ("AttackReset", 0.5f);
				} 
			}
		}

		public void AllowCombo()
		{
			allowCombo = true;
			currChar().playerAnimtor.SetBool (AnimParam.ALLOW_COMBO, allowCombo);
		}

		public void DoNotAllowCombo()
		{
			Invoke ("ComboDisable", 0.3f);
		}

		private void ComboDisable()
		{
			allowCombo = false;
			currChar().playerAnimtor.SetBool (AnimParam.ALLOW_COMBO, allowCombo);
		}

		public void DamageTarget()
		{
			Vector3 fwd = currChar().playerChar.transform.TransformDirection(Vector3.forward);
			Vector3 curPos = currChar ().GetPostion;
			curPos.y += 4;
			RaycastHit hitInfo;
			bool hitSomething = Physics.Raycast (curPos, fwd, out hitInfo, 25);
			if (hitSomething) 
			{
				if(hitInfo.collider != null)
					Debug.Log (hitInfo.collider.name);
				
				EnemyController enemy = hitInfo.collider.gameObject.GetComponent<EnemyController> ();
				if (enemy != null)
				{
					enemy.ReduceLife (currChar().playerData.DMG);
					enemy.AggroMeterUpdate(currChar().playerData.characterID, 1);
				}
				Debug.Log ("OUCH");
			}
		}

		private void MoveCharacter(Vector3 move)
		{
			currChar().charController.Move (move);
		}

		private void CharacterRotater(Vector3 direc)
		{
			currChar().playerChar.transform.rotation = Quaternion.LookRotation (direc);
		}
	}
}
