using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArmorGenerator : ItemGenerator
{
	public bool ArmorClassValue;
	public IncresingTypeEnum ArmorIncreasingDamageType;
	public int ArmorAmount;
	
	public ArmorGenerator() : base()
	{
		
		ArmorClassValue = GeneratorConfig.ArmorClassValue;
		ArmorIncreasingDamageType = GeneratorConfig.ArmorIncreasingDamageType;
		ArmorAmount = GeneratorConfig.ArmorAmount;
	}
	
	protected override void FillItems ()
	{
		for(int x = 1; x <= ItemsCount; x++)
		{
			RPGArmor sourceItem = (RPGArmor)SourceItem;
			RPGArmor item = new RPGArmor();
			item.Stackable = sourceItem.Stackable;
			item.MaximumStack = sourceItem.MaximumStack;
			item.Value = sourceItem.Value;
			item.Rarity = sourceItem.Rarity;
			item.NumberCharges = sourceItem.NumberCharges;
			item.IconPath = sourceItem.IconPath;
			item.PrefabName = sourceItem.PrefabName;
			item.Durability = sourceItem.Durability;
			item.CurrentDurability = sourceItem.Durability;
			foreach(Effect effect in sourceItem.Effects)
			{
				Effect e = new Effect();
				e.Duration = effect.Duration;
				e.EffectType = effect.EffectType;
				e.Target = effect.Target;
				e.Value = effect.Value;
				item.Effects.Add(e);
			}
			item.Categories = sourceItem.Categories;
			item.IsCopy = true;
			item.SourceItem = sourceItem.ID;
			item.Categories = sourceItem.Categories;
			item.ArmorClassValue = sourceItem.ArmorClassValue;
			item.EquipmentSlots = sourceItem.EquipmentSlots;
			item.WornEffects = sourceItem.WornEffects;
			item.Conditions = sourceItem.Conditions;
			Items.Add(item);
		}
	}
	
	public string GetArmor
	{
		get
		{
			string result = string.Empty;
			foreach(IItem item in Items)
			{
				RPGArmor i = (RPGArmor)item;
				result += i.ArmorClassValue + ",";
			}
			return result;
		}
	}
	
	protected override void PrepareCollection(IItem i)
	{
		Items = new List<IItem>();
		SourceItem = (RPGArmor)i;
		sourceItem = (RPGArmor)i;
	}
	
	protected override void GenerateAnother(IItem i)
	{
		RPGArmor armor = (RPGArmor)i;
		//generating weapon damage
		if (ArmorClassValue)
		{
			int amor = armor.ArmorClassValue;
			for(int x = 0; x <= ItemsCount; x++)
			{
				RPGArmor w = (RPGArmor)Items[x];
				
				if (ArmorIncreasingDamageType == IncresingTypeEnum.Linear)
				{
					amor = amor + (int)(w.ArmorClassValue * ArmorAmount / 100);
				}
				else
				{
					amor = (int)(amor * ArmorAmount);
				}
				w.ArmorClassValue = amor;
			}
		}
	}
}
