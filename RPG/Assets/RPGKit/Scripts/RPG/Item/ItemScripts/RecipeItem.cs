using UnityEngine;
using System.Collections;

public class RecipeItem
{
	public int ItemID;
	public string ItemPreffix;
	public int Quantity;
	public bool UsedDuringCraft;
	
	public RecipeItem()
	{
		Quantity = 1;
		UsedDuringCraft = false;
	}
}
