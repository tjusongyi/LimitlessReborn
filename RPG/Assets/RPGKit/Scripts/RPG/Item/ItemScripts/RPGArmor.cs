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
public class RPGArmor : Equiped 
{
	public int ArmorClassValue;
	public List<Effect> EffectsOnHit;
	
	public RPGArmor()
	{
		preffix = "ARMOR";
		Name = string.Empty;
		FBXName = "Armor/";
		EffectsOnHit = new List<Effect>();
	}
	
	[XmlIgnore]
	public int ArmorTooltipRows
	{
		get
		{
			//armor class value
			int result = -3;
			result += EquipedTooltipRows;
			result += EffectsOnHit.Count;
			
			return result;
		}
	}
	
	protected override int TooltipTotalRows
	{
		get
		{
			return ArmorTooltipRows;
		}
	}
	
	protected override void ItemToolTip(float x, float y, float height, GUISkin skin, ItemInWorld item, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
		y = BasicToolTipInfo(x, y, height, skin, item, shop, isInventoryToolTip, priceModifier, player);
		
		y += 10;
		
		Rect rect = new Rect(x + 10, y,340, 20);
		GUI.Label(rect, "Armor: " + ArmorClassValue, skin.label);

        y += player.Hero.Settings.TooltipRowSize;

        y += player.Hero.Settings.TooltipRowSize;
		if (EffectsOnHit.Count > 0)
		{
            y = EffectsOnHit[0].TooltipPart(x, y, skin, "Effects on hit", EffectsOnHit, player);
		}
		
		if (WornEffects.Count > 0)
		{
            y = WornEffects[0].TooltipPart(x, y, skin, "Equip bonus", WornEffects, player);
		}
		
		UsableItemTooltip(x, y, height, skin, item.rpgItem, player);
	}
}
