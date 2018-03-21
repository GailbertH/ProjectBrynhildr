using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Brynhildr.Player
{
	public class PlayerController : MonoBehaviour 
	{
		[SerializeField] private CharacterController charController;
		[SerializeField] private Animator playerAnimtor;
		[SerializeField] private GameObject playerChar;
		[SerializeField] public PlayerData playerData;

	}
}
