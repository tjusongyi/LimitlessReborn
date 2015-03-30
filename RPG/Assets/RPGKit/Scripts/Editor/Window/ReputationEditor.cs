using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ReputationEditor : BaseEditorWindow
{
	public ReputationEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Reputation";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGReputation> list = Storage.Load<RPGReputation>(new RPGReputation());
		items = new List<IItem>();
		foreach(RPGReputation category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGReputation();
	}
	
	public List<RPGReputation> Reputations
	{
		get
		{
			List<RPGReputation> list = new List<RPGReputation>();
			foreach(IItem category in items)
			{
				list.Add((RPGReputation)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGReputation>(Reputations, new RPGReputation());
	}
	
	protected override void EditPart()
	{
		RPGReputation s = (RPGReputation)currentItem;
		
		s.TextBefore = EditorUtils.TextField(s.TextBefore, "Text before");
		
		s.LowestRank = EditorUtils.TextField(s.LowestRank, "Lowest rank");
		
		foreach(ReputationRank r in s.Ranks)
		{
			EditorUtils.Separator();
			
			r.Name = EditorUtils.TextField(r.Name, "Name");
			
			r.Amount = EditorUtils.IntField(r.Amount, "Amount");
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("Delete", GUILayout.Width(120)))
			{
				s.Ranks.Remove(r);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUILayout.Button("Add reputation rank",GUILayout.Width(180)))
		{
			s.Ranks.Add(new ReputationRank());
		}
		
		currentItem = s;
	}

}
