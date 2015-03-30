using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlternateEnd 
{	
	public string QuestLogEntry;
	public List<Reward> Rewards;
	public int ID;
	
	public AlternateEnd()
	{
		Rewards = new List<Reward>();
	}
	
	public void GiveReward(Player player)
	{
		foreach(Reward r in Rewards)
		{
			PreffixSolver.GiveItem(r.Preffix, r.ItemId, r.Amount, player);
		}
	}
}
