using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGuild 
{
	public List<GuildProgress> Guilds;
	
	
	public PlayerGuild()
	{
		Guilds = new List<GuildProgress>();
	}
	
	public GuildProgress GetGuildProgress(int GuildID)
	{
		foreach(GuildProgress gp in Guilds)
		{
			if (gp.GuildID == GuildID)
				return gp;
		}
		return null;
	}
	
	public bool IsMemberGuild(int GuildID)
	{
		foreach(GuildProgress gp in Guilds)
		{
			if (gp.GuildID == GuildID)
				return true;
		}
		return false;
	}
	
	public void Load()
	{
		foreach(GuildProgress gp in Guilds)
		{
			gp.Load();
		}
	}
	
	public void JoinGuild(int guildId)
	{
		GuildProgress gp = new GuildProgress();
		gp.GuilRankID = 1;
		gp.GuildID = guildId;
		Guilds.Add(gp);
	}
	
	public void UpdateRank(RPGGuild guild, Player player)
	{
		GuildProgress gp = GetGuildProgress(guild.ID);

        guild.GainReward(gp.GuilRankID, player);
		
		gp.GuilRankID = gp.GuilRankID + 1; 
	}
}
