using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class GuildEditor : BaseEditorWindow 
{
	public GuildEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Guild";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGGuild> list = Storage.Load<RPGGuild>(new RPGGuild());
		items = new List<IItem>();
		foreach(RPGGuild category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGGuild();
	}
	
	public List<RPGGuild> Guilds
	{
		get
		{
			List<RPGGuild> list = new List<RPGGuild>();
			foreach(IItem category in items)
			{
				list.Add((RPGGuild)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGGuild>(Guilds, new RPGGuild());
	}
	
	protected override void EditPart()
	{
		RPGGuild s = (RPGGuild)currentItem;
		
		EditorUtils.Label("Join guild conditions");
		
		ConditionsUtils.Conditions(s.Conditions, Data);
		
		if (GUILayout.Button("Add guild rank", GUILayout.Width(400)))
		{
			GuildRank gr = new GuildRank();
			gr.ID = s.Ranks.Count + 1;
			s.Ranks.Add(gr);
		}
		
		foreach(GuildRank gr in s.Ranks)
		{
			AddGuildRank(gr);
			
			if (GUILayout.Button("Delete", GUILayout.Width(100)))
			{
				s.Ranks.Remove(gr);
				break;
			}
			EditorUtils.Separator();
		}
		
		currentItem = s;
	}
	
	void AddGuildRank(GuildRank gr)
	{
		
		EditorUtils.Label("Guild rank");
		gr.Name = EditorUtils.TextField(gr.Name, "Name");
		
		if (GUILayout.Button("Add task", GUILayout.Width(400))) 
		{
			gr.Tasks.Add(new RankTask());
		}
		
		foreach(RankTask task in gr.Tasks)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Task type");
		
			task.Task = (RankTaskEnum)EditorGUILayout.EnumPopup(task.Task, GUILayout.Width(300));
			EditorGUILayout.PrefixLabel(" ID: ");
			task.ID = EditorGUILayout.IntField(task.ID, GUILayout.Width(90));
			EditorGUILayout.PrefixLabel(" amount: ");
			task.Amount = EditorGUILayout.IntField(task.Amount, GUILayout.Width(100));
			
			task.Preffix =  EditorUtils.TextField(task.Preffix, "Preffix", 100, FieldTypeEnum.Middle);
			
			if (GUILayout.Button("Delete", GUILayout.Width(100)))
			{
				gr.Tasks.Remove(task);
				break;
			}
			
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUILayout.Button("Add reward", GUILayout.Width(400)))
		{
			Reward reward = new Reward();
			gr.Rewards.Add(reward);
		}
		if (gr.Rewards != null && gr.Rewards.Count >0)
		{
			foreach(Reward reward in gr.Rewards)
			{
				AddReward(reward);	
			}
		}
	}
	
	void AddReward(Reward reward)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Reward");
		
		reward.Preffix = (PreffixType)EditorGUILayout.EnumPopup(reward.Preffix, GUILayout.Width(300));
		EditorGUILayout.PrefixLabel(" ID: ");
		reward.ItemId = EditorGUILayout.IntField(reward.ItemId, GUILayout.Width(90));
		EditorGUILayout.PrefixLabel(" amount: ");
		reward.Amount = EditorGUILayout.IntField(reward.Amount, GUILayout.Width(100));
		
		EditorGUILayout.EndHorizontal();
	}
}
