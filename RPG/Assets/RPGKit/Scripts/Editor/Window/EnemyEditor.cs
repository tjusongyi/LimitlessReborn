using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EnemyEditor : BaseEditorWindow 
{
	public EnemyEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Enemy";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGEnemy> list = Storage.Load<RPGEnemy>(new RPGEnemy());
		items = new List<IItem>();
		foreach(RPGEnemy category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGEnemy();
	}
	
	public List<RPGEnemy> Enemies
	{
		get
		{
			List<RPGEnemy> list= new List<RPGEnemy>();
			foreach(IItem category in items)
			{
				list.Add((RPGEnemy)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGEnemy>(Enemies, new RPGEnemy());
	}
	
	protected override void EditPart()
	{
		RPGEnemy s = (RPGEnemy)currentItem;
		
		s.PrefabPath = EditorUtils.TextField(s.PrefabPath, "Prefab path");
		
		s.AttackSpeed = EditorUtils.FloatField(s.AttackSpeed, "Attack speed");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Difficulty");
		
		s.Difficulty = (DifficultyEnum)EditorGUILayout.EnumPopup(s.Difficulty, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		s.UseDefaultValues = EditorUtils.Toggle(s.UseDefaultValues, "Use default values");
		
		
		
		s.Level = EditorUtils.IntField(s.Level, "Level", 100,FieldTypeEnum.BeginningOnly);
		
		if (GUILayout.Button("Recalculate stats", GUILayout.Width(150)))
		{
			s.CalculateStats();
		}
		EditorGUILayout.EndHorizontal();
		
		if (!s.UseDefaultValues)
		{
			s.Experience = EditorUtils.IntField(s.Experience, "Experience");
			
			s.ChanceToEvade = EditorUtils.IntField(s.ChanceToEvade, "Chance to evade");
			
			s.ChanceToHit = EditorUtils.IntField(s.ChanceToHit, "Chance to hit");
			
			s.MaximumHp = EditorUtils.IntField(s.MaximumHp, "Hit points");
			
			s.MinimumDmg = EditorUtils.IntField(s.MinimumDmg, "Minimum dmg", 100, FieldTypeEnum.BeginningOnly);
			
			s.MaximumDmg = EditorUtils.IntField(s.MaximumDmg, "Maximum dmg", 100, FieldTypeEnum.Middle);
			
			s.Armor = EditorUtils.IntField(s.Armor, "Armor", 100, FieldTypeEnum.EndOnly);
		}
		
		EditorUtils.Separator();
		
		EditorUtils.Label("Enemy loot");
		
		DisplayContainer(s.Container, Data);
		
		currentItem = s;
	}
	
	public void DisplayContainer(RPGContainer container, MainWindowEditor Data)
	{
		//container categories
		foreach(ContainerCategory category in container.Categories)
		{
			DisplayContainerCategory(category, Data);
			
			if (GUILayout.Button("Delete category", GUILayout.Width(200)))
			{
				container.Categories.Remove(category);
				break;
			}
		}
		if (GUILayout.Button("Add category", GUILayout.Width(300)))
		{
			container.Categories.Add(new ContainerCategory());
		}
		EditorUtils.Separator();
		
		//shop items
		foreach(ContainerItem item in container.ContainerItems)
		{
			DisplayItem(item);
			
			if (GUILayout.Button("Delete item", GUILayout.Width(200)))
			{
				container.ContainerItems.Remove(item);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorUtils.Separator();
		if (GUILayout.Button("Add item", GUILayout.Width(200)))
		{
			container.ContainerItems.Add(new ContainerItem());
		}
	}
		
	
	public void DisplayItem(ContainerItem item)
	{
		ConditionsUtils.Conditions(item.Conditions,Data);
		
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
