using UnityEngine;
using UnityEditor;
using System.Collections;

public class EventUtils 
{
	public static void DisplayEvent(ActionEvent action, MainWindowEditor data)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Event type");
		action.ActionType = (ActionEventType)EditorGUILayout.EnumPopup(action.ActionType, GUILayout.Width(150));
		switch(action.ActionType)
		{
			//end conversation
			case ActionEventType.EndConversation : 
				break;
			
			//remove world object
			case ActionEventType.RemoveWorldObject:
				action.Item = EditorUtils.IntPopup(action.Item, data.worldObjectEditor.items, "World object", 90, FieldTypeEnum.Middle);
				break;
			
			//give item
			case ActionEventType.GiveItem:
			//take item
			case ActionEventType.TakeItem:
				action.PreffixItem = (PreffixType)EditorGUILayout.EnumPopup(action.PreffixItem, GUILayout.Width(200));
			
				switch (action.PreffixItem)
				{
					case PreffixType.ARMOR:
						action.Item = EditorUtils.IntPopup(action.Item, data.armorEditor.items, "Armor", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.ATTRIBUTE:
						action.Item = EditorUtils.IntPopup(action.Item, data.attributeEditor.items, "Attribute", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.ITEM:
						action.Item = EditorUtils.IntPopup(action.Item, data.itemEditor.items, "Item", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.QUEST:
						action.Item = EditorUtils.IntPopup(action.Item, data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.WEAPON:
						action.Item = EditorUtils.IntPopup(action.Item, data.weaponEditor.items, "Weapon", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.SKILL:
						action.Item = EditorUtils.IntPopup(action.Item, data.skillEditor.items, "Skill", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.SPELL:
						action.Item = EditorUtils.IntPopup(action.Item, data.skillEditor.items, "Spell", 90, FieldTypeEnum.Middle);
						break;
				
					case PreffixType.REPUTATION:
						action.Item = EditorUtils.IntPopup(action.Item, data.reputationEditor.items, "Reputation", 90, FieldTypeEnum.Middle); 
						break;
				}
				action.Amount = EditorUtils.IntField(action.Amount, "Amount: ", 90, FieldTypeEnum.Middle);
				break;
			
			//quest - alternate end
			case ActionEventType.QuestAlternateEnd:
				action.Item = EditorUtils.IntPopup(action.Item, data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
				
				action.Amount = EditorUtils.IntField(action.Amount, "End ID: ", 90, FieldTypeEnum.Middle);
				break;
			
			//quest start
			case ActionEventType.QuestStart:
			
			//quest failed
			case ActionEventType.QuestFailed:
			
			//quest end
			case ActionEventType.QuestEnd:
				action.Item = EditorUtils.IntPopup(action.Item, data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
				break;
			
			case ActionEventType.SpawnCreature:
				action.Item = EditorUtils.IntPopup(action.Item, data.spawnEditor.items, "Spawn point", 90, FieldTypeEnum.Middle);
				break;
			
			case ActionEventType.UseTeleport:
				action.Item = EditorUtils.IntPopup(action.Item, data.teleportEditor.items, "Teleport", 90, FieldTypeEnum.Middle);
				break;
			
			case ActionEventType.NoteDisplay:
				EditorGUILayout.EndHorizontal();
				action.Text = EditorUtils.TextField(action.Text, "Text", FieldTypeEnum.BeginningOnly);
				break;

            case ActionEventType.AddRace:
                action.Item = EditorUtils.IntPopup(action.Item, data.raceEditor.items, "Race", 90, FieldTypeEnum.Middle);
                break;
		}
	}
}
