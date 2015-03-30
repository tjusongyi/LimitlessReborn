using UnityEngine;
using System.Collections;
using System.Xml;

public class RPGSkill : StatisticIncrease
{
	public float IncreasePerUse;
	
	public RPGSkill()
	{
		preffix = "SKILL";
	}
	
	public void IncreaseSkill(CombatSkillType skillType, Player player)
	{
		if (Value >= Maximum || IncreasePerUse == 0)
			return;
		
		int previousValue = (int)System.Math.Round(Value);
		
		switch(skillType)
		{
			case CombatSkillType.MeleeDefense: 
				if (MeleeAvoidance > 0)
					Value += IncreasePerUse;
				break;
			case CombatSkillType.RangeDefense: 
				if (RangeAvoidance > 0)
					Value += IncreasePerUse;
				break;
			case CombatSkillType.MagicDefense: 
				if (MagicResistance > 0)
					Value += IncreasePerUse;
				break;
			case CombatSkillType.Melee:
				if (MeleeDmg > 0)
					Value += IncreasePerUse;
				break;
			case CombatSkillType.Range:
				if (RangeDmg > 0)
					Value += IncreasePerUse;
				break;
			case CombatSkillType.Magic:
				if (MagicDmg > 0)
					Value += IncreasePerUse;
				break;
		}

        if (System.Math.Round(Value) - previousValue > 0 && player.Hero.Settings.Leveling == LevelingSystem.Skill)
		{
            player.Hero.AddXp((int)(System.Math.Round(Value) - previousValue));
		}
	}
}

