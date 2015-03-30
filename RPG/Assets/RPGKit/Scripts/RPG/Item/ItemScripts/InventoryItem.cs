using UnityEngine;
using System;
using System.Collections;

public class InventoryItem : ItemInWorld {

	public int ItemInventoryIndex;
	
	/// <summary>
	/// not used in this version
	/// </summary>
	public InventoryTypeEnum InventoryType; 
	
	public void AttachScript(GameObject go)
	{
		if (rpgItem.Preffix == "WEAPON" && go.GetComponent<Weapon>() == null)
		{
			Weapon weapon = (Weapon)go.AddComponent<Weapon>();
			weapon.ID = this.rpgItem.ID;
		}
		if (rpgItem.Preffix == "ARMOR" && go.GetComponent<Armor>() == null)
		{
			Armor armor = (Armor)go.AddComponent<Armor>();
			armor.ID = this.rpgItem.ID;
		}
		if (rpgItem.Preffix == "ITEM" && go.GetComponent<Item>() == null)
		{
			Item item = (Item)go.AddComponent<Item>();
			item.ID = this.rpgItem.ID;
		}
	}
	
	public void LoadItem()
	{
		if (UniqueItemId.IndexOf("WEAPON") != -1)
		{
			int id = Convert.ToInt32(UniqueItemId.Replace("WEAPON", string.Empty));
			rpgItem = Storage.LoadById<RPGWeapon>(id, new RPGWeapon());
		}
		if (UniqueItemId.IndexOf("ITEM") != -1)
		{
			int id = Convert.ToInt32(UniqueItemId.Replace("ITEM", string.Empty));
			rpgItem = Storage.LoadById<RPGItem>(id, new RPGItem());
		}
		if (UniqueItemId.IndexOf("ARMOR") != -1)
		{
			int id = Convert.ToInt32(UniqueItemId.Replace("ARMOR", string.Empty));
			rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
		}
		rpgItem.CurrentDurability = CurrentDurability;
		rpgItem.Icon = (Texture2D)Resources.Load(rpgItem.IconPath, typeof(Texture2D)); 
	}
	
	public bool IsItemEquiped
	{
		get
		{
			if (UniqueItemId.IndexOf("WEAPON") != -1 || UniqueItemId.IndexOf("ARMOR") != -1)
				return true;
		
			return false;
		}
	}
}

public enum InventoryTypeEnum
{
	Inventory = 0,
	Bank = 1
}
