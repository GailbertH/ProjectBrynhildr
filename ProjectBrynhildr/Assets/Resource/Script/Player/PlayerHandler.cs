using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;

namespace Brynhildr.Player
{
	public class PlayerHandler : MonoBehaviour 
	{
		//Player States
		[SerializeField] private CharacterController charController;
		[SerializeField] private Animator playerAnimtor;
		[SerializeField] private GameObject playerChar;

		private float vertical = 0;
		private float horizontal = 0;
		private float speed = 0.4f;
		private bool movementReset = true;
		private bool attackReset = true;
		private bool delayReseting = false;

		private Vector3 movement;

		private void MovementReset()
		{
			vertical = 0;
			horizontal = 0;
			playerAnimtor.SetFloat ("Vertical", vertical);
			playerAnimtor.SetFloat ("Horizontal", horizontal);
			movementReset = true;
		}
		private void AttackReset()
		{
			GameManager.Instance.GameControls.ButtonClick(0);
			playerAnimtor.SetInteger ("ActionTracker", 0);
			playerAnimtor.SetBool ("PerformingAttack", false);
			attackReset = true;
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
					playerAnimtor.SetFloat ("Vertical", vertical);
					playerAnimtor.SetFloat ("Horizontal", horizontal);
					movement = new Vector3 (horizontal * speed, 0, vertical * speed);
					CharacterRotater (movement);
					MoveCharacter (movement);
					movementReset = false;
					Debug.Log (GameManager.Instance.GameControls.OnJoyStickDrag.ToString());
				}
				else if(!GameManager.Instance.GameControls.OnJoyStickDrag && !movementReset)
				{
					this.MovementReset ();
				}
				if(GameManager.Instance.GameControls.GetButtonClick != ButtonType.NONE && attackReset && !delayReseting)
				{
					ButtonType buttType = GameManager.Instance.GameControls.GetButtonClick;
					playerAnimtor.SetBool ("PerformingAttack", true);
					playerAnimtor.SetInteger ("ActionTracker", (int)buttType);
					attackReset = false;
					Debug.Log (buttType.ToString());
				}
				else if(GameManager.Instance.GameControls.GetButtonClick != ButtonType.NONE && !attackReset && !delayReseting)
				{
					delayReseting = true;
					Invoke ("AttackReset", 0.5f);
				}
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
