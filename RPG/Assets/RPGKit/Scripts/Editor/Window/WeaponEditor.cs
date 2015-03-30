using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WeaponEditor : BaseEditorWindow 
{
	WeaponGenerator generator = new WeaponGenerator();
	
	public WeaponEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Weapon";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGWeapon> list = Storage.Load<RPGWeapon>(new RPGWeapon());
		items = new List<IItem>();
		foreach(RPGWeapon category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGWeapon();
	}
	
	protected override void AditionalSwitch()
	{
		if (MenuMode ==  MenuModeEnum.ThirdWindow)
		{
			GenerateStrongerItem();
		}
	}
	
	public List<RPGWeapon> Weapons
	{
		get
		{
			List<RPGWeapon> list = new List<RPGWeapon>();
			foreach(IItem category in items)
			{
				list.Add((RPGWeapon)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGWeapon>(Weapons, new RPGWeapon());
	}
	
	protected override void EditPart()
	{
		RPGWeapon s = (RPGWeapon)currentItem;
		
		if (s.ID > 0 && !s.IsCopy && updateMode)
		{
			if (GUILayout.Button("Generater stronger versions", GUILayout.Width(400)))
			{
				generator = new WeaponGenerator();
				generator.Calculate(s);
				MenuMode = MenuModeEnum.ThirdWindow;
			}
		}
		
		EditorUtils.Separator();
		
		ItemUtils.AddWeapon(s);
		
		EditorUtils.Separator();
		
		ItemUtils.AddEquiped(s, Data);
		
		ItemUtils.DisplayItemPart(s, Data, EffectTypeUsage.Usable);
		EditorUtils.Separator();
		
		EditorUtils.Label("Effects on hit");
		
		EffectUtils.EffectsEditor(s.EffectsHit, Data, EffectTypeUsage.WeaponHit);
		
		currentItem = s;
	}
	
	private List<RPGWeapon> generatorCollection;
	
	void GenerateStrongerItem()
	{
		StartMainBox();
		
		if (GUILayout.Button("Back to item", GUILayout.Width(400))) 
		{
			MenuMode = MenuModeEnum.Edit;
		}
		EditorUtils.Separator();
		RPGWeapon s = (RPGWeapon)currentItem;
		generatorCollection = Weapons;
		
		ItemUtils.WeaponGenerator(generator);
		
		EditorUtils.Separator();
		
		ItemUtils.ItemGenerator(s, generator);
		
		EditorUtils.Separator();
		
		if (GUILayout.Button("Generate and save items", GUILayout.Width(300)))
		{
			//delete old generated items
			do
			{
				foreach(RPGWeapon rpgItem in items)
				{
					if (rpgItem.SourceItem == s.ID)
					{
						items.Remove(rpgItem);
						break;
					}
				}
			}
			while (IsAnyGeneratedItems());
			generator.Calculate(s);
			//insert new generated items
			foreach(IItem item in generator.Items)
			{
				RPGWeapon rpgItem = (RPGWeapon)item;
				generatorCollection.Add(rpgItem);
			}
				
			foreach(IItem item in generatorCollection)
			{
				if (item.ID > 0)
					continue;
				item.ID = GUIUtils.NewAttributeID(items);
			}
			Storage.Save<RPGWeapon>(generatorCollection, s);
			LoadData();
			MenuMode = MenuModeEnum.List;
		}
	}
	
	private bool IsAnyGeneratedItems()
	{
		foreach(RPGWeapon rpgItem in generatorCollection)
		{
			if (rpgItem.SourceItem == currentItem.ID)
				return true;
		}
		return false;
	}
}
