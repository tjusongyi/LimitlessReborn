using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(GuildRank))]
[XmlInclude(typeof(Condition))]
public class RPGGuild : BasicItem
{
	public List<GuildRank> Ranks;
	public List<Condition> Conditions;
	
	public RPGGuild()
	{
		Ranks = new List<GuildRank>();
		Conditions = new List<Condition>();
		Name = string.Empty;
		Description = string.Empty;
        preffix = "GUILD";
	}
	
	public bool CanJoinGuild(Player player)
	{
        foreach (Condition c in Conditions)
        {
            if (!c.Validate(player))
            {
                return false;
            }
        }
        return true;
	}
	
	public GuildRank ActualRank(int id)
	{
		GuildRank gr = new GuildRank();
		foreach(GuildRank r in Ranks)
		{
			if (r.ID == id)
				gr = r;
		}
		return gr;
	}

    public bool IsPreparedForNextRank(int guildRankId, Player player)
	{
		GuildRank gr = ActualRank(guildRankId);

        return gr.Validate(ID, player);
	}
	
	public void GainReward(int guildRankId, Player player)
	{
		GuildRank gr = ActualRank(guildRankId);
        gr.UpdateToNextRank(player);
        gr.GiveReward(player);
	}
	
	public bool IsLastRank(int guildRankId)
	{
		foreach(GuildRank gr in Ranks)
		{
			if (gr.ID > guildRankId)
				return false;
		}
		return true;
	}
	
}
