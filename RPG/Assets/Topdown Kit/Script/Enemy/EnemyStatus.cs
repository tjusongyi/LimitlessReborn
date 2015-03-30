/// <summary>
/// Enemy status.
/// This script use for adjust a status enemy
/// </summary>

using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {
	
	public string enemyName;
	
	[System.Serializable]
	public class Attribute
	{
		public int hp,mp,atk,def,spd,hit;
		public float criticalRate,atkSpd,atkRange,movespd;
	}
	
	public Attribute status;
	
	public int expGive;
	
}
