using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

public class ContainerCategory : BaseLootCategory
{
	public float ChanceToFind = 100;
	
	//temp items for getting one item
	[XmlIgnore]
	public List<ItemInWorld> tempItems = new List<ItemInWorld>();

    public ContainerCategory()
    {
        Conditions = new List<Condition>();
        Category = new RPGItemCategory();
        Items = new List<ItemInWorld>();
        tempItems = new List<ItemInWorld>();
    }

	public void GetItem(List<ItemInWorld> ContainerItems,int Level)
	{
		//validate conditions
		if (!CanYouDisplay)
			return;
		
		//calculate chance
		if (BasicRandom.Instance.Next(10000) > (int)(ChanceToFind * 100))
			return;
		
		//add items to one collection
        AddItemsByLevel(tempItems);
		//items count
		int itemsCount = tempItems.Count;
		if (itemsCount == 0)
			return;
		//calculate chance that one item will be found
		int itemIndex = BasicRandom.Instance.Next(itemsCount - 1); 
		//add item from collection
		ContainerItems.Add(tempItems[itemIndex]);
	}
}
