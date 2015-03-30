using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlInclude(typeof(ShopCategory))]
public class ShopCategory : BaseLootCategory
{
	public ShopCategory() : base()
	{
	}
	
	public void GetItems(List<ItemInWorld> shopItems, Player player)
	{
		//validate conditions
		if (!CanYouDisplay)
			return;
		
		
		AddItemsByLevel(shopItems);
	}
}

public enum LevelAdjustmentType
{
	Floating = 0,
	Fixed = 1
}
