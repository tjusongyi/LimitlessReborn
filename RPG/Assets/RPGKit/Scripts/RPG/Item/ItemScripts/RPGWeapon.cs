using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[Serializable]
[XmlInclude(typeof(Effect))]
public class RPGWeapon : Equiped 
{
	public float AttackSpeed;
	public WeaponCombatSkillType Attack;
	public int MinimumDmg;
	public int MaximumDmg;
	public int MaximumRange;
	public int MinimumRange;
	public bool IsAmmo;
	public bool NeedAmmo;
	public WeaponCategoryType WeaponType;
	public List<Effect> EffectsHit;
	
	public RPGWeapon()
	{
		preffix = "WEAPON";
		MaximumRange = 3;
		FBXName = "Weapons/";
		IconPath = "Icon/";
		Name = string.Empty;
		EffectsHit = new List<Effect>();
	}
	
	[XmlIgnore]
	public int WeaponTooltipRows
	{
		get
		{
			int result = 3;
			result += EquipedTooltipRows;
			
			
			if (NeedAmmo)
			{
				result++;
			}
			
			result += EffectsHit.Count;
			if (EffectsHit.Count > 0)
				result+=2;
			return result;
		}
	}
	
	protected override int TooltipTotalRows
	{
		get
		{
			return WeaponTooltipRows;
		}
	}
	
	protected override void ItemToolTip(float x, float y, float height, GUISkin skin, ItemInWorld item, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
		y = BasicToolTipInfo(x, y, height, skin, item, shop, isInventoryToolTip, priceModifier, player);
		
		y += 10;
		
		Rect rect = new Rect(x + 10, y,340, 20);
		string damage = "Damage: " + MinimumDmg;
		if (MaximumDmg != MinimumDmg && MaximumDmg > 0)
		{
			damage += "-" + MaximumDmg;
		}
		GUI.Label(rect, damage, skin.label);

        y += player.Hero.Settings.TooltipRowSize;
		rect = new Rect(x + 10, y,340, 20);
		GUI.Label(rect, "Speed: " + AttackSpeed, skin.label);

        y += player.Hero.Settings.TooltipRowSize;
		rect = new Rect(x + 10, y,340, 20);
		GUI.Label(rect, "Type: " + WeaponType.ToString(), skin.label);

        y += player.Hero.Settings.TooltipRowSize;
		rect = new Rect(x + 10, y,340, 20);
		GUI.Label(rect, "Max range: " + MaximumRange.ToString(), skin.label);

        y += player.Hero.Settings.TooltipRowSize;
		if (EffectsHit.Count > 0)
		{
			y = EffectsHit[0].TooltipPart(x, y, skin, "Effect on hit", EffectsHit, player);
		}
		
		if (WornEffects.Count > 0)
		{
            y = WornEffects[0].TooltipPart(x, y, skin, "Equip bonus", WornEffects, player);
		}

        UsableItemTooltip(x, y, height, skin, item.rpgItem, player);
	}
}


public enum WeaponCategoryType
{
	Sword = 0,
	Mace = 1,
	Bow = 2
}

public enum WeaponCombatSkillType
{
	Melee = 0,
	Range = 1
}
