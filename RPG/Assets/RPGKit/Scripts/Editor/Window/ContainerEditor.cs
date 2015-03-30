using UnityEngine;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ContainerEditor : BaseEditorWindow 
{
	public ContainerEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Container";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGContainer> list = Storage.Load<RPGContainer>(new RPGContainer());
		items = new List<IItem>();
		foreach(RPGContainer category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGContainer();
	}
	
	public List<RPGContainer> Containers
	{
		get
		{
			List<RPGContainer> list = new List<RPGContainer>();
			foreach(IItem category in items)
			{
				list.Add((RPGContainer)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGContainer>(Containers, new RPGContainer());
	}
	
	protected override void EditPart()
	{
		RPGContainer s = (RPGContainer)currentItem;

        EditorUtils.Label("Conditions neccessary for openining container");

        ConditionsUtils.Conditions(s.Conditions, Data);

        EditorUtils.Label("Events triggered when player open container");

        GUIUtils.EventsUtils(s.Events, Data);
		
		s.DestroyEmpty = EditorUtils.Toggle(s.DestroyEmpty, "Destroy empty");
		
		s.SharedStash = EditorUtils.Toggle(s.SharedStash, "Shared stash");
		
		s.OnlyLoot = EditorUtils.Toggle(s.OnlyLoot, "Only loot");

        EditorUtils.Separator();

        if (s.Categories != null && s.Categories.Count > 0)
        {
            //container categories
            foreach (ContainerCategory category in s.Categories)
            {
                DisplayContainerCategory(category, Data);

                if (GUILayout.Button("Delete category", GUILayout.Width(200)))
                {
                    s.Categories.Remove(category);
                    break;
                }
            }
        }
        if (GUILayout.Button("Add category", GUILayout.Width(300)))
        {
            try
            {
                s.Categories.Add(new ContainerCategory());
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        EditorUtils.Separator();
        //shop items
        foreach (ContainerItem item in s.ContainerItems)
        {
            DisplayItem(item);

            if (GUILayout.Button("Delete item", GUILayout.Width(200)))
            {
                s.ContainerItems.Remove(item);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add item", GUILayout.Width(200)))
        {
            s.ContainerItems.Add(new ContainerItem());
        }

        EditorUtils.Separator();

        EditorUtils.Label("Effects after opening container");

        EffectUtils.EffectsEditor(s.OpenEffects, Data, EffectTypeUsage.EffectArea);

        EditorUtils.Label("Conditions for avoiding container effect");

        ConditionsUtils.Conditions(s.AvoidConditions, Data);

		currentItem = s;
	}
	
	
	
	public void DisplayItem(ContainerItem item)
	{
		ConditionsUtils.Conditions(item.Conditions, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Chance to find");
		item.ChanceToFind = EditorGUILayout.FloatField(item.ChanceToFind, GUILayout.Width(200));
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Item");
		
		item.Preffix = (ItemTypeEnum)EditorGUILayout.EnumPopup(item.Preffix, GUILayout.Width(200));
		
		switch (item.Preffix)
		{
			case ItemTypeEnum.ARMOR:
				item.ID = EditorUtils.IntPopup(item.ID, Data.armorEditor.items, "Armor", 100,FieldTypeEnum.Middle);
				break;
			
			case ItemTypeEnum.ITEM:
				item.ID = EditorUtils.IntPopup(item.ID, Data.itemEditor.items, "Item", 100,FieldTypeEnum.Middle);
				break;
			
			case ItemTypeEnum.WEAPON:
				item.ID = EditorUtils.IntPopup(item.ID, Data.weaponEditor.items, "Weapon", 100,FieldTypeEnum.Middle);
				break;
		}
		
		EditorGUILayout.PrefixLabel(" amount: ");
		item.StackAmount = EditorGUILayout.IntField(item.StackAmount, GUILayout.Width(100));
		
	}
	
	public void DisplayContainerCategory(ContainerCategory category, MainWindowEditor Data)
	{
        ConditionsUtils.Conditions(category.Conditions, Data);

        category.Category.ID = EditorUtils.IntPopup(category.Category.ID, Data.itemCategory.items, "Category");
		
        category.ChanceToFind = EditorUtils.FloatField(category.ChanceToFind, "Chance to find");
		

        category.MinStackAmount = EditorUtils.IntField(category.MinStackAmount, "Min stack amount");
        category.MaxStackAmount = EditorUtils.IntField(category.MaxStackAmount, "Max stack amount");
	}
}
