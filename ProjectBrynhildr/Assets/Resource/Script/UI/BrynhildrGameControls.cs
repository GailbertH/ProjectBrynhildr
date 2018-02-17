using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;

namespace Brynhildr.UI
{
	public class BrynhildrGameControls : MonoBehaviour 
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private GameJoyStick gameJoyStick;

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
		}
		#endif

	}
}
