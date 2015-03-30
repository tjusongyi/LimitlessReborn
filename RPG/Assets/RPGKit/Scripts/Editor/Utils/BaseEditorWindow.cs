using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class BaseEditorWindow
{
	public List<IItem> items;
	
	private List<IItem> displayedItems;
	public IItem currentItem;
	public MenuModeEnum MenuMode;
	protected bool updateMode;
	protected Vector2 scrollPosition;
	protected bool updateOutside;
	protected int outsideID;
	
	public Rect position;
	public GUISkin skin;
	
	private bool loadData = true;
	public string EditorName;
	private string searchedString;
	private string searchedID;
	
	protected MainWindowEditor Data;
	
	protected void Init(GUISkin guiSkin, MainWindowEditor data)
	{
		skin = guiSkin;
		Data = data;
		searchedID = searchedString = string.Empty;
		
		LoadData();
	}
	
	protected virtual void Refresh()
	{
		throw new Exception("Not implemented yet!");
	}
	
	protected virtual void LoadData()
	{
		throw new Exception("Not implemented yet!");
	}
	
	protected virtual void StartNewIItem()
	{
		throw new Exception("Not implemented yet!");
	}
	
	protected virtual void EditPart()
	{
		//can be empty
	}
	
	protected virtual void SaveCollection()
	{
		throw new Exception("Not implemented yet!");
	}
	
	protected virtual void AditionalSwitch()
	{
		//can be empty
	}
	
	public bool CheckIDExist(int ID)
	{
		if (ID == 0)
			return true;
		
		bool founded = false;
		foreach(IItem item in items)
		{
			if (item.ID == ID)
			{
				founded = true;
				break;
			}
		}
		
		return founded;
	}
	
	public void DisplayWindow(Rect Position)
	{
		position = Position;
		if (loadData)
		{
			LoadData();
			loadData = false;
			displayedItems = items;
		}
		Data.isValidateDisplay = false;
		GUILayout.BeginArea(new Rect(5,150, position.width - 10, 20), skin.box);
		EditorGUILayout.LabelField(EditorName);
		GUILayout.EndArea();
		
		switch(MenuMode)
		{
			case MenuModeEnum.List : ListMode();
										break;
			
			case MenuModeEnum.Edit : EditMode();
										break;
		} 
		
		AditionalSwitch();
	}
	
	private void DetermineID()
	{
		int maximum = 0;
		foreach(IItem item in items)
		{
			if (item.ID > maximum)
				maximum = item.ID;
		}
		maximum++;
		currentItem.ID = maximum;
	}
	
	private void StartTopBox()
	{
		GUILayout.BeginArea(new Rect(5,180, position.width - 10, 60), skin.box);
	}
	
	protected void StartMainBox()
	{
		GUILayout.BeginArea(new Rect(5,250, position.width - 10, Screen.height - 280), skin.box);
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
	}
	
	private void Search()
	{
		displayedItems = new List<IItem>();
		searchedString.ToLower();
		foreach(IItem item in items)
		{
			if (item.Name.ToLower().IndexOf(searchedString) != -1 && !string.IsNullOrEmpty(searchedString))
			{
				displayedItems.Add(item);
				continue;
			}
			
			if (item.Description.ToLower().IndexOf(searchedString) != -1 && !string.IsNullOrEmpty(searchedString))
			{
				displayedItems.Add(item);
				continue;
			}
			
			if (item.SystemDescription.ToLower().IndexOf(searchedString) != -1 && !string.IsNullOrEmpty(searchedString))
			{
				displayedItems.Add(item);
				continue;
			}
			if (item.ID.ToString().IndexOf(searchedID) != -1 && !string.IsNullOrEmpty(searchedID))
			{
				displayedItems.Add(item);
				continue;
			}
			
			if (item.SystemDescription.ToString().IndexOf(searchedString) != -1 && !string.IsNullOrEmpty(searchedString))
			{
				displayedItems.Add(item);
				continue;
			}
		}
	}
	
	private void PrepareSearch()
	{
		if (string.IsNullOrEmpty(searchedString) && string.IsNullOrEmpty(searchedID))
		{
			Clear();	
		}
		else
		{
			Search();
		}
	}
	
	private void Clear()
	{
		displayedItems = items;
		searchedString = string.Empty;
		searchedID = string.Empty;
	}
	
	private void ListMode()
	{
		//add button area
		StartTopBox();
		
		if (GUILayout.Button("Add new " + EditorName.ToLower(), GUILayout.Width(300)))
		{
			MenuMode = MenuModeEnum.Edit;
			updateMode = false;
			StartNewIItem();
			DetermineID();
			currentItem.Name = string.Empty;
			currentItem.Description = string.Empty;
			currentItem.SystemDescription = string.Empty;
		}
		string temp = searchedString;
		temp = EditorUtils.TextField(temp, "Search", 200, FieldTypeEnum.BeginningOnly);
		
		if (temp != searchedString)
		{
			searchedString = temp;
			PrepareSearch();
		}
		
		temp = searchedID;
		temp = EditorUtils.TextField(temp, "ID", 80, FieldTypeEnum.Middle);
		if (temp != searchedID)
		{
			searchedID = temp;
			PrepareSearch();
		}
		
		if (GUILayout.Button("Clear", GUILayout.Width(150)))
		{
			Clear();
		}
		
		GUILayout.EndArea();
		
		StartMainBox();
		
		foreach(IItem item in displayedItems)
		{
			GUILayout.BeginHorizontal();
			string buttonLabel = "ID: " + item.ID + " - ";
			
			if (!string.IsNullOrEmpty(item.Name))
			{
				buttonLabel += item.Name;
			}
			if (!string.IsNullOrEmpty(item.SystemDescription))
			{
				buttonLabel += "(" + item.SystemDescription + ")";
			}
			if ((outsideID == item.ID && updateOutside) || GUILayout.Button(buttonLabel, GUILayout.Width(position.width - 200)))
			{
				MenuMode = MenuModeEnum.Edit;
				updateMode = true;
				updateOutside = false;
				outsideID = 0;
				currentItem = item;
			}

            if (GUILayout.Button("Copy", GUILayout.Width(80)))
            {
                CopyItem(item);
                return;
            }
			
			if (GUILayout.Button("Delete", GUILayout.Width(80)))
			{
				items.Remove(item);
				SaveCollection();
				Data.InitWindows();
				return;
			}
			
			GUILayout.EndHorizontal();
		}
		
		EditorGUILayout.EndScrollView();
		
		GUILayout.EndArea();
	}

    private void CopyItem(IItem item)
    {
        IItem newItem = item;

        displayedItems.Add(newItem);

        SaveCollection();

        Data.InitWindows();
    }
	
	public void SelectItemBYID(int ID)
	{
		MenuMode = MenuModeEnum.List;
		outsideID = ID;
		updateOutside = true;
	}
	
	protected void TopFormPart()
	{
		StartTopBox();
		GUILayout.EndArea();
	}
	
	protected void SaveButton()
	{
        EditorUtils.MainSeparator();

		if (GUILayout.Button("Save "+EditorName.ToLower(), GUILayout.Width(250)))
		{
			SaveButtonEvent();
		}  
		
		EditorGUILayout.EndScrollView();
		GUILayout.EndArea(); 
	}
	
	void SaveButtonEvent()
	{
		if (!updateMode)
			items.Add(currentItem);	
		SaveCollection();
		loadData = true;
		MenuMode = MenuModeEnum.List;
	}
	
	protected void EditMode()
	{
		StartTopBox();
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Save " + EditorName.ToLower(), GUILayout.Width(300)))
		{
			SaveButtonEvent();
		}
		
		if (GUILayout.Button("Back to list", GUILayout.Width(300)))
		{
			MenuMode = MenuModeEnum.List;
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		//main area
		StartMainBox();
		
		EditorUtils.IItemEditor(currentItem); 
		
		EditPart();
		
		SaveButton();
	}
}

public enum MenuModeEnum
{
	List,
	Edit,
	ThirdWindow
}
