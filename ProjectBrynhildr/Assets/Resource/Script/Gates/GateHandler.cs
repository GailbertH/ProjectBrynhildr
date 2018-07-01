using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Brynhildr.Gate
{
	public class GateHandler : MonoBehaviour 
	{
		private static GateHandler instance = null;
		public static GateHandler Instance {get { return instance;}}

		[SerializeField] private List<GateController> gates;

		void Awake()
		{
			instance = this;
		}

		public GateController GetCurrentActiveGate
		{
			get
			{ 
				if (gates == null)
					return null;
				else if(gates.Count <= 0)
					return null;
				
				gates.OrderByDescending(m => m.gateId); 
				return gates[0]; 
			}
		}

		public void GateDamage(int damageAmount)
		{
			gates [0].Damage (damageAmount);
			if (gates [0].isDead)
				RemoveGate ();
		}

		public void RemoveGate()
		{
			gates [0].GateDestroy ();
			gates.RemoveAt (0);
		}
	}
}