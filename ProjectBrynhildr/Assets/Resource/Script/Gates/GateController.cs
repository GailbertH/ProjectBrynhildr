using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

	[SerializeField] public int gateId;
	[SerializeField] public int life = 20;
	public bool isDead = false;

	public void GateDestroy()
	{
		Debug.Log ("BOOM");
	}


	public Vector3 GetPostion
	{
		get{ return this.transform.position; }
	}

	public void Damage(int damage)
	{
		life -= damage;
		isDead = life > 0;
		if (isDead)
			this.gameObject.SetActive (false);
	}
}
