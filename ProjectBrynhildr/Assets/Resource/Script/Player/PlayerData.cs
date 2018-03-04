using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brynhildr.Player
{
	[Serializable]
	public class PlayerData 
	{
		public int characterID;
		public CharacterClass characterClass = CharacterClass.NONE;
		public int BaseHP; 
		public int HP;   //Character Health Ponts
		public int DMG;  //Amount of damage that the player does
		public int DEF;  //Amount of damage that the player reduces
		public int HEAL; //Effectiveness of heal spells
		public int SPD;  //Movement speed
	}
		
	public enum PlayerStatus
	{
		ALIVE = 0,
		DEAD = 1
	}

	public enum CharacterClass
	{
		NONE = 0,
		VANGUARD = 1,	 //TANK
		SPEARMAN = 2,	 //Melee DPS
		CASTER = 3,  	 //Range DPS
		SUMMONER = 4 	 //HEALER
	}
}
