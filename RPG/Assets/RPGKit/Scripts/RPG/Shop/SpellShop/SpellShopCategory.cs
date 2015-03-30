using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

public class SpellShopCategory
{
	public List<Condition> Conditions;
	public LevelAdjustmentType LevelAdjustment;
	public int MinValue;
	public int MaxValue;
	public int SkillID;
	
	[XmlIgnore]
	private int currentMinLevel;
	[XmlIgnore]
	private int currentMaxLevel;
	
	public SpellShopCategory()
	{
		Conditions = new List<Condition>();
	}
	
	private void DetermineLevel(Player player)
	{
		//determine level of items for this category
		if (LevelAdjustment == LevelAdjustmentType.Fixed)
		{
			currentMinLevel = MinValue;
			currentMaxLevel = MaxValue;
		}
		else
		{
            currentMinLevel = player.Hero.CurrentLevel - MinValue;
            currentMaxLevel = player.Hero.CurrentLevel + MaxValue;
		}
	}
	
	public void AddSpells(List<RPGSpell> allSpells, List<RPGSpell> currentCollection, Player player)
	{
		//validate conditions
		if (!CanYouDisplay(player))
			return;

        DetermineLevel(player);
		
		foreach(RPGSpell s in allSpells)
		{
			if (s.Level < currentMinLevel || s.Level > currentMaxLevel)
				continue;
			
			if (s.SkillId != SkillID)
				continue;
			
			AddSpell(s, currentCollection);
		}
	}
	
	private void AddSpell(RPGSpell spell, List<RPGSpell> currentCollection)
	{
		foreach(RPGSpell s in currentCollection)
		{
			if (s.ID == spell.ID)
				return;
		}
		currentCollection.Add(spell);
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
