using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlInclude(typeof(Condition))]
public class BaseLootCategory 
{
	public RPGItemCategory Category;
	public List<Condition> Conditions;
    public int MinStackAmount = 1;
    public int MaxStackAmount = 1;
	
	[XmlIgnore]
	protected List<ItemInWorld> Items;

    [XmlIgnore]
    public Player player;
	
	public BaseLootCategory()
	{
		Conditions = new List<Condition>();
		Category = new RPGItemCategory();
		Items = new List<ItemInWorld>();
	}

	
	protected void AddItem(RPGItem item, List<ItemInWorld> collection)
	{
		foreach(ItemInWorld i in Items)
		{
			if (i.rpgItem.UniqueId == item.UniqueId)
				return;
		}
		
		ItemInWorld itemInWorld = new ItemInWorld();
		itemInWorld.UniqueItemId = item.UniqueId;
		itemInWorld.rpgItem = item;
		if (item.Stackable)
			itemInWorld.CurrentAmount = Random.Range(MinStackAmount, MaxStackAmount);
		else
			itemInWorld.CurrentAmount = 1;
		collection.Add(itemInWorld);
	}
	
	//add items from collection to one collection according level
	public void AddItemsByLevel(List<ItemInWorld> collection)
	{
		foreach(RPGItem item in Player.Data.Items)
		{
			foreach(RPGItemCategory c in item.Categories)
			{
				if (c.ID == Category.ID)
				{
                    item.LoadIcon();
					AddItem(item, collection);
					break;
				}
			}
		}

        foreach (RPGWeapon item in Player.Data.Weapons)
		{
			foreach(RPGItemCategory c in item.Categories)
			{
				if (c.ID == Category.ID)
				{
                    item.LoadIcon();
					AddItem(item, collection);
					break;
				}
			}
		}

        foreach (RPGArmor item in Player.Data.Armors)
		{
			foreach(RPGItemCategory c in item.Categories)
			{
				if (c.ID == Category.ID)
				{
                    item.LoadIcon();
					AddItem(item, collection);
					break;
				}
			}
		}
	}
	
	public bool CanYouDisplay
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
}
