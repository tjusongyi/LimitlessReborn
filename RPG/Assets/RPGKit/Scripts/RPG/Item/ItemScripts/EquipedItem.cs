using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EquipedItem : ItemInWorld
{
	public List<RPGEquipmentSlot> Slots;
	
	public EquipedItem() : base()
	{
		Slots = new List<RPGEquipmentSlot>();
	}
}
