using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotBar
{
	public List<HotBarItem> Items;
	public int Size = 10;
	
	public HotBar()
	{
		Items = new List<HotBarItem>();	
	}
	
	public bool EquipUsable(UsableItem item, int Index)
	{
		if (!item.IsUsable)
			return false;
		
		HotBarItem hotBarItem = new HotBarItem();
		hotBarItem.Index = Index;
		hotBarItem.Usable = item;
		Items.Add(hotBarItem);
		
		return true;
	}
	
	public HotBarItem GetByPosition(int itemIndex)
	{
		foreach(HotBarItem h in Items)
		{
			if (h.Index == itemIndex)
			    return h;
		}
		return new HotBarItem();
	}
	
	public void RemoveUsable(int itemIndex)
	{
		foreach(HotBarItem barItem in Items)
		{
			if (barItem.Index == itemIndex)
			{
				Items.Remove(barItem);
				break;
			}
		}
	}
}

public class HotBarItem
{
	public int Index;
	public UsableItem Usable;
	
	public HotBarItem()
	{
		Usable = new UsableItem();
		Index = -1;
	}
}
