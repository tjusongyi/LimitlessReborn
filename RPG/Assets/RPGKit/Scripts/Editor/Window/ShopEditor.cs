using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ShopEditor : BaseEditorWindow 
{
	public ShopEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Shop";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<Shop> list = Storage.Load<Shop>(new Shop());
		items = new List<IItem>();
		foreach(Shop category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new Shop();
	}
	
	public List<Shop> Shops
	{
		get
		{
			List<Shop> list= new List<Shop>();
			foreach(IItem category in items)
			{
				list.Add((Shop)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<Shop>(Shops, new Shop());
	}
	
	protected override void EditPart()
	{
		Shop s = (Shop)currentItem;
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Respawn");
		
		s.RespawnTimer = (ShopRespawnTimer)EditorGUILayout.EnumPopup(s.RespawnTimer,  GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		s.CurrencyID = EditorUtils.IntPopup(s.CurrencyID, Data.itemEditor.items, "Currency");
		
		s.BuyPriceModifier = EditorUtils.FloatField(s.BuyPriceModifier, "Buy modifier");
		
		s.SellPriceModifier = EditorUtils.FloatField(s.SellPriceModifier, "Sell modifier");
		
		s.SellSameAsBuy = EditorUtils.Toggle(s.SellSameAsBuy, "Accept all goods");
		
		if (s.SellSameAsBuy == false)
		{
			//shop categories to sell
			foreach(ShopCategory category in s.SellCategories)
			{
				DisplayShopCategory(category);
				
				if (GUILayout.Button("Delete", GUILayout.Width(200)))
				{
					s.SellCategories.Remove(category);
					break;
				}
			}
			if (GUILayout.Button("Add category", GUILayout.Width(300)))
			{
				s.SellCategories.Add(new ShopCategory());
			}
		}
		
		EditorUtils.Separator();
		
		//shop categories
		foreach(ShopCategory category in s.Categories)
		{
			DisplayShopCategory(category);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Categories.Remove(category);
				break;
			}
		}
		if (GUILayout.Button("Add category", GUILayout.Width(300)))
		{
			s.Categories.Add(new ShopCategory());
		} 
		EditorUtils.Separator();
		//shop items
		foreach(ShopItem item in s.Items)
		{
			DisplayShopItem(item);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Items.Remove(item);
				break;
			}
		}
		EditorUtils.Separator();
		if (GUILayout.Button("Add item", GUILayout.Width(200)))
		{
			s.Items.Add(new ShopItem());
		}
			
		currentItem = s;
	}
	
	void DisplayShopItem(ShopItem item)
	{
		ConditionsUtils.Conditions(item.Conditions, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Item");
		
		item.Preffix = (ItemTypeEnum)EditorGUILayout.EnumPopup(item.Preffix, GUILayout.Width(200));
		EditorGUILayout.PrefixLabel(" ID: ");
		item.ID = EditorGUILayout.IntField(item.ID, GUILayout.Width(90));
		EditorGUILayout.PrefixLabel(" amount: ");
		item.StackAmount = EditorGUILayout.IntField(item.StackAmount, GUILayout.Width(100));
		
	}
	
	void DisplayShopCategory(ShopCategory category)
	{
		EditorUtils.Separator();
        
        ConditionsUtils.Conditions(category.Conditions, Data);


        category.Category.ID = EditorUtils.IntPopup(category.Category.ID, Data.itemCategory.items, "Category", 100, FieldTypeEnum.WholeLine);

        category.MinStackAmount = EditorUtils.IntField(category.MinStackAmount, "Min stack amount");
        category.MaxStackAmount = EditorUtils.IntField(category.MaxStackAmount, "Max stack amount");
	}
}
