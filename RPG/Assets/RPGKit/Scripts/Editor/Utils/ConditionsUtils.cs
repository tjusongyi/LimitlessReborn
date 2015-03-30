using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConditionsUtils 
{
	public static void Conditions(List<Condition> Conditions, MainWindowEditor Data)
	{
		foreach(Condition condition in Conditions)
		{
			AddCondition(condition, Data);
			
			if (GUILayout.Button("Delete", GUILayout.Width(120)))
			{
				Conditions.Remove(condition);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUILayout.Button("Add condition",GUILayout.Width(120)))
		{
			Conditions.Add(new Condition());
		}
	}
	
	private static void AddCondition(Condition condition, MainWindowEditor D)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Condition type");
		
		if (string.IsNullOrEmpty(condition.ItemToHave))
		{
			condition.ItemToHave = string.Empty;
		}
		
		bool reloadList = false;
		ConditionTypeEnum temp = condition.ConditionType;
		temp = (ConditionTypeEnum)EditorGUILayout.EnumPopup(temp, GUILayout.Width(300));
		if (temp != condition.ConditionType)
		{
			reloadList = true;
			condition.ConditionType = temp;
		}
		
		switch(condition.ConditionType)
		{
			case ConditionTypeEnum.AttributePoint:
				condition.ItemToHave = IntPopup(condition, D.attributeEditor.items, "Attribute", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.BaseAttribute:
				condition.ItemToHave = IntPopup(condition, D.attributeEditor.items, "Attribute", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.BaseSkill:
				condition.ItemToHave = IntPopup(condition, D.skillEditor.items, "Skill", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.ClassNotAllowed:
				condition.ItemToHave = IntPopup(condition, D.classEditor.items, "Class", 150, reloadList);
				break;
			
			case ConditionTypeEnum.ClassRequired:
                condition.ItemToHave = IntPopup(condition, D.classEditor.items, "Class", 150, reloadList);
				break;
			
			case ConditionTypeEnum.KillTarget:
				condition.ItemToHave = IntPopup(condition, D.enemyEditor.items, "Enemy", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.LevelMaximum:
                EditorGUILayout.PrefixLabel(" Level: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.LevelMinimum:
                EditorGUILayout.PrefixLabel(" Level: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.CompletedQuestsCount:
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.QuestCompleted:
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.QuestFinished:
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.QuestInProgress:
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.QuestNotStarted:
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.RaceNotAllowed:
				condition.ItemToHave = IntPopup(condition, D.raceEditor.items, "Race", 150, reloadList);
				break;
			
			case ConditionTypeEnum.RaceRequired:
				condition.ItemToHave = IntPopup(condition, D.raceEditor.items, "Race", 150, reloadList);
				break;
			
			case ConditionTypeEnum.SkillPoint:
				condition.ItemToHave = IntPopup(condition, D.skillEditor.items, "Skill", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.SomeItemMustBeInInventory:
				EditorGUILayout.PrefixLabel(" Unique ID: ");
				condition.ItemToHave = EditorGUILayout.TextField(condition.ItemToHave, GUILayout.Width(100));

                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.TargetObject:
				break;
			
			case ConditionTypeEnum.TotalAttribute:
				condition.ItemToHave = IntPopup(condition, D.attributeEditor.items, "Attribute", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.TotalSkill:
				condition.ItemToHave = IntPopup(condition, D.skillEditor.items, "Skill", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
				break;
			
			case ConditionTypeEnum.QuestStepInProgress:
				condition.SecondaryID = EditorUtils.IntField(condition.SecondaryID, "Step number", 40, FieldTypeEnum.Middle);
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.AlternatedQuestCompleted:
				condition.SecondaryID = EditorUtils.IntField(condition.SecondaryID, "End ID", 40, FieldTypeEnum.Middle);
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.QuestFailed:
				condition.ItemToHave = IntPopup(condition, D.questEditor.items, "Quest", 150, reloadList);
				break;
			
			case ConditionTypeEnum.ReputationValue:
				condition.ItemToHave = IntPopup(condition, D.reputationEditor.items, "Reputation", 150, reloadList);
                EditorGUILayout.PrefixLabel(" amount: ");
		        condition.AmountToReach = EditorGUILayout.IntField(condition.AmountToReach, GUILayout.Width(100));
                break;

            case ConditionTypeEnum.WeaponEquiped:
                EditorGUILayout.PrefixLabel(" Unique ID: ");
				condition.ItemToHave = EditorGUILayout.TextField(condition.ItemToHave, GUILayout.Width(100));
				break;

            case ConditionTypeEnum.ArmorEquiped:
                EditorGUILayout.PrefixLabel(" Unique ID: ");
				condition.ItemToHave = EditorGUILayout.TextField(condition.ItemToHave, GUILayout.Width(100));
                break;
		}
		
		
	}
	
	private static string IntPopup(Condition condition, List<IItem> list, string label, int width, bool reloadList)
	{
		int fieldValue = 0;
		if (!string.IsNullOrEmpty(condition.ItemToHave))
		{
			fieldValue = Convert.ToInt32(condition.ItemToHave);	
		}
		
		if (condition.FirstLoad || reloadList)
		{
			condition.FirstLoad = false;
			condition.displayedItems = list;
		}
		
		string[] names = new string[condition.displayedItems.Count];
		int[] ID = new int[condition.displayedItems.Count];
		int index = 0;
		foreach(IItem s in condition.displayedItems)
		{
			names[index] = s.Name;
			ID[index] = s.ID;
			index++;
		}
		string temp = condition.SearchString;
		temp = EditorUtils.TextField(temp, "Search", 60, FieldTypeEnum.Middle);
		EditorGUILayout.PrefixLabel(label); 
		
		fieldValue = EditorGUILayout.IntPopup(fieldValue, names, ID ,GUILayout.Width(width));
		
		condition.PrepareSearch(temp, list);
		
		return fieldValue.ToString();
	}
}
