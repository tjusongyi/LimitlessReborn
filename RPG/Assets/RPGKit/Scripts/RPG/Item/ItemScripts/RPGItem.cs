using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlInclude(typeof(Effect))]
[XmlInclude(typeof(RPGItemCategory))]
[Serializable]
public class RPGItem : UsableItem
{
	public RPGItem()
	{
		Categories = new List<RPGItemCategory>();
		Destroyable = true;
		Droppable = false;
		Name = string.Empty;
		preffix = "ITEM";
		IconPath = "Icon/";
	}
	
	public bool Stackable;
	public int MaximumStack;
	public bool Destroyable;
	public int LevelItem;
	public int Value;
	public RarityType Rarity;
	public int NumberCharges;
	public string PrefabName;
	public bool Droppable;
	public float CurrentDurability;
	public List<RPGItemCategory> Categories;
	
	//item generator variables (not used in game, used only in editors)
	public bool IsCopy;
	public int SourceItem;
	
	
	
	public int ItemTooltipRows
	{
		get
		{
			int result = -3;
			
			result += UsableItemTooltipRows;
			//name
			result++;
			
			if (Value > 0)
				result++;
			
			if (Rarity != RarityType.None)
				result++;
			
			if (LevelItem != 0)
				result++;
		
			if (NumberCharges != 0)
				result++;
			
			return result;
		}
	}
	
	protected override int TooltipTotalRows
	{
		get
		{
			return ItemTooltipRows;
		}
	}

    protected override void ItemToolTip(float x, float y, float height, GUISkin skin, ItemInWorld item, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
		y = BasicToolTipInfo(x, y, height, skin, item, shop, isInventoryToolTip, priceModifier, player);
		
		UsableItemTooltip(x, y, height, skin, item.rpgItem, player);
	}

    protected float BasicToolTipInfo(float x, float y, float height, GUISkin skin, ItemInWorld item, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
		Rect rect = new Rect(x, y, 370, height);
		
		GUI.Box(rect,"", skin.box);
		
		rect = new Rect(x + 10, y+ 10,140, 20);
		
		
		string text = Name;
		if (item.CurrentAmount > 1)
			text += " (" + item.CurrentAmount.ToString() + ")";
		
		GUI.Label(rect, text, skin.label);
		
		rect = new Rect(x + 280, y +10,80,80);
		GUI.DrawTexture(rect, item.rpgItem.Icon);
        y += player.Hero.Settings.TooltipRowSize + 10;
		
	
		
		if (Value > 0)
		{
			rect = new Rect(x + 10, y,200, 20);
			GUI.Label(rect, "Value: " + Value.ToString(), skin.label);
            y += player.Hero.Settings.TooltipRowSize;
		}
		
		if (shop)
		{
			float price = Value * priceModifier;
			
			text = "Buy price: " + price;
			if (isInventoryToolTip)
			{
				text = "Sell price: " + price;
			}
			
			rect = new Rect(x + 10, y,200, 20);
			GUI.Label(rect, text, skin.label);
            y += player.Hero.Settings.TooltipRowSize;
		}
		
		if (Rarity != RarityType.None)
		{
			rect = new Rect(x + 10, y,200, 20);
			GUI.Label(rect, Rarity.ToString(), skin.label);
            y += player.Hero.Settings.TooltipRowSize;
		}
		
		return y;
	}
}

public enum RarityType
{
	Worthless = 0,
	Common = 1,
	Magic = 2,
	Rare = 3,
	Epic = 4,
	Legendary = 5,
	None = 6
}
