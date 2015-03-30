using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerItem : BaseLootItem
{
	public float ChanceToFind = 100;
	
	public void GetItem(List<ItemInWorld> ContainerItems)
	{
		//calculate chance
		if (BasicRandom.Instance.Next(10000) > (int)(ChanceToFind * 100))
			return;
		
		AddOneItem(ContainerItems);
	}
}
