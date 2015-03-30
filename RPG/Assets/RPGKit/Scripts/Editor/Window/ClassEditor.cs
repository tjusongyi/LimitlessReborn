using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ClassEditor : BaseEditorWindow 
{
	public ClassEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Class";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGCharacterClass> list = Storage.Load<RPGCharacterClass>(new RPGCharacterClass());
		items = new List<IItem>();
		foreach(RPGCharacterClass category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGCharacterClass();
	}
	
	public List<RPGCharacterClass> Classes
	{
		get
		{
			List<RPGCharacterClass> list = new List<RPGCharacterClass>();
			foreach(IItem category in items)
			{
				list.Add((RPGCharacterClass)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGCharacterClass>(Classes, new RPGCharacterClass());
	}
	
	protected override void EditPart()
	{
		RPGCharacterClass s = (RPGCharacterClass)currentItem;
		
		
		
		currentItem = s;
	}
}
