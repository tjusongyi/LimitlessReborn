using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecipeEditor : EditorWindow 
{
	/*RPGRecipe currentItem = new RPGRecipe();
	List<RPGRecipe> items = new List<RPGRecipe>();
	List<Shop> shops = new List<Shop>();
	List<SpellShop> spellShops = new List<SpellShop>();
	
	Vector2 scrollPosition = new Vector2(0, 0);
	
	int MenuMode = 1;
	bool IsPostBack = false;
    bool editMode = false;
	
	[MenuItem("RPG/Recipe editor")]
    static void Init()
    {
        RecipeEditor window = (RecipeEditor)EditorWindow.GetWindow(typeof(RecipeEditor));
        window.autoRepaintOnSceneChange = true;
        window.position = new Rect(200, 100, 1200, 880);
        window.title = "Recipe editor";
        window.wantsMouseMove = true;
        window.Show();
        window.ShowUtility();
    }
	
	private void LoadItems()
	{
		items = Storage.Load<RPGRecipe>(currentItem);
		
	}
	
	void OnGUI()
	{
		if (!IsPostBack)
			LoadItems();
		
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		
		//list mode
		if (MenuMode == 1)
		{
			ListMode();
		}
		//edit mode
		else if (MenuMode == 2)
		{
			EditMode();
		}
		
		EditorGUILayout.EndScrollView();
		IsPostBack = true;
	}
	
	void ListMode()
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add new recipe", GUILayout.Width(400))) 
		{
			currentItem = new RPGRecipe();
			MenuMode = 2;
			currentItem.ID = NewAttributeID();
		}
		EditorGUILayout.EndHorizontal();
		
		foreach(RPGRecipe item in items)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
				
			if (GUILayout.Button("Edit ID " + item.ID + ", name  " + item.Name, GUILayout.Width(500))) 
			{
				currentItem = item;
				MenuMode = 2;
                editMode = true;
				EditMode();
			}
			if (GUILayout.Button("Delete", GUILayout.Width(100)))
			{
				Delete(item.ID);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
	}
	
	void Delete(int itemId)
	{
		foreach(RPGRecipe item in items)
		{
			if (item.ID == itemId)
			{
				items.Remove(item);
				break;
			}
		}
		Storage.Save<RPGRecipe>(items, new RPGRecipe());
		
		LoadItems();
	}
	
	void EditMode()
	{
		if (GUILayout.Button("Back to recipe list", GUILayout.Width(400))) 
		{
			MenuMode = 1;
		}
		
		EditorUtils.Separator();
		
		GUIUtils.AddBasicInformation(currentItem);
		
		EditorUtils.Separator();
		
		GUIUtils.Conditions(currentItem.Conditions);
		
		
		
		
		
		EditorUtils.Separator();
		
		if (GUILayout.Button("Save Recipe", GUILayout.Width(400))) 
		{
			SaveItem();
		}
	}
	
	void SaveItem()
	{
        if (!editMode)
        {
            items.Add(currentItem);
        }
        Storage.Save<RPGRecipe>(items, currentItem);
        MenuMode = 1;
        editMode = false;
		IsPostBack = false;
	}
	
	int NewAttributeID()
	{
		int maximum = 0;
		foreach(IItem p in items)
		{
			if (p.ID > maximum)
				maximum = p.ID;
		}
		maximum++; 
		return maximum;
	}*/
}
