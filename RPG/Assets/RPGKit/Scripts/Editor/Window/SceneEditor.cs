using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneEditor : BaseEditorWindow 
{
	public SceneEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Scene";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGScene> list = Storage.Load<RPGScene>(new RPGScene());
		items = new List<IItem>();
		foreach(RPGScene category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGScene();
	}
	
	public List<RPGScene> Scenes
	{
		get
		{
			List<RPGScene> list = new List<RPGScene>();
			foreach(IItem category in items)
			{
				list.Add((RPGScene)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGScene>(Scenes, new RPGScene());
	}
	
	protected override void EditPart()
	{
		RPGScene s = (RPGScene)currentItem;
		
		s.SceneName = EditorUtils.TextField(s.SceneName, "Scene name");
		
		s.SceneId = EditorUtils.IntField(s.SceneId, "Scene ID");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
			
		EditorGUILayout.PrefixLabel("Scene type");
		s.SceneType = (SceneTypeEnum)EditorGUILayout.EnumPopup(s.SceneType, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add new enemy ID", GUILayout.Width(300)))
		{
			s.Enemies.Add(0);
		}
		
		EditorGUILayout.EndHorizontal();
		
		for (int index = 0; index <= s.Enemies.Count -1; index++)
		{
			
			s.Enemies[index] = EditorUtils.IntPopup(s.Enemies[index], Data.enemyEditor.items, "Enemy ID", 200, FieldTypeEnum.BeginningOnly);
			
			if (GUILayout.Button("Delete", GUILayout.Width(150)))
			{
				 s.Enemies.Remove(s.Enemies[index]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		currentItem = s;
	}
}
