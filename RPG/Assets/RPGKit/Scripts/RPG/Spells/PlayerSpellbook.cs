using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpellbook 
{
	public List<RPGSpell> Spells;
	
	public PlayerSpellbook()
	{
		Spells = new List<RPGSpell>();
	}
	
	public void AddSpell(RPGSpell spell)
	{
		foreach(RPGSpell s in Spells)
		{
			if (s.ID == spell.ID)
				return;
		}
		
		Spells.Add(spell);
	}
}
