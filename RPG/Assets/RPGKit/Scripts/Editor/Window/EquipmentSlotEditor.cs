using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class EquipmentSlotEditor : BaseEditorWindow 
{
	public EquipmentSlotEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Equipment slot";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGEquipmentSlot> list = Storage.Load<RPGEquipmentSlot>(new RPGEquipmentSlot());
		items = new List<IItem>();
		foreach(RPGEquipmentSlot category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGEquipmentSlot();
	}
	
	public List<RPGEquipmentSlot> Slots
	{
		get
		{
			List<RPGEquipmentSlot> list= new List<RPGEquipmentSlot>();
			foreach(IItem category in items)
			{
				list.Add((RPGEquipmentSlot)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGEquipmentSlot>(Slots, new RPGEquipmentSlot());
	}
	
	protected override void EditPart()
	{
		RPGEquipmentSlot s = (RPGEquipmentSlot)currentItem;

        EditorUtils.Label("Coordinates for character screen slot position");
		s.PosX = EditorUtils.IntField(s.PosX, "Position X", 100, FieldTypeEnum.BeginningOnly);
		s.PosY = EditorUtils.IntField(s.PosY, "Position Y", 100, FieldTypeEnum.EndOnly);
		
		currentItem = s;
	}
}
