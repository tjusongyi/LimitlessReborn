using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class QuestEditor : BaseEditorWindow 
{
	public QuestEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Quests";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGQuest> list = Storage.Load<RPGQuest>(new RPGQuest());
		items = new List<IItem>();
		foreach(RPGQuest category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGQuest();
	}
	
	public List<RPGQuest> Quests
	{
		get
		{
			List<RPGQuest> list = new List<RPGQuest>();
			foreach(IItem category in items)
			{
				list.Add((RPGQuest)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		List<RPGQuest> quests = Quests;
		foreach(RPGQuest q in quests)
		{
			q.OrderAlternateEnd();
		}
		Storage.Save<RPGQuest>(quests, new RPGQuest());
	}
	
	protected override void EditPart()
	{
		RPGQuest s = (RPGQuest)currentItem;
		
		s.GuildID = EditorUtils.IntPopup(s.GuildID, Data.guildEditor.items, "Guild quest");
		
		s.GuildRank = EditorUtils.IntField(s.GuildRank, "Rank number");

        s.QuestCategoryID = EditorUtils.IntPopup(s.QuestCategoryID, Data.questCategoryEditor.items, "Quest category");
		
		s.Repeatable = EditorUtils.Toggle(s.Repeatable, "Repeatable");
		
		s.FinalQuestLog = EditorUtils.TextField(s.FinalQuestLog, "Final quest log");
		
		if (GUILayout.Button("Add alternate end", GUILayout.Width(400)))
		{
			AlternateEnd end = new AlternateEnd();
			s.AlternateEnds.Add(end);
			s.OrderAlternateEnd();
		}
		if (s.AlternateEnds != null && s.AlternateEnds.Count >0)
		{
			foreach(AlternateEnd e in s.AlternateEnds)
			{
				AlternateEnd(e);	
			}
		}
		
		if (GUILayout.Button("Add reward", GUILayout.Width(400)))
		{
			Reward reward = new Reward();
			s.Rewards.Add(reward);
		}
		if (s.Rewards != null && s.Rewards.Count >0)
		{
			foreach(Reward reward in s.Rewards)
			{
				AddReward(reward);	
			}
		}
		EditorUtils.Separator();
		
		foreach(QuestStep questStep in s.QuestSteps)
		{
			questStep.QuestLogNote = EditorUtils.TextField(questStep.QuestLogNote, "Quest log note");
			
			AddQuestStep(questStep);
		}
		
		if (GUILayout.Button("Add quest step", GUILayout.Width(400)))
		{
			QuestStep questStep = new QuestStep();
			s.QuestSteps.Add(questStep);
			questStep.StepNumber = s.QuestSteps.Count;
		}
		
		currentItem = s;
	}
	
	void AlternateEnd(AlternateEnd end)
	{
		EditorUtils.Label("Alternate end " + end.ID);
		
		end.QuestLogEntry = EditorUtils.TextField(end.QuestLogEntry, "Description");
		
		if (GUILayout.Button("Add reward", GUILayout.Width(400)))
		{
			Reward reward = new Reward();
			end.Rewards.Add(reward);
		}
		if (end.Rewards != null && end.Rewards.Count >0)
		{
			foreach(Reward reward in end.Rewards)
			{
				AddReward(reward);	
			}
		}
		EditorUtils.Separator();
	}
	
	void AddReward(Reward reward)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Reward");
		
		reward.Preffix = (PreffixType)EditorGUILayout.EnumPopup(reward.Preffix, GUILayout.Width(300));
		
		switch (reward.Preffix)
		{
			case PreffixType.ARMOR:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.armorEditor.items, "Armor", 90, FieldTypeEnum.Middle);
				break;
			
			case PreffixType.ATTRIBUTE:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.attributeEditor.items, "Attribute", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.ITEM:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.itemEditor.items, "Item", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.QUEST:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.WEAPON:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.weaponEditor.items, "Weapon", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.SKILL:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.skillEditor.items, "Skill", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.SPELL:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.skillEditor.items, "Spell", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.REPUTATION:
				reward.ItemId = EditorUtils.IntPopup(reward.ItemId, Data.reputationEditor.items, "Reputation", 90, FieldTypeEnum.Middle); 
				break;
		}
		
		EditorGUILayout.PrefixLabel(" ID: ");
		reward.ItemId = EditorGUILayout.IntField(reward.ItemId, GUILayout.Width(90));
		EditorGUILayout.PrefixLabel(" amount: ");
		reward.Amount = EditorGUILayout.IntField(reward.Amount, GUILayout.Width(100));
		
		EditorGUILayout.EndHorizontal();
	}
	
	void AddQuestStep(QuestStep questStep)
	{
		if (GUILayout.Button("Add task", GUILayout.Width(400))) 
		{
			questStep.Tasks.Add(new Task());
		}
		
		foreach(Task task in questStep.Tasks)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Task type");
		
			task.TaskType = (TaskTypeEnum)EditorGUILayout.EnumPopup(task.TaskType, GUILayout.Width(300));
			
			switch(task.TaskType)
			{
				case TaskTypeEnum.BringItem:
					task.PreffixTarget = (PreffixType)EditorGUILayout.EnumPopup(task.PreffixTarget, GUILayout.Width(200));
				
					switch(task.PreffixTarget)
					{
						case PreffixType.ITEM:
							task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.itemEditor.items, "Item", 100, FieldTypeEnum.Middle);
							break;
						case PreffixType.ARMOR:
							task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.armorEditor.items, "Armor", 100, FieldTypeEnum.Middle);
							break;
						case PreffixType.WEAPON:
							task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.weaponEditor.items, "Weapon", 100, FieldTypeEnum.Middle);
							break;
					}
					task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
					break;
				
				case TaskTypeEnum.KillEnemy:
					task.PreffixTarget = PreffixType.ENEMY;
					task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.enemyEditor.items, "Enemy", 100, FieldTypeEnum.Middle);
					
					break;
				
				case TaskTypeEnum.ReachPartOfConversation:
					task.PreffixTarget = PreffixType.PARAGRAPH;
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.conversationEditor.items, "Paragraph", 100, FieldTypeEnum.Middle);
					break;
				
				case TaskTypeEnum.TargetEnemy:
					task.PreffixTarget = PreffixType.ENEMY;
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.enemyEditor.items, "Enemy", 100, FieldTypeEnum.Middle);
					break;
				
				case TaskTypeEnum.VisitArea:
					task.PreffixTarget = PreffixType.WORLDOBJECT;
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.worldObjectEditor.items, "WO", 100, FieldTypeEnum.Middle);
					break;
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.PrefixLabel("Task quest log");
			task.QuestLogDescription = EditorGUILayout.TextField(task.QuestLogDescription, GUILayout.Width(500));
			EditorGUILayout.EndHorizontal();
		}
		
		EditorUtils.Separator();
	}
}
