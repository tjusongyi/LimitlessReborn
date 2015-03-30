using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

public class ItemInWorld 
{
	public string UniqueItemId;
	public int CurrentAmount;
	public float CurrentDurability;
	public float PriceModifier;
	
	[XmlIgnore]
	public RPGItem rpgItem;
	
	public string Tooltip
	{
		get
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(rpgItem.Name);
			sb.AppendLine();
			sb.AppendLine("Rarity: " + rpgItem.Rarity.ToString());
			if (rpgItem.Value > 0)
			{
				sb.AppendLine();
				if (PriceModifier == 0)
					PriceModifier = 1;
				sb.AppendLine("Price: " + (int)(rpgItem.Value * PriceModifier));
			}
			if (rpgItem.Preffix != PreffixType.ITEM.ToString())
			{
				Equiped equiped = (Equiped)rpgItem;
				if (equiped.Durability > 0)
				{
					sb.AppendLine();
					sb.AppendLine("Durability: " + CurrentDurability + " / " + equiped.Durability);
				}
			}
			//sb.AppendLine(rpgItem.GetAdditionalInfo());
			return sb.ToString();
		}
	}
	
	public bool IsItemLoaded()
	{
		if (!string.IsNullOrEmpty(UniqueItemId) && rpgItem == null)
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
		}
		
		if (rpgItem != null && !string.IsNullOrEmpty(rpgItem.Name))
		{
			rpgItem.LoadIcon();
			return true;
		}
		return false;
	}
	
	public bool IsItemLoaded(float priceModifier)
	{
		if (!string.IsNullOrEmpty(UniqueItemId) && rpgItem != null && !string.IsNullOrEmpty(rpgItem.Name))
		{
			rpgItem.LoadIcon();
			PriceModifier = priceModifier;
			return true;
		}
		
		return false;
	}
}
