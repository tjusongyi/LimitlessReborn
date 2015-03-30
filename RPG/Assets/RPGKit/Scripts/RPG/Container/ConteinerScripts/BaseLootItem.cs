using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//used for shops, enemy loots, items
[XmlInclude(typeof(Condition))]
public class BaseLootItem
{
	public int StackAmount = 1;
	public ItemTypeEnum Preffix;
	public int ID;
	public List<Condition> Conditions;

    [XmlIgnore]
    public Player player;
	
	public BaseLootItem()
	{
		Conditions = new List<Condition>();
	}
	
	//if player can loot this item
	public bool CanYouLoot
	{
		get
		{
			foreach(Condition condition in Conditions)
			{
                if (condition.Validate(player) == false)
					return false;
			}
			return true;
		}
	}
	
	public void AddOneItem(List<ItemInWorld> BaseLootItems)
	{
		//validate conditions
		if (!CanYouLoot)
			return;
		
		if (Preffix == ItemTypeEnum.ITEM)
		{
			foreach(RPGItem item in Player.Data.Items)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}
		
		if (Preffix == ItemTypeEnum.WEAPON)
		{
			foreach(RPGWeapon item in Player.Data.Weapons)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}
		
		if (Preffix == ItemTypeEnum.ARMOR)
		{
			foreach(RPGArmor item in Player.Data.Armors)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}
	}
	
	
	protected void AddItem(RPGItem item, List<ItemInWorld> BaseLootItems)
	{
		foreach(ItemInWorld i in BaseLootItems)
		{
			if (i.rpgItem.UniqueId == item.UniqueId)
				return;
		}
		
		ItemInWorld itemInWorld = new ItemInWorld();
		itemInWorld.rpgItem = item;
		itemInWorld.UniqueItemId = item.UniqueId;
		if (item.Stackable)
			itemInWorld.CurrentAmount = StackAmount;
		else
			itemInWorld.CurrentAmount = 1;
		BaseLootItems.Add(itemInWorld);
	}
}
