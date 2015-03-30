using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class BasicInventory
{
	public BasicInventory()
	{
		Items = new List<InventoryItem>();	
	}
	
	public int sizeXOfInventory
	{
		get
		{
			return maximumItems / sizeYOfInventory;
		}
	}
	
	public List<InventoryItem> Items;
	public int maximumItems;
	public int sizeYOfInventory = 8;
	
	public bool DoYouHaveThisItem(string itemToHave, int amountToReach)
	{
		int numberItemsInInventory = 0;
		foreach(ItemInWorld itemInInventory in Items)
		{
			if (itemToHave == itemInInventory.rpgItem.UniqueId)
			{
				numberItemsInInventory += itemInInventory.CurrentAmount;
			}
		}
		if (numberItemsInInventory >= amountToReach)
			return true;
		
		return false;
	}
	
	public bool DoYouHaveThisItem(string itemToHave)
	{
		return DoYouHaveThisItem(itemToHave, 1);
	}
	
	private int GetFirstUnusedPosition()
	{
		if (Items.Count == 0)
			return 0;
		
		for (int index = 1; index <= maximumItems; index++)
		{
			bool result = true;
			foreach(InventoryItem item in Items)
			{
				if (item.ItemInventoryIndex == index)
				{
					result = false;
				}
			}
			if (result)
			{
				return index;
			}
		}
		return -1;
	}
	
	protected void AddInventoryItem(RPGItem itemToAdd, int amount)
	{
		InventoryItem item = new InventoryItem();
		item.ItemInventoryIndex = GetFirstUnusedPosition();
		item.rpgItem = itemToAdd;
		item.CurrentAmount = amount;
		item.CurrentDurability = itemToAdd.CurrentDurability;
		item.UniqueItemId = itemToAdd.UniqueId;
		Items.Add(item);
	}
	
	public bool DoYouHaveSpaceForThisItem(RPGItem itemToHave, int amountToReach)
	{
		if (itemToHave == null)
			return false;
		int size = amountToReach;
		if (itemToHave.Stackable)
		{
			foreach(InventoryItem item in Items)
			{
				if (item.rpgItem.UniqueId != itemToHave.UniqueId)
					 continue;
				if (item.rpgItem.MaximumStack == item.CurrentAmount)
					continue;
				size = item.rpgItem.MaximumStack - item.CurrentAmount;
			}
			if (size <= 0)
				return true;
			int freeSpaces = maximumItems - Items.Count;
			if (freeSpaces + itemToHave.MaximumStack > size)
				return true;
			else
				return false;
		}
		else
		{
			if (Items.Count <= maximumItems + size)
				return true;
			else
				return false;
		}
	}
	
	protected virtual void FinalizeInventoryOperation(Player player)
	{
		foreach(InventoryItem items in Items)
		{
			items.CurrentDurability = items.rpgItem.CurrentDurability;
		}
	}

    public bool AddItem(RPGItem itemToAdd, Player player)
	{
		return AddItem(itemToAdd, 1, player);
	}
	
	public bool AddItem(RPGItem itemToAdd, int amountToReach, Player player)
	{
		if (!DoYouHaveSpaceForThisItem(itemToAdd, amountToReach))
		    return false;
		if (itemToAdd.Stackable)
		{
			foreach(InventoryItem item in Items)
			{
				if (item.rpgItem.UniqueId != itemToAdd.UniqueId)
					 continue;
				if (item.rpgItem.MaximumStack == item.CurrentAmount)
					continue;
				if (item.rpgItem.MaximumStack - item.CurrentAmount >= amountToReach)
				{
					item.CurrentAmount += amountToReach;
					return true;
				}
				else
				{
					amountToReach = item.rpgItem.MaximumStack - item.CurrentAmount;
					item.CurrentAmount = item.rpgItem.MaximumStack;
				}
			}
			if (amountToReach <= 0)
				return true;
			
			do
			{
				if (amountToReach >= itemToAdd.MaximumStack)
				{
					AddInventoryItem(itemToAdd, itemToAdd.MaximumStack);
					amountToReach = amountToReach - itemToAdd.MaximumStack;
				}
				else
				{
					AddInventoryItem(itemToAdd, amountToReach);
					amountToReach = 0;
				}
			}
			while (amountToReach > 0);
		}
		else
		{
			for(int index = 1; index <= amountToReach; index++)
			{
				AddInventoryItem(itemToAdd, 1);
			}
		}
		FinalizeInventoryOperation(player);
		return true;
	}
	
	public bool RemoveItem(int ID, string preffix, int amountToRemove, Player player)
	{
		RPGItem item = new RPGItem();
		
		if (preffix == PreffixType.ARMOR.ToString())
			item = Storage.LoadById<RPGArmor>(ID, new RPGArmor());
		
		if (preffix == PreffixType.WEAPON.ToString())
			item = Storage.LoadById<RPGWeapon>(ID, new RPGWeapon());
		
		if (preffix == PreffixType.ITEM.ToString())
			item = Storage.LoadById<RPGItem>(ID, new RPGItem());

        return RemoveItem(item, amountToRemove, player);
	}
	
	public bool RemoveItem(RPGItem itemToRemove, Player player)
	{
		return RemoveItem(itemToRemove, 1, player);
	} 
	
	public bool RemoveItem(RPGItem itemToRemove, int amountToRemove, Player player)
	{
		if (itemToRemove.ID == 0)
			return true;
		
		if (!DoYouHaveThisItem(itemToRemove.UniqueId, amountToRemove))
			return false;
		
		if (itemToRemove.Stackable)
		{
			int itemsToRemove = 0;
			foreach(ItemInWorld item in Items)
			{
				if (item.rpgItem.UniqueId != itemToRemove.UniqueId)
					continue;
				if (item.CurrentAmount > amountToRemove)
				{
					item.CurrentAmount -= amountToRemove;
					amountToRemove = 0;
				}
				else if (item.CurrentAmount < amountToRemove)
				{
					amountToRemove -= item.CurrentAmount;
					item.CurrentAmount = 0;
					itemsToRemove++;
				}
				else
				{
					amountToRemove = 0;
					item.CurrentAmount = 0;
					itemsToRemove++;
				}
			}
			CleanInventory(itemsToRemove);
		}
		else
		{
			for(int x = 1; x <= amountToRemove; x ++)
			{
				foreach(InventoryItem item in Items)
				{
					if (itemToRemove.UniqueId == item.rpgItem.UniqueId && item.CurrentAmount == 1)
					{
						Items.Remove(item);
						break;
					}
				}
			}
		}
        FinalizeInventoryOperation(player);
			
		return true;
	}
	
	/// <summary>
	/// Clean inventory from items that have current amount 0 
	/// </summary>
	private void CleanInventory(int itemsToRemove)
	{
		for(int x = 1; x <= itemsToRemove; x++)
		{
			foreach(InventoryItem item in Items)
			{
				if (item.CurrentAmount == 0)
				{
					Items.Remove(item);
					break;
				}
			}
		}
	}
	
	public int CountItem(string uniqueId)
	{
		int count = 0;
		foreach(InventoryItem item in Items)
		{
			if (item.rpgItem.UniqueId == uniqueId)
				count += item.CurrentAmount;
		}
		return count;
	}
	
	[XmlIgnore]
	public bool IsFullInventory
	{
		get
		{
			if (maximumItems == Items.Count)
				return true;
			else
				return false;
		}
	}
	
	private int Index(int x, int y)
	{
		return (y * sizeXOfInventory) + x;
	}
	
	public InventoryItem GetByPosition(int x, int y)
	{
		int index = Index(x, y);
		foreach(InventoryItem item in Items)
		{
			if (item.ItemInventoryIndex == index)
				return item;
		}
		return new InventoryItem();
	}
	
	public bool MoveItem(InventoryItem item, int x, int y)
	{
		if (!string.IsNullOrEmpty(GetByPosition(x, y).UniqueItemId))
			return false;
		
		int index = Index(x, y);
		item.ItemInventoryIndex = index;
		return true;
	}
}
