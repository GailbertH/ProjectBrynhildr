using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Game
{
	public class Brynhildr_Exit : BrynhildrState_Base<BrynhildrState>
	{
		public Brynhildr_Exit (GameManager manager) : base (BrynhildrState.EXIT, manager)
		{
		}
			
		public override void Start ()
		{ 
			//START UNLOADING 
		}
		public override void End () 
		{ 
			//END EVERYTHING
		}
	}
}
