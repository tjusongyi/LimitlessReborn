using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class RPGContainer : BasicItem
{
	public List<ContainerCategory> Categories;
	public List<ContainerItem> ContainerItems;

    public List<Condition> Conditions;
    public List<ActionEvent> Events;

    public List<Effect> OpenEffects;
    public List<Condition> AvoidConditions;
	
	public bool DestroyEmpty;
	public bool SharedStash;
	public bool OnlyLoot;
	public int maximumItems;
	//for "container" that will be enemy body
	public bool IsEnemyContainer;
	public int Level;
	
	private int sizeXOfInventory = 10;
	
	[XmlIgnore]
	public List<ItemInWorld> Items = new List<ItemInWorld>();
	
	public RPGContainer()
	{
		Categories = new List<ContainerCategory>();
        Events = new List<ActionEvent>();
		ContainerItems = new List<ContainerItem>();
		Items = new List<ItemInWorld>();
        Conditions = new List<Condition>();
		DestroyEmpty = true;
		SharedStash = false;
		maximumItems = 30;
        AvoidConditions = new List<Condition>();
        preffix = "CONTAINER";
	}

    public bool CanOpen(Player player)
    {
        foreach (Condition condition in Conditions)
        {
            if (!condition.Validate(player))
                return false;
        }
        return true;
    }

    public bool IsEffect(Player player)
    {
        if (AvoidConditions.Count == 0)
            return true;

        bool result = false;
        foreach (Condition condition in AvoidConditions)
        {
            if (!condition.Validate(player))
                result = true;
        }
        return result;
    }

    public void DoEvents(Player player)
    {
        foreach (ActionEvent e in Events)
        {
            e.DoAction(player);   
        }
    }

	public void LoadInitialize()
	{
		List<RPGItem> rpgItems = Storage.Load<RPGItem>(new RPGItem());
		List<RPGArmor> rpgArmors = Storage.Load<RPGArmor>(new RPGArmor());
		List<RPGWeapon> rpgWeapons = Storage.Load<RPGWeapon>(new RPGWeapon());
		foreach(ItemInWorld itemInWorld in Items)
		{
			foreach(RPGItem i in rpgItems)
			{
				if (i.UniqueId == itemInWorld.UniqueItemId)
				{
					itemInWorld.rpgItem = i;
					break;
				}
			}
			foreach(RPGWeapon i in rpgWeapons)
			{
				if (i.UniqueId == itemInWorld.UniqueItemId)
				{
					itemInWorld.rpgItem = i;
					break;
				}
			}
			foreach(RPGArmor i in rpgArmors)
			{
				if (i.UniqueId == itemInWorld.UniqueItemId)
				{
					itemInWorld.rpgItem = i;
					break;
				}
			}
		}
	}
	
	public void Initialize(Player player)
	{
		int targetLevel = player.Hero.CurrentLevel;
		if (IsEnemyContainer)
			targetLevel = Level;
		
		//get container category
		foreach(ContainerCategory cc in Categories)
			cc.GetItem(Items, targetLevel);
		
		//get container items
		foreach(ContainerItem cc in ContainerItems)
			cc.GetItem(Items);
	}

    public bool TakeAll(Player player)
	{
		if (Items.Count == 0)
			return true;
		do
		{
			if (!player.Hero.Inventory.DoYouHaveSpaceForThisItem(Items[0].rpgItem, Items[0].CurrentAmount))
				return false;
            TakeItem(Items[0].rpgItem, Items[0].CurrentAmount, player);
		}
		while (Items.Count > 0);
		//finish container operation
        FinalizeOperation(player);	
		if (Items.Count == 0)
			return true;
		else
			return false;
	}

    public bool TakeItem(RPGItem rpgItem, Player player)
	{
		return TakeItem(rpgItem, 1, player);
	}

    public bool TakeItem(RPGItem rpgItem, int Amount, Player player)
	{
		if (!player.Hero.Inventory.DoYouHaveSpaceForThisItem(rpgItem, Amount))
			return false;
			
		//add item to inventory
		player.Hero.Inventory.AddItem(rpgItem, Amount, player);
			
		//remove item from current container collection collection
		foreach(ItemInWorld containerItem in Items)
		{
			if (containerItem.rpgItem.UniqueId == rpgItem.UniqueId)
			{
				containerItem.CurrentAmount = containerItem.CurrentAmount - Amount;
				if (containerItem.CurrentAmount == 0)
				{
					Items.Remove(containerItem);
				}
				break;
			}
		}
		//finish container operation
		FinalizeOperation(player);
		return true;
	}
	
	private int Index(int x, int y)
	{
		return ((y-1) * sizeXOfInventory) + x;
	}
	
	public bool DropItemFromInventory(RPGItem item, Player player)
	{
		if (IsContainerFull || OnlyLoot)
			return false;
        return DropItemFromInventory(item, 1, player);
	}
	
	public bool DropItemFromInventory(RPGItem itemToAdd, int Amount, Player player)
	{
		if (OnlyLoot)
			return false;
		
		if (!DoYouHaveSpaceForThisItem(itemToAdd, Amount))
		    return false;
		if (itemToAdd.Stackable)
		{
			foreach(ItemInWorld item in Items)
			{
				if (item.rpgItem.UniqueId != itemToAdd.UniqueId)
					 continue;
				if (item.rpgItem.MaximumStack == item.CurrentAmount)
					continue;
				if (item.rpgItem.MaximumStack - item.CurrentAmount >= Amount)
				{
					item.CurrentAmount += Amount;
					return true;
				}
				else
				{
					Amount = item.rpgItem.MaximumStack - item.CurrentAmount;
					item.CurrentAmount = item.rpgItem.MaximumStack;
				}
			}
			if (Amount <= 0)
				return true;
			
			do
			{
				if (Amount >= itemToAdd.MaximumStack)
				{
                    AddItem(itemToAdd, itemToAdd.MaximumStack, player);
					Amount = Amount - itemToAdd.MaximumStack;
				}
				else
				{
                    AddItem(itemToAdd, Amount, player);
					Amount = 0;
				}
			}
			while (Amount > 0);
		}
		else
		{
			for(int index = 1; index <= Amount; index++)
			{
                AddItem(itemToAdd, 1, player);
			}
		}
		
		return true;
	}
	
	private void FinalizeOperation(Player player)
	{
		bool founded = false;
		foreach(RPGContainer c in player.Hero.SharedContainers)
		{
			if (c.ID == ID)
			{
				c.Items = Items;
				founded = true;
			}
		}
		if (!founded)
			player.Hero.SharedContainers.Add(this);
	}

    private void AddItem(RPGItem itemToAdd, int amount, Player player)
	{
		ItemInWorld item = new InventoryItem();
		item.rpgItem = itemToAdd;
		item.CurrentAmount = amount;
		item.UniqueItemId = itemToAdd.UniqueId;
		Items.Add(item);
		
		//finish container operation
        FinalizeOperation(player);
	}
	
	private bool DoYouHaveSpaceForThisItem(RPGItem itemToHave, int Amount)
	{
		int size = Amount;
		if (itemToHave.Stackable)
		{
			foreach(ItemInWorld item in Items)
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
	
	public ItemInWorld GetByPosition(int x, int y)
	{
		int index = Index(x, y);
		int countItem = 1;
		foreach(ItemInWorld item in Items)
		{
			if (countItem == index)
				return item;
			countItem++;
		}
		return new ItemInWorld();
	}
	
	[XmlIgnore]
	public bool IsContainerFull
	{
		get
		{
			if (Items.Count == maximumItems)
				return true;
			return false;
		}
	}
}
