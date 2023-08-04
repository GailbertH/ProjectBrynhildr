using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;
using UnityEngine.UI;

namespace Brynhildr.UI
{
	public class BrynhildrGameControls : MonoBehaviour 
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private GameJoyStick gameJoyStick;
		[SerializeField] private Animation loadingScreen;
		[SerializeField] private Text gameResult;
		[SerializeField] private Text playerHP1;
		[SerializeField] private Text playerHP2;
        [SerializeField] private Text playerHP3;

        private static BrynhildrGameControls instance;
		private ButtonType buttonType = ButtonType.NONE;
		private bool isDragging = false;


		public static BrynhildrGameControls Instance { get { return instance; } }
		public float GetJoyStickHorizontal{ get {return gameJoyStick.Horizontal ();} }
		public float GetJoyStickVertical{ get {return gameJoyStick.Vertical ();} }
		public ButtonType GetButtonClick{ get{ return buttonType; } }

		public bool OnJoyStickDrag
		{
			set{ isDragging = value; }
			get{ return isDragging; }
		}

		public void ButtonClick(int butt)
		{
			buttonType = GameManager.Instance.ConvertToButtonType(butt);
		}

		public void BackToMainMenu()
		{
			GameManager.Instance.ExitGame ();
		}

		public void AnimateLoadingScreen()
		{
			loadingScreen.Play ();
		}

		public void GameResult(string result)
		{
			gameResult.text = result;
			gameResult.gameObject.SetActive (!string.IsNullOrEmpty (result));
		}

		void Awake()
		{
			instance = this;
			mainCamera.gameObject.SetActive (false);
		}

		#if UNITY_EDITOR
		void FixedUpdate()
		{
			if (gameJoyStick.GetInputVector != Vector3.zero)
				return;

			if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) 
			{
				isDragging = true;
			} 
			else if (isDragging) 
			{
				isDragging = false;
			}

			if (Input.GetKeyDown (KeyCode.Z))
			{
				GameManager.Instance.Player.CharacterSwitch ();
			}
		}
#endif

		public void UpdateHP(int id, int value)
		{
			if (id == 0)
			{
				UpdateP1HP(value);
            }
			else if (id == 1)
			{
                UpdateP2HP(value);
            }
			else if (id == 2)
			{
                UpdateP3HP(value);
            }
		}

		private void UpdateP1HP(int value)
		{
			playerHP1.text = "P1 HP: " + value;
		}

        private void UpdateP2HP(int value)
        {
            playerHP2.text = "P2 HP: " + value;
        }

        private void UpdateP3HP(int value)
        {
            playerHP3.text = "P3 HP: " + value;
        }
    }
}
