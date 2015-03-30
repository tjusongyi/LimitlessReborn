using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class BonusCollection
{
	public List<BonusValue> ArmorAttributes;
	public List<BonusValue> ArmorSkills;
	public List<BonusValue> SpellAttributes;
	public List<BonusValue> SpellSkills;
	
	public List<RPGAttribute> TotalAttributes;
	public List<RPGSkill> TotalSkills;

   

    public BonusCollection()
	{
		ArmorAttributes = new List<BonusValue>();
		ArmorSkills = new List<BonusValue>();
		SpellAttributes = new List<BonusValue>();
		SpellSkills = new List<BonusValue>();
		
		TotalAttributes = new List<RPGAttribute>();
		TotalSkills = new List<RPGSkill>();
	}
	
	public void DefaultFill(Player player)
	{
		TotalAttributes = Storage.Load<RPGAttribute>(new RPGAttribute());
		TotalSkills = Storage.Load<RPGSkill>(new RPGSkill());
		
		foreach(RPGAttribute atr in player.Hero.Attributes)
		{
			ArmorAttributes.Add(GetCurrent(atr.ID));
			SpellAttributes.Add(GetCurrent(atr.ID));
		}
		
		foreach(RPGSkill skill in player.Hero.Skills)
		{
			ArmorSkills.Add(GetCurrent(skill.ID));
			SpellSkills.Add(GetCurrent(skill.ID));
		}
	}
	
	private BonusValue GetCurrent(int ID)
	{
		BonusValue bonusValue = new BonusValue();
		bonusValue.Value = 0;
		bonusValue.ID = ID;
		return bonusValue;
	}
	
	public BonusValue GetBonusValue(int ID, int amount)
	{
		BonusValue bonusValue = new BonusValue();
		bonusValue.Value = amount;
		bonusValue.ID = ID;
		return bonusValue;
	}
	
	public void AddEquipedBonus(List<Effect> effects)
	{
		foreach(BonusValue b in ArmorAttributes)
			b.Value = 0;
		
		foreach(BonusValue b in ArmorSkills)
			b.Value = 0;
		
		foreach(Effect effect in effects)
		{
			if (effect.EffectType == EffectTypeEnum.Attribute)
			{
				foreach(BonusValue b in ArmorAttributes)
				{
					if (b.ID == effect.ID)
					{
						b.Value += effect.Value;
					}
				}
			}
			
			if (effect.EffectType == EffectTypeEnum.Skill)
			{
				foreach(BonusValue b in ArmorSkills)
				{
					if (b.ID == effect.ID)
						b.Value += effect.Value;
				}
			}
		}
	}
	
	public void Calculate(Player player)
	{
		//reseting value
		foreach(RPGAttribute b in TotalAttributes)
			b.Value = 0;
		
		foreach(RPGSkill b in TotalSkills)
			b.Value = 0;
		
		//calculate stats from equip
        AddEquipedBonus(player.Hero.Equip.GetEffects());
		
		//adding normal stats
        foreach (RPGAttribute attribute in player.Hero.Attributes)
		{
			foreach(RPGAttribute a in TotalAttributes)
			{
				if (a.ID == attribute.ID)
					a.Value = attribute.Value;
			}
		}

        foreach (RPGSkill skill in player.Hero.Skills)
		{
			foreach(RPGSkill s in TotalSkills)
			{
				if (s.ID == skill.ID)
					s.Value = skill.Value;
			}
		}
		
		//adding stats from equip
		foreach(RPGAttribute attribute in TotalAttributes)
		{
			foreach(BonusValue b in ArmorAttributes)
			{
				if (attribute.ID == b.ID)
				{
					attribute.Value += b.Value;
				}
			}
		}
		
		foreach(RPGSkill skill in TotalSkills)
		{
			foreach(BonusValue b in ArmorSkills)
			{
				if (skill.ID == b.ID)
				{
					skill.Value += b.Value;
				}
			}
		}
		
		//adding stats from spells
		foreach(RPGAttribute attribute in TotalAttributes)
		{
			foreach(BonusValue b in SpellAttributes)
			{
				if (attribute.ID == b.ID)
				{
					attribute.Value += b.Value;
				}
			}
		}
		
		foreach(RPGSkill skill in TotalSkills)
		{
			foreach(BonusValue b in SpellSkills)
			{
				if (skill.ID == b.ID)
				{
					skill.Value += b.Value;
				}
			}
		}
	}
}
