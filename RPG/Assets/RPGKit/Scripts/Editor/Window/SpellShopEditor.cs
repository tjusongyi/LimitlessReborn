using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpellShopEditor : BaseEditorWindow 
{

	public SpellShopEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Spell shop";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGSpellShop> list = Storage.Load<RPGSpellShop>(new RPGSpellShop());
		items = new List<IItem>();
		foreach(RPGSpellShop category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGSpellShop();
	}
	
	public List<RPGSpellShop> SpellShops
	{
		get
		{
			List<RPGSpellShop> list = new List<RPGSpellShop>();
			foreach(IItem category in items)
			{
				list.Add((RPGSpellShop)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGSpellShop>(SpellShops, new RPGSpellShop());
	}
	
	protected override void EditPart()
	{
		RPGSpellShop s = (RPGSpellShop)currentItem;
		
		s.CurrencyID = EditorUtils.IntPopup(s.CurrencyID, Data.itemEditor.items, "Currency");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Buy modifier");
		
		s.BuyPriceModifier = EditorGUILayout.FloatField(s.BuyPriceModifier,  GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		EditorUtils.Separator();
		//shop categories
		foreach(SpellShopCategory category in s.Categories)
		{
			DisplayShopCategory(category);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Categories.Remove(category);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		if (GUILayout.Button("Add category", GUILayout.Width(300)))
		{
			s.Categories.Add(new SpellShopCategory());
		}
		
		EditorUtils.Separator();
		
		
		//shop items
		foreach(SpellShopItem item in s.Items)
		{
			DisplayShopItem(item);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Items.Remove(item);
				break;
			}
			
			EditorGUILayout.EndHorizontal();
		}
		EditorUtils.Separator();
		if (GUILayout.Button("Add item", GUILayout.Width(200)))
		{
			s.Items.Add(new SpellShopItem());
		}
		
		currentItem = s;
	}

	
	public void DisplayShopCategory(SpellShopCategory category)
	{
		ConditionsUtils.Conditions(category.Conditions, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Levels");
		category.LevelAdjustment = (LevelAdjustmentType)EditorGUILayout.EnumPopup(category.LevelAdjustment ,GUILayout.Width(200));
		
		EditorGUILayout.PrefixLabel("Min");
		category.MinValue = EditorGUILayout.IntField(category.MinValue ,GUILayout.Width(100));
		
		EditorGUILayout.PrefixLabel("Max");
		category.MaxValue = EditorGUILayout.IntField(category.MaxValue ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Skill");
		
		category.SkillID = EditorGUILayout.IntField(category.SkillID ,GUILayout.Width(300));
	}
	
	void DisplayShopItem(SpellShopItem item)
	{
		ConditionsUtils.Conditions(item.Conditions, Data);
		
		item.SpellID = EditorUtils.IntPopup(item.SpellID, Data.spellEditor.items, "Spell", FieldTypeEnum.BeginningOnly);
	}
}
