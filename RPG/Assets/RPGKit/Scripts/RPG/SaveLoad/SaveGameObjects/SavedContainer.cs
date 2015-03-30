using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedContainer : BasicSaveEntity
{
	public List<ItemInWorld> Items;
	
	public void StoreItems(List<ItemInWorld> items)
	{
		Items = new List<ItemInWorld>();
		foreach(ItemInWorld item in items)
		{
			ItemInWorld i = new ItemInWorld();
			i.CurrentAmount = item.CurrentAmount;
			i.UniqueItemId = item.UniqueItemId;
			i.CurrentDurability = item.CurrentDurability;
			Items.Add(i);
		}
	}
}
