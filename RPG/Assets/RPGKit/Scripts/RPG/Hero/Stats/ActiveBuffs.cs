using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ActiveBuffs
{
	public List<ActiveEffect> Effects;
	private List<Effect> ConstantEffects;
	[XmlIgnore]
	private List<Effect> AllEffects;
	[XmlIgnore]
	int numberToRemove = 0;

    public ActiveBuffs()
	{
		Effects = new List<ActiveEffect>();
		ConstantEffects = new List<Effect>();
		AllEffects = new List<Effect>();
	}
	
	public void DoEffects(Player player)
	{
		//add one second to active effect
		AddSecond();
		
		//bonus from race
		if (player.Hero.Settings.UseCharacterRace)
		{
			if (player.Hero.characterRace != null)
			{
				List<Effect> effects = player.Hero.characterRace.Effects;
				foreach(Effect e in effects)
					ConstantEffects.Add(e);
			}
		}
		
		//adding all type effects together
		PrepareCollection();
		
		BonusValue bv;
		
		//reset bonus stats
		player.Hero.BonusHp = 0;
        player.Hero.HpRegeneration = player.Hero.Settings.StartHitPointRegeneration;
        player.Hero.ManaRegeneration = player.Hero.Settings.StartManaPointRegeneration;
        player.Hero.BonusMana = 0;
		
		if (AllEffects == null || AllEffects.Count == 0)
			return;
		
		//adding effects to player stats
		foreach(Effect activeEffect in AllEffects)
		{
			switch(activeEffect.EffectType)
			{
				case EffectTypeEnum.Attribute:
					bv = player.Hero.Bonuses.GetBonusValue(activeEffect.ID, activeEffect.Value);
					player.Hero.Bonuses.SpellAttributes.Add(bv);
					break;
				case EffectTypeEnum.HitPoint:
                    player.Hero.BonusHp += activeEffect.Value;
					break;
				case EffectTypeEnum.HitPointRegen:
                    player.Hero.HpRegeneration += activeEffect.Value;
					break;
				case EffectTypeEnum.HitPointChange:
                    player.Hero.CurrentHp += activeEffect.Value;
					break;
				case EffectTypeEnum.Mana:
                    player.Hero.BonusMana += activeEffect.Value;
					break;
				case EffectTypeEnum.ManaPointChange:
                    player.Hero.CurrentMana += activeEffect.Value;
					break;
				case EffectTypeEnum.ManaRegen:
                    player.Hero.ManaRegeneration += activeEffect.Value;
					break;
				case EffectTypeEnum.Skill:
					bv = player.Hero.Bonuses.GetBonusValue(activeEffect.ID, activeEffect.Value);
					player.Hero.Bonuses.SpellSkills.Add(bv);
					break;
			}
		}

        player.Hero.TotalHp = player.Hero.BaseHp + player.Hero.BonusHp;
        player.Hero.TotalMana = player.Hero.BaseMana + player.Hero.BonusMana;

        if (player.scene.SceneType != SceneTypeEnum.MainMenu)
        {
            //regen mana
            if (player.Hero.CurrentMana >= player.Hero.TotalMana)
            {
                player.Hero.CurrentMana = player.Hero.TotalMana;
            }
            else
            {
                player.Hero.CurrentMana += player.Hero.ManaRegeneration;
            }
            //regen hitpoint
            if (player.Hero.CurrentHp >= player.Hero.TotalHp)
            {
                player.Hero.CurrentHp = player.Hero.TotalHp;
            }
            else
            {
                player.Hero.CurrentHp += player.Hero.HpRegeneration;
            }
        }
		
		//remove effect that ended duration
		RemoveEffects();
	}
	
	private void PrepareCollection()
	{
		AllEffects = new List<Effect>();
		foreach(ActiveEffect af in Effects)
		{
			AllEffects.Add(af);
		}
		
		foreach(Effect ef in ConstantEffects)
		{
			AllEffects.Add(ef);
		}
	}
	
	private void AddSecond()
	{
		//add 1 second to all effects
		//calculate how much effect should be removed
		foreach(ActiveEffect effect in Effects)
		{
			//-1 for duration is infinite
			if (effect.Duration == -1)
				continue;
			
			effect.CurrentDuration++;
				
			if (effect.CurrentDuration >= effect.Duration)
				numberToRemove++;
		}
	}
		
	private void RemoveEffects()
	{
		if (numberToRemove == 0)
			return;
		
		//remove effects
		for(int x = 1; x <= numberToRemove; x++)
		{
			foreach(ActiveEffect af in Effects)
			{
				if (af.CurrentDuration >= af.Duration)
				{
					Effects.Remove(af);
					break;
				}
			}
		}	
	}
	
	public void AddEffect(Effect effect)
	{
		ActiveEffect af = new ActiveEffect();
		af.Duration = effect.Duration;
		af.ID = effect.ID;
		af.Target = effect.Target;
		af.Value = effect.Value;
		af.ValueMax = effect.ValueMax;
		af.EffectType = effect.EffectType;
		Effects.Add(af);
	}
}
