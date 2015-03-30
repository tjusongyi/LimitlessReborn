using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ItemCategoryEditor : BaseEditorWindow 
{
	public ItemCategoryEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Item category";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGItemCategory> list = Storage.Load<RPGItemCategory>(new RPGItemCategory());
		items = new List<IItem>();
		foreach(RPGItemCategory category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGItemCategory();
	}
	
	public List<RPGItemCategory> ItemCategories
	{
		get
		{
			List<RPGItemCategory> list= new List<RPGItemCategory>();
			foreach(IItem category in items)
			{
				list.Add((RPGItemCategory)category);
			}
			return list;		
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGItemCategory>(ItemCategories, new RPGItemCategory());
	}
}
