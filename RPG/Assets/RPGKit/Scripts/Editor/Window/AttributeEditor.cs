using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AttributeEditor : BaseEditorWindow 
{
	public AttributeEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Attribute";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGAttribute> list = Storage.Load<RPGAttribute>(new RPGAttribute());
		items = new List<IItem>();
		foreach(RPGAttribute category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGAttribute();
	}
	
	public List<RPGAttribute> Attributes
	{
		get
		{
			List<RPGAttribute> list= new List<RPGAttribute>();
			foreach(IItem category in items)
			{
				list.Add((RPGAttribute)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGAttribute>(Attributes, new RPGAttribute());
	}
	
	protected override void EditPart()
	{
		RPGAttribute s = (RPGAttribute)currentItem;
		
		StatisticIncreaseUtils.DisplayForm(s, false);
		
		currentItem = s;
	}
}
