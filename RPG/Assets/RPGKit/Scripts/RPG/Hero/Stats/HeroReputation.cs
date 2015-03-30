using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroReputation
{
	public int ReputationID;
	public int Value;
	public string Rank;
	public string TextBefore;
	public string Name;
	
	public HeroReputation()
	{
		Rank = string.Empty;
		TextBefore = string.Empty;
		Name = string.Empty;
	}
}
