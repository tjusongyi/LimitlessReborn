using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(ReputationRank))]
public class RPGReputation : BasicItem
{
	public string TextBefore;
	public string LowestRank;
	
	public List<ReputationRank> Ranks;
	
	public RPGReputation()
	{
		TextBefore = string.Empty;
		LowestRank = string.Empty;
        preffix = "REPUTATION";
		Ranks = new List<ReputationRank>();
	}
	
	public string RankName(int amount)
	{
		if (Ranks.Count == 0)
			return string.Empty;
			
			
		string result = string.Empty;
			
		foreach(ReputationRank rp in Ranks)
		{
			if (amount > rp.Amount)
			{
				result = rp.Name;
			}
		}
		
		return result;
	}
}
