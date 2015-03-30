using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Inventory  : BasicInventory
{
   

	public Inventory() : base()
	{
		maximumItems = 40;
	}

	public bool DoYouHaveSpecForTheseItems(List<Equiped> items)
	{
		int size = 0;
		foreach(Equiped equiped in items)
		{
			if (equiped.Stackable)
			{
				bool founded = false;
				foreach(InventoryItem item in Items)
				{
					if (item.UniqueItemId == equiped.UniqueId && item.CurrentAmount < item.rpgItem.MaximumStack)
						founded = true;
				}
				if (!founded)
					size++;
			}
			else
				size++;
		}
		if (maximumItems < Items.Count + size)
			return false;
		else
			return true;
	}
	
	public void DropItem(InventoryItem item, Player player)
	{
		foreach(InventoryItem i in Items)
		{
			if (item.ItemInventoryIndex == i.ItemInventoryIndex)
			{
				Items.Remove(i);
				FinalizeInventoryOperation(player);
				break;
			}
		}
	}
	
	protected override void FinalizeInventoryOperation(Player player)
	{
        player.Hero.Quest.CheckInventoryItem(player);
	}
	
	public bool EquipItem(InventoryItem item, Player player)
	{
		if (!item.IsItemEquiped)
			return false;
		
		if (!player.Hero.Settings.EquipedItemInInventory)
		{
			foreach(InventoryItem i in Items)
			{
				if (item == i)
				{
					Items.Remove(i);
					break;
				}
			}
		}

        player.Hero.Equip.EquipItem(item, player);
		
		return true;
	}
}

