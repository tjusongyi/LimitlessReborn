using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// Hold information how much attribute / skill will incrase in game statistic per one point of attribute / skill
/// </summary>
public class StatisticIncrease : BasicItem
{
	public int Maximum;
	public float Value;
	public int BaseValue;
	
	public float MeleeDmg;
	public float RangeDmg;
	public float MagicDmg;
	
	public float MeleeAccuracy;
	public float RangeAccuracy;
	public float MagicAccuracy;
	
	public int HitPoint;
	public float HitPointRegen;
	
	public int Mana;
	public float ManaRegen;
	
	public int Vitality;
	public float VitalityRegen;
	
	public float MeleeCrit;
	public float RangeCrit;
	public float MagicCrit;
	
	public float Armor;
	public float MagicResistance;
	
	public float MovementSpeed;
	
	public float MeleeAvoidance;
	public float RangeAvoidance;
	
	//calculate chance to hit by attack type
	public int ChanceToHit(CombatSkillType attackType)
	{
		float chanceToHit = 0;
		switch(attackType)
		{
			case CombatSkillType.Magic: chanceToHit += MagicAccuracy * Value;
				break;
			case CombatSkillType.Melee: chanceToHit += MeleeAccuracy * Value;
				break;
			case CombatSkillType.Range: chanceToHit += RangeAccuracy * Value;
				break;
		}
		return (int)System.Math.Round(chanceToHit);
	}
	
	//calculate damage by attack type
	public int AddDamage(CombatSkillType attackType)
	{
		float damage = 0;
		switch(attackType)
		{
			case CombatSkillType.Magic: damage += MagicDmg * Value;
				break;
			case CombatSkillType.Melee: damage += MeleeDmg * Value;
				break;
			case CombatSkillType.Range: damage += RangeDmg * Value;
				break;
		}
		return (int)System.Math.Round(damage);
	}
	
	public float AddAvoidance(CombatSkillType attackType)
	{
		float avoidance = 0;
		switch(attackType)
		{
			case CombatSkillType.Melee: avoidance += MeleeAvoidance * Value;
				break;
			case CombatSkillType.Range: avoidance += RangeAvoidance * Value;
				break;
		}
		return avoidance;
	}
	
	public void Add(int amount, Player player)
	{
		if (Value + amount > Maximum)
			Value = Maximum;
		else
			Value += amount;

        player.Hero.CalculateDamage(player);
	}
}
