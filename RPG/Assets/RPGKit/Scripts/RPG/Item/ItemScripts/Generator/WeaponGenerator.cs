using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponGenerator : ItemGenerator
{
	public bool WeaponDamage;
	public IncresingTypeEnum WeaponIncreasingDamageType;
	public int WeaponAmount;
	
	public WeaponGenerator() : base()
	{
		
		WeaponDamage = GeneratorConfig.WeaponDamage;
		WeaponIncreasingDamageType = GeneratorConfig.WeaponIncreasingDamageType;
		WeaponAmount = GeneratorConfig.WeaponAmount;
	}
	
	protected override void FillItems ()
	{
		for(int x = 1; x <= ItemsCount; x++)
		{
			RPGWeapon sourceItem = (RPGWeapon)SourceItem;
			RPGWeapon item = new RPGWeapon();
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
			item.MinimumDmg = sourceItem.MinimumDmg;
			item.MaximumDmg = sourceItem.MaximumDmg;
			item.MinimumRange = sourceItem.MinimumRange;
			item.MaximumRange = sourceItem.MaximumRange;
			item.IsAmmo = sourceItem.IsAmmo;
			item.NeedAmmo = sourceItem.NeedAmmo;
			item.WeaponType = sourceItem.WeaponType;
			item.Attack = sourceItem.Attack;
			item.AttackSpeed = sourceItem.AttackSpeed;
			item.WornEffects = sourceItem.WornEffects;
			item.EquipmentSlots = sourceItem.EquipmentSlots;
			item.Conditions = sourceItem.Conditions;
			Items.Add(item);
		}
	}
	
	protected override void PrepareCollection(IItem i)
	{
		Items = new List<IItem>();
		SourceItem = (RPGWeapon)i;
		sourceItem = (RPGWeapon)i;
	}
	
	public string GetDamage
	{
		get
		{
			string result = string.Empty;
			foreach(IItem item in Items)
			{
				RPGWeapon i = (RPGWeapon)item;
				result += i.MinimumDmg + "-" + i.MaximumDmg + ",";
			}
			return result;
		}
	}
	
	protected override void GenerateAnother(IItem i)
	{
		RPGWeapon weapon = (RPGWeapon)i;
		//generating weapon damage
		if (WeaponDamage)
		{
			int minDmg = weapon.MinimumDmg;
			int maxDmg = weapon.MaximumDmg;
			for(int x = 0; x <= ItemsCount; x++)
			{
				RPGWeapon w = (RPGWeapon)Items[x];
				
				if (WeaponIncreasingDamageType == IncresingTypeEnum.Linear)
				{
					minDmg = minDmg + (int)(w.MinimumDmg * WeaponAmount / 100);
					maxDmg = maxDmg + (int)(w.MaximumDmg * WeaponAmount / 100);
				}
				else
				{
					minDmg = (int)(minDmg * WeaponAmount);
					maxDmg = (int)(maxDmg * WeaponAmount);
				}
				w.MinimumDmg = minDmg;
				w.MaximumDmg = maxDmg;
			}
		}
	}
}
