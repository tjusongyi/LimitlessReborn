using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellShopItem
{
	public int SpellID;
	public List<Condition> Conditions;
	
	public SpellShopItem()
	{
		Conditions = new List<Condition>();
	}
	
	public void AddSpell(List<RPGSpell> allSpells, List<RPGSpell> currentCollection, Player player)
	{
		if (!CanYouDisplay(player))
			return;
		
		RPGSpell spell = new RPGSpell();
		bool founded = false;
		foreach(RPGSpell s in allSpells)
		{
			if (s.ID == SpellID)
			{
				founded = true;
				spell = s;
			}
		}
		
		if (founded)
		{
			foreach(RPGSpell s in currentCollection)
			{
				if (s.ID == SpellID)
					return;
			}
			currentCollection.Add(spell);
		}
	}
	
	public bool CanYouDisplay(Player player)
	{
		foreach(Condition condition in Conditions)
		{
            if (condition.Validate(player) == false)
				return false;
		}
		return true;
	}
}
