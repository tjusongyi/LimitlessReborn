using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class GuildProgress
{
	public int GuildID;
	public int GuilRankID;
	
	[XmlIgnore]
	public RPGGuild G;
	
	public void Load()
	{
		G = Storage.LoadById<RPGGuild>(GuildID, new RPGGuild());
	}
	
	public bool IsReady(Player player)
	{
        return G.IsPreparedForNextRank(GuilRankID, player);
	}

    public void Upgrade(Player player)
	{
		G.GainReward(GuilRankID, player);
	}
}
