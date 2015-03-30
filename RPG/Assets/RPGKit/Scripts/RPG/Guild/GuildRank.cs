using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(RankTask))]
[XmlInclude(typeof(Reward))]
public class GuildRank 
{
	public string Name;
	public int ID;
	public List<RankTask> Tasks;
	public List<Reward> Rewards;
	
	public GuildRank()
	{
		Rewards = new List<Reward>();
		Tasks = new List<RankTask>();
		Name = string.Empty;
	}
	
	public bool Validate(int GuildID, Player player)
	{
		foreach(RankTask rt in Tasks)
		{
            if (!rt.ValidateTask(ID, GuildID, player))
				return false;
		}
		return true;
	}

    public void GiveReward(Player player)
	{
		foreach(Reward r in Rewards)
		{
			PreffixSolver.GiveItem(r.Preffix, r.ItemId, r.Amount, player);
		}
	}

    public void UpdateToNextRank(Player player)
	{
		foreach(RankTask rt in Tasks)
		{
            rt.UpgradeToNextRank(player);
		}
	}
}
