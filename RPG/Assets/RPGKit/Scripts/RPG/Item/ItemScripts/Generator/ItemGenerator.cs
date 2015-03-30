using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGenerator
{
	public int Frequency;
	public int MaximumLevel;
	public bool FirstItemUseCustomName;
	public bool UseNames;
	
	public bool Price;
	public IncresingTypeEnum PriceType;
	public float IncreasingPrice;
	
	public bool Effects;
	public IncresingTypeEnum EffectType;
	public float IncreasingEffect;
	
	public List<IItem> Items;
	
	public string Prices;
	public string Names;
	
	protected int ItemsCount;
	protected IItem SourceItem;
	protected RPGItem sourceItem;
	
	public bool IsGenerated = false;
	
	private List<Effect> effects;
	
	public ItemGenerator()
	{
		Frequency = GeneratorConfig.Frequency;
		MaximumLevel = GeneratorConfig.MaximumLevel;
		FirstItemUseCustomName = GeneratorConfig.FirstItemUseCustomName;
		UseNames = GeneratorConfig.UseNames;
		
		Price = GeneratorConfig.Price;
		PriceType = GeneratorConfig.ValueType;
		IncreasingPrice = GeneratorConfig.IncreasingPrice;
		
		Effects = GeneratorConfig.Effects;
		EffectType = GeneratorConfig.EffectType;
		IncreasingEffect = GeneratorConfig.IncreasingEffect;
		
		effects = new List<Effect>();
	}
	
	protected virtual void FillItems()
	{
		for(int x = 1; x <= ItemsCount; x++)
		{
			RPGItem sourceItem = (RPGItem)SourceItem;
			RPGItem item = new RPGItem();
			item.Stackable = sourceItem.Stackable;
			item.MaximumStack = sourceItem.MaximumStack;
			item.Value = sourceItem.Value;
			item.Rarity = sourceItem.Rarity;
			item.NumberCharges = sourceItem.NumberCharges;
			item.IconPath = sourceItem.IconPath;
			item.PrefabName = sourceItem.PrefabName;
			
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
			Items.Add(item);
		}
	}
	
	protected virtual void PrepareCollection(IItem i)
	{
		Items = new List<IItem>();
		SourceItem = (RPGItem)i;
		sourceItem = (RPGItem)i;
	}
	
	public bool Calculate(IItem i)
	{
		PrepareCollection(i);
		
		foreach(Effect effect in sourceItem.Effects)
		{
			Effect e = new Effect();
			e.Duration = effect.Duration;
			e.EffectType = effect.EffectType;
			e.Target = effect.Target;
			e.Value = effect.Value;
			effects.Add(e);
		}
		
		ItemsCount = (MaximumLevel - sourceItem.LevelItem + 1) / Frequency;
		
		FillItems();
		
		ItemsCount--;
		
		for(int x = 0; x <= ItemsCount; x++)
		{
			RPGItem rpgItem = (RPGItem)Items[x];
			rpgItem.LevelItem = (x + 1) * Frequency;
		}
		
		//generating price
		if (Price)
		{
			int price = sourceItem.Value;
			for(int x = 0; x <= ItemsCount; x++)
			{
				RPGItem rpgItem = (RPGItem)Items[x];
				
				if (PriceType == IncresingTypeEnum.Linear)
					price = price + (int)(rpgItem.Value * IncreasingPrice / 100);
				else
					price = (int)(price * IncreasingPrice);
				
				rpgItem.Value = price;
			}
		}
		
		//generating effects
		if (Effects)
		{
			foreach(Effect effect in effects)
			{
				int tempValue = effect.Value;
				for(int x = 0; x <= ItemsCount; x++)
				{
					RPGItem rpgItem = (RPGItem)Items[x];
					
					foreach(Effect e in rpgItem.Effects)
					{
						if (e.EffectType != effect.EffectType && e.Duration != effect.Duration && e.Target != effect.Target)
							continue;
						if (EffectType == IncresingTypeEnum.Linear)
							tempValue +=  (int)(e.Value * IncreasingPrice / 100);
						else
							tempValue = (int)(tempValue * IncreasingPrice);
						e.Value = tempValue;
					}	
				}
			}
		}
		
		//generating names
		if (UseNames)
		{
			for(int x = 0; x <= ItemsCount; x++)
			{
				RPGItem rpgItem = (RPGItem)Items[x];
				rpgItem.Name = string.Format(GeneratorConfig.WordsMask[x], GeneratorConfig.Words[x], SourceItem.Name);
			}
		}
		
		GenerateAnother(i);
		IsGenerated = true;
		return true;
	}
	
	public string GetPrices
	{
		get
		{
			string result = string.Empty;
			foreach(IItem item in Items)
			{
				RPGItem i = (RPGItem)item;
				result += i.Value + ",";
			}
			return result;
		}
	}
	
	public string GetNames
	{
		get
		{
			string result = string.Empty;
			foreach(IItem item in Items)
			{
				RPGItem i = (RPGItem)item;
				result += i.Name + ",";
			}
			return result;
		}
	}
	
	protected virtual void GenerateAnother(IItem i)
	{
		
	}
}

public enum IncresingTypeEnum
{
	Linear = 0,
	Exponential = 1
}
