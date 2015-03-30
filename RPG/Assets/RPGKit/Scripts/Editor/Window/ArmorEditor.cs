using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ArmorEditor : BaseEditorWindow 
{
	ArmorGenerator generator = new ArmorGenerator();
	
	public ArmorEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Armor";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGArmor> list = Storage.Load<RPGArmor>(new RPGArmor());
		items = new List<IItem>();
		foreach(RPGArmor category in list)
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
		currentItem = new RPGArmor();
	}
	
	public List<RPGArmor> Armors
	{
		get
		{
			List<RPGArmor> list = new List<RPGArmor>();
			foreach(IItem category in items)
			{
				list.Add((RPGArmor)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGArmor>(Armors, new RPGArmor());
	}
	
	protected override void EditPart()
	{
		RPGArmor s = (RPGArmor)currentItem;
		
		if (s.ID > 0 && !s.IsCopy && updateMode)
		{
			if (GUILayout.Button("Generater stronger versions", GUILayout.Width(400)))
			{
				generator = new ArmorGenerator();
				generator.Calculate(s);
				MenuMode = MenuModeEnum.ThirdWindow;		
			}
		}
		
		EditorUtils.Separator();
		
		s.ArmorClassValue = EditorUtils.IntField(s.ArmorClassValue, "Armor value");
		
		ItemUtils.DisplayItemPart(s, Data, EffectTypeUsage.Usable);
		
		ItemUtils.AddEquiped(s, Data);
		
		EditorUtils.Label("Effects on hit");
		
		EffectUtils.EffectsEditor(s.EffectsOnHit, Data, EffectTypeUsage.ArmorTakeHit);
		
		EditorUtils.Separator();
		
		currentItem = s;
	}
	
	private List<RPGArmor> generatorCollection;
	void GenerateStrongerItem()
	{
		StartMainBox();
		
		RPGArmor s = (RPGArmor)currentItem;
		generatorCollection = Armors;
		
		if (GUILayout.Button("Back to item", GUILayout.Width(400))) 
		{
			MenuMode = MenuModeEnum.Edit;
		}
		EditorUtils.Separator();
		
		ItemUtils.ArmorGenerator(generator);
		
		EditorUtils.Separator();
		
		ItemUtils.ItemGenerator(s, generator);
		
		EditorUtils.Separator();
		
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
			generator = new ArmorGenerator();
			generator.Calculate(currentItem);
			//insert new generated items
			foreach(IItem item in generator.Items)
			{
				RPGArmor rpgItem = (RPGArmor)item;
				generatorCollection.Add(rpgItem);
			}
				
			foreach(IItem item in generatorCollection)
			{
				if (item.ID > 0)
					continue;
				item.ID = EditorUtils.NewAttributeID<RPGArmor>(generatorCollection);
			}
			
			Storage.Save<RPGArmor>(generatorCollection, s);
			LoadData();
			MenuMode = MenuModeEnum.List;
		}
	}
	
	private bool IsAnyGeneratedItems()
	{
		foreach(RPGArmor rpgItem in generatorCollection)
		{
			if (rpgItem.SourceItem == currentItem.ID)
				return true;
		}
		return false;
	}
}
