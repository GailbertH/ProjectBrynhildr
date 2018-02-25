using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brynhildr.Game;

public class PlayerAnimEventListner : MonoBehaviour 
{

	public void AllowCombo()
	{
		if(GameManager.Instance != null)
			GameManager.Instance.Player.AllowCombo ();
	}

	public void DoNotAllowCombo()
	{
		if(GameManager.Instance != null)
			GameManager.Instance.Player.DoNotAllowCombo ();
	}

	public void DamageTarget()
	{
		if(GameManager.Instance != null)
			GameManager.Instance.Player.DamageTarget ();
	}
}
