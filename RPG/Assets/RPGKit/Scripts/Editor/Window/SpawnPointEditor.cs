using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpawnPointEditor : BaseEditorWindow
{

	public SpawnPointEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Spawn point";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGSpawnPoint> list = Storage.Load<RPGSpawnPoint>(new RPGSpawnPoint());
		items = new List<IItem>();
		foreach(RPGSpawnPoint category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGSpawnPoint();
	}
	
	public List<RPGSpawnPoint> SpawnPoints
	{
		get
		{
			List<RPGSpawnPoint> list = new List<RPGSpawnPoint>();
			foreach(IItem category in items)
			{
				list.Add((RPGSpawnPoint)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGSpawnPoint>(SpawnPoints, new RPGSpawnPoint());
	}
	
	protected override void EditPart()
	{
		RPGSpawnPoint s = (RPGSpawnPoint)currentItem;
		
		s.IsOnlyEvent = EditorUtils.Toggle(s.IsOnlyEvent, "Only event");
		
		if (!s.IsOnlyEvent)
		{
			s.Timer = EditorUtils.FloatField(s.Timer, "Timer");
			
			s.ChanceToSpawn = EditorUtils.IntField(s.ChanceToSpawn, "Chance to spawn");
			
			s.MinPlayerRange = EditorUtils.FloatField(s.MinPlayerRange, "Min player range");
			
			s.MaxPlayerRange = EditorUtils.FloatField(s.MaxPlayerRange, "Max player range");
			
			ConditionsUtils.Conditions(s.SpawnConditions, Data);
		}
		
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
