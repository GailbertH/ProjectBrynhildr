using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Player
{
	public class PlayersHandler : MonoBehaviour 
	{
		[SerializeField] private CharacterController charController;
		[SerializeField] private Animator playerAnimtor;
		[SerializeField] private GameObject playerChar;
		[SerializeField] private GameObject cameraHolder;
		[SerializeField] public PlayerData playerData;
	}
}
