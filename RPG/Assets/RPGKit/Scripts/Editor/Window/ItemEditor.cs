using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ItemEditor : BaseEditorWindow 
{
	ItemGenerator generator;
	
	public ItemEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Item";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGItem> list = Storage.Load<RPGItem>(new RPGItem());
		items = new List<IItem>();
		foreach(RPGItem category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void AditionalSwitch()
	{
		if (MenuMode ==  MenuModeEnum.ThirdWindow)
		{
			GenerateStrongerItem();
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGItem();
	}
	
	public List<RPGItem> Items
	{
		get
		{
			List<RPGItem> list = new List<RPGItem>();
			foreach(IItem category in items)
			{
				list.Add((RPGItem)category);
			}
			return list;
		}
	}
	
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGItem>(Items, new RPGItem());
	}
	
	protected override void EditPart()
	{
		RPGItem s = (RPGItem)currentItem;
		
		if (s.ID > 0 && !s.IsCopy && updateMode)
		{
			if (GUILayout.Button("Generater stronger versions", GUILayout.Width(400)))
			{
				generator = new ItemGenerator();
				generator.Calculate(s);
				MenuMode = MenuModeEnum.ThirdWindow;		
			}
		}
		
		ItemUtils.DisplayItemPart(s, Data, EffectTypeUsage.Usable);
		
		currentItem = s;
	}
	
	private List<RPGItem> generatorCollection;
	private bool IsAnyGeneratedItems()
	{
		foreach(RPGItem rpgItem in generatorCollection)
		{
			if (rpgItem.SourceItem == currentItem.ID)
				return true;
		}
		return false;
	}
	
	void GenerateStrongerItem()
	{
		
		StartMainBox();
		
		RPGItem s = (RPGItem)currentItem;
		generatorCollection = Items;
		if (GUILayout.Button("Back to item", GUILayout.Width(400))) 
		{
			MenuMode = MenuModeEnum.Edit;
		}
		EditorUtils.Separator();
		
		ItemUtils.ItemGenerator(s, generator);
		
		
		
		if (GUILayout.Button("Generate and save items", GUILayout.Width(300)))
		{
			//delete old generated items
			do
			{
				foreach(RPGItem rpgItem in generatorCollection)
				{
					if (rpgItem.SourceItem == currentItem.ID)
					{
						items.Remove(rpgItem);
						break;
					}
				}
			}
			while (IsAnyGeneratedItems());
			generator = new ItemGenerator();
			generator.Calculate(currentItem);
			//insert new generated items
			foreach(IItem item in generator.Items)
			{
				RPGItem rpgItem = (RPGItem)item;
				generatorCollection.Add(rpgItem);
			}
				
			foreach(IItem item in generatorCollection)
			{
				if (item.ID > 0)
					continue;
				item.ID = EditorUtils.NewAttributeID<RPGItem>(generatorCollection);
			}
			
			Storage.Save<RPGItem>(generatorCollection, s);
			LoadData();
			MenuMode = MenuModeEnum.List;
		}
		
		GUILayout.EndArea();
		GUILayout.EndArea();
	}
}
