using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(RPGEquipmentSlot))]
[XmlInclude(typeof(Effect))]
[XmlInclude(typeof(Condition))]
public class Equiped : RPGItem {
	 
	public int Durability;
	public List<Condition> Conditions;
	public List<Effect> WornEffects;
	public List<RPGEquipmentSlot> EquipmentSlots;
	public string FBXName;
	
	[XmlIgnore]
	public int CurrentAmount;
	
	public Equiped()
	{
		EquipmentSlots = new List<RPGEquipmentSlot>();
		WornEffects = new List<Effect>();
		Conditions = new List<Condition>();
		FBXName = string.Empty;
	}
	
	[XmlIgnore]
	public int EquipedTooltipRows
	{
		get
		{
			int result = 0;
			
			result += ItemTooltipRows;
			result += Conditions.Count;
			
			result += WornEffects.Count;
			if (WornEffects.Count > 0)
				result+=2;
			
			//for equipment slot
			result ++;
			
			if (Durability > 0)
				Durability++;
			
			return result;
		}
	}
	
	public bool CanYouEquip(Player player)
	{
        foreach (Condition condition in Conditions)
        {
            if (!condition.Validate(player))
                return false;
        }
        return true;
	}
}

