using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effect 
{
	public TargetType Target;
	public EffectTypeEnum EffectType; 
	public int Value;
	public int ValueMax;
	public int Duration;
	public int ID;
	
	public float TooltipPart(float x, float y, GUISkin skin, string caption, List<Effect> effects, Player player)
	{
		y += 25;
		
		Rect rect = new Rect(x + 10, y, 140,20);
		GUI.Label(rect, caption, skin.label);
		
		y += 25;
		
		foreach(Effect e in effects)
		{
			rect = new Rect(x + 10, y, 340,20);
			
			switch(e.EffectType)
			{
				case EffectTypeEnum.Attribute:
					foreach(RPGAttribute a in Player.Data.Attributes)
					{
						if (a.ID == ID)
						{
							
							GUI.Label(rect, Label(e ,"Increase", "Lower", a.Name), skin.label);
							
							break;
						}
					}
					break;
				
				case EffectTypeEnum.CastSpell:
					foreach(RPGSpell a in Player.Data.Spells)
					{
						if (a.ID == ID)
						{
							GUI.Label(rect, Label(e ,"Cast", "Cast", a.Name), skin.label);
							
							break;
						}
					}
					break;
				
				case EffectTypeEnum.HitPoint:
					GUI.Label(rect, Label(e ,"Increase", "Lower",  "health"), skin.label);
					break;
				
				case EffectTypeEnum.HitPointChange:
					GUI.Label(rect, Label(e ,"Restore", "Damage", "health"), skin.label);
					break;
				
				case EffectTypeEnum.HitPointRegen:
					GUI.Label(rect, Label(e ,"Increase", "Lower", "hp regen"), skin.label);
					break;
				
				case EffectTypeEnum.Mana:
					GUI.Label(rect, Label(e ,"Increase", "Lower", "mana"), skin.label);
					break;
				
				case EffectTypeEnum.ManaPointChange:
					GUI.Label(rect, Label(e ,"Restore", "Lower", "mana"), skin.label);
					break;
				
				case EffectTypeEnum.ManaRegen:
					GUI.Label(rect, Label(e ,"Increase", "Lower", "mana regen"), skin.label);
					break;
				
				case EffectTypeEnum.Skill:
					foreach(RPGSkill a in Player.Data.Skills)
					{
						if (a.ID == ID)
						{
							GUI.Label(rect, Label(e ,"Increase", "Lower", a.Name), skin.label);
							
							break;
						}
					}
					break;
			}
			y += player.Hero.Settings.TooltipRowSize;
		}
		return y;
	}
	
	private string Label(Effect e, string effectType, string negativeEffect, string effectTarget)
	{
		string label = effectType + " ";
		
		if (Value < 1) 
			label = negativeEffect + " ";
		
		label += effectTarget;
		
		if (e.Target == TargetType.Self)
		{
			label += " to self";						
		}
		else if (e.Target == TargetType.Other)
		{
			label += " to other";
		}
		else
		{
			label += " to all enemies";
		}
		
		int v = e.Value;
		if (v < 0)
			v = v * -1;
		
		int vv = e.ValueMax;
		if (vv < 0)
			vv = v * -1;
		
		label += " by " + v.ToString();
		
		if (v != vv && vv > 0)
		{
			label += "-" + vv.ToString();
		}
		
		
		
		
		if (e.Duration > 0)
		{
			label += " for " + e.Duration.ToString() + "s";
		}
		else if (e.Duration == -1)
		{
			label += " permanently";
		}
		return label;
	}
}

public enum TargetType
{
	Self = 0,
	Other = 1,
	AOE = 2
}

public enum EffectTypeEnum
{
	HitPoint = 0,
	Mana = 1,
	Attribute = 2,
	Skill = 3,
	CastSpell = 4,
	HitPointRegen = 5,
	ManaRegen = 6,
	HitPointChange = 7,
	ManaPointChange = 8
}

public enum EffectTypeUsage
{
	ArmorTakeHit = 1,
	WeaponHit = 2,
	Spell = 3,
	Usable = 4,
	Equiped = 5,
	RaceBonus = 6,
    EffectArea =7
}