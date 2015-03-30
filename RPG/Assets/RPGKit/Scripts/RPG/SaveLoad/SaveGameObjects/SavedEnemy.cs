using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SavedEnemy : BasicSaveEntity
{
	public int CurrentHp;
	public List<ItemInWorld> Items;
	
	public SavedEnemy()
	{
		Items = new List<ItemInWorld>();
	}
}
