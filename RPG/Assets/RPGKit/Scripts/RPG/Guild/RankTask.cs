using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankTask 
{
	public RankTaskEnum Task;
	public int ID;
	public string Preffix;
	public int Amount;
	
	public RankTask()
	{
		Preffix = "ITEM";
	}

    public void UpgradeToNextRank(Player Player)
	{
		if (Task == RankTaskEnum.Item)
		{
            Player.Hero.Inventory.RemoveItem(ID, Preffix, Amount, Player);
		}
	}
	
	public bool ValidateTask(int rankID, int guildID, Player player)
	{
		bool result = false;
		Preffix = "ITEM";
		switch(Task)
		{
			//completed quest
			case RankTaskEnum.Quest:
				int count = 0;
				foreach(CompletedQuest cq in player.Hero.Quest.CompletedQuests)
				{
					if (cq.GuildRankID == rankID && guildID == cq.GuildID)
					 	count++;
				}
				if (count >= Amount)
					result = true;
				
				break;
			
			//required item in inventory
			case RankTaskEnum.Item:
				count = 0;
				foreach(InventoryItem item in player.Hero.Inventory.Items)
				{
					if (item.UniqueItemId == UniqueId)
					{
						count += item.CurrentAmount;
					}
				}
				if (count >= Amount)
					result = true;
				break;
			
			//attribute value
			case RankTaskEnum.Attribute:
				foreach(RPGAttribute atr in player.Hero.Attributes)
				{
					if (atr.ID == ID && atr.Value > Amount)
						result =  true;
				}
				break;
			
			//skill value
			case RankTaskEnum.Skill:
				foreach(RPGSkill skill in player.Hero.Skills)
				{
					if (skill.ID == ID && skill.Value > Amount)
						result =  true;
				}
				break;
			
			//spell in spell book
			case RankTaskEnum.Spell:
				foreach(RPGSpell spell in player.Hero.Spellbook.Spells)
				{
					if (spell.ID == ID)
						result = true;
				}
				break;
				
		}
		return result;
	}
					
	private string UniqueId
	{
		get
		{
			return Preffix + ID.ToString();
		}
	}
}


public enum RankTaskEnum
{
	Quest,
	Attribute,
	Item,
	Spell,
	Skill
}