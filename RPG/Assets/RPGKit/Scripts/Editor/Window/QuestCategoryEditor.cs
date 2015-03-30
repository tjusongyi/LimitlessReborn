using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class QuestCategoryEditor : BaseEditorWindow
{
    public QuestCategoryEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Quest category";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGQuestCategory> list = Storage.Load<RPGQuestCategory>(new RPGQuestCategory());
		items = new List<IItem>();
        foreach (RPGQuestCategory category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
        currentItem = new RPGQuestCategory();
	}

    public List<RPGQuestCategory> QuestCategories
	{
		get
		{
            List<RPGQuestCategory> list = new List<RPGQuestCategory>();
			foreach(IItem category in items)
			{
                list.Add((RPGQuestCategory)category);
			}
			return list;		
		}
	}
	
	protected override void SaveCollection()
	{
        Storage.Save<RPGQuestCategory>(QuestCategories, new RPGQuestCategory());
	}
}
