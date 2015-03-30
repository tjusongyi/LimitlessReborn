using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCTeleportTarget
{
	public int NPCTeleportID;
	public int CurrencyID;
	public string CurrencyPreffix;
	public string Name;
	public int HoursSpend;
	public string SceneName;
	public Vector3 Position;
	
	public List<Condition> Conditions;
	
	public NPCTeleportTarget()
	{
		Conditions = new List<Condition>();
		Name = string.Empty;
		SceneName = string.Empty;
		CurrencyPreffix = string.Empty;
	}
}
