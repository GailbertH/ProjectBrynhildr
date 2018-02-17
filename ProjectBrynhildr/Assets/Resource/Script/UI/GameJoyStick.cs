using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace Brynhildr.UI
{
	public class GameJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler 
	{
		[SerializeField] private Image bgImg;
		[SerializeField] private Image joyStickImage;
		private Vector3 inputVector;

		public virtual void OnDrag(PointerEventData ped) 
		{
			Vector2 pos;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform
				,ped.position
				,ped.pressEventCamera
				,out pos))
			{
				pos.x = (pos.x /bgImg.rectTransform.sizeDelta.x);
				pos.y = (pos.y /bgImg.rectTransform.sizeDelta.y);

				inputVector = new Vector3 (pos.x * 2f, 0f, pos.y * 2f);
				inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

				joyStickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x/2)
					,inputVector.z * (bgImg.rectTransform.sizeDelta.y/2));
			}
		}
		public virtual void OnPointerDown(PointerEventData ped) 
		{
			OnDrag(ped);
			BrynhildrGameControls.Instance.OnJoyStickDrag = true;
		}
		public virtual void OnPointerUp(PointerEventData ped) 
		{
			inputVector = Vector3.zero;
			joyStickImage.rectTransform.anchoredPosition = inputVector;
			BrynhildrGameControls.Instance.OnJoyStickDrag = false;
		}

		public float Horizontal()
		{
			if (inputVector.x != 0)
				return inputVector.x;
			else
				return Input.GetAxis("Horizontal");
		}

		public float Vertical()
		{
			if (inputVector.z != 0)
				return inputVector.z;
			else
				return Input.GetAxis("Vertical");
		}

		#if UNITY_EDITOR
		public Vector3 GetInputVector{ get { return inputVector; } }
		#endif
	}
}
