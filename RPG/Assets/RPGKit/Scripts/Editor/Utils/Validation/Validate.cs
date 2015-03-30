using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Validate
{
	public Rect position;
	private Vector2 scrollPosition;
	private List<XMLError> Errors;
	
	private BaseEditorWindow currentEditor;
	private int currentID;
	private string currentString;
	private string startErrorString;
	private MainWindowEditor e;
	private MainWindowTypeEnum currentWindowType;
	
	public void ValidateXML(MainWindowEditor editor)
	{
		Errors = new List<XMLError>();
		e = editor;
		//armor
		currentEditor = editor.armorEditor;
		currentWindowType = MainWindowTypeEnum.Armor;
		foreach(RPGArmor armor in editor.armorEditor.Armors)
		{
			currentID = armor.ID;
			currentString = "effects on hit";
			CheckEffects(armor.EffectsOnHit);
			
			CheckEquiped(armor);
			
			CheckRPGItem(armor);
		}
		
		//weapon
		currentEditor = editor.weaponEditor;
		currentWindowType = MainWindowTypeEnum.Weapon;
		foreach(RPGWeapon w in editor.weaponEditor.Weapons)
		{
			currentID = w.ID;
			currentString = "hit effects";
			CheckEffects(w.EffectsHit);
			
			CheckEquiped(w);
			
			CheckRPGItem(w);
		}
		
		//item
		currentEditor = editor.itemEditor;
		currentWindowType = MainWindowTypeEnum.Item;
		foreach(RPGItem w in editor.itemEditor.Items)
		{
			currentID = w.ID;
			CheckRPGItem(w);
		}
		
		//spell
		currentEditor = editor.spellEditor;
		foreach(RPGSpell spell in editor.spellEditor.Spells)
		{
			currentID = spell.ID;
			
			if (!e.skillEditor.CheckIDExist(spell.SkillId))
			{
				currentString = " required skill "; 
				AddError();
			}
			
			CheckUsableItem(spell);
		}
		
		//container
		currentEditor = editor.containerEditor;
		currentWindowType = MainWindowTypeEnum.Container;
		foreach(RPGContainer container in editor.containerEditor.Containers)
		{
			currentID = container.ID;
			foreach(ContainerCategory cc in container.Categories)
			{
				CheckBaseLootCategory(cc);
			}
			
			foreach(ContainerItem ci in container.ContainerItems)
			{
				CheckBaseLootItem(ci);
			}
		}
		
		//conversation
		currentEditor = editor.conversationEditor;
		currentWindowType = MainWindowTypeEnum.Conversation;
		foreach(RPGParagraph p in editor.conversationEditor.Paragraphs)
		{
			currentID = p.ID;
			currentString = "Paragraph conditions";
			CheckConditions(p.Conditions);
			
			currentString ="Paragraph actions";
			CheckActionEvent(p.Actions);
			
			foreach(LineText lt  in p.LineTexts)
			{
				currentString = "Line text conditions";
				CheckConditions(lt.Conditions);
			
				currentString ="Line text actions";
				CheckActionEvent(lt.GameEvents);
			}
		}
		
		//enemy
		currentEditor = editor.enemyEditor;
		currentWindowType = MainWindowTypeEnum.Enemy;
		foreach(RPGEnemy enemy in editor.enemyEditor.Enemies)
		{
			currentID = enemy.ID;
			
			RPGContainer container = enemy.Container;
			foreach(ContainerCategory cc in container.Categories)
			{
				CheckBaseLootCategory(cc);
			}
			
			foreach(ContainerItem ci in container.ContainerItems)
			{
				CheckBaseLootItem(ci);
			}
		}
		
		//NPC
		currentEditor = editor.npcEditor;
		currentWindowType = MainWindowTypeEnum.NPC;
		foreach(RPGNPC npc in editor.npcEditor.NPC)
		{
			currentID = npc.ID;
			
			currentString = "Shop conditions";
			CheckConditions(npc.ShopConditions);
			
			currentString = "Spell shop conditions";
			CheckConditions(npc.SpellShopConditions);
			
			currentString = "Repair conditions";
			CheckConditions(npc.ReparingConditions);
		
			currentString = "shop"; 
			if (!e.shopEditor.CheckIDExist(npc.ShopID) && npc.ShopID > 0)
			{
				AddError();
			}
			
			currentString = "spell shop"; 
			if (!e.spellShop.CheckIDExist(npc.SpellShopID) && npc.SpellShopID > 0)
			{
				AddError();
			}
			
			currentString = "guild"; 
			if (!e.guildEditor.CheckIDExist(npc.GuildID))
			{
				AddError();
			}
		}
		
		//guild
		currentEditor = editor.guildEditor;
		currentWindowType = MainWindowTypeEnum.Guild;
		foreach(RPGGuild guild in editor.guildEditor.Guilds)
		{
			currentString = "Join conditions";
			CheckConditions(guild.Conditions);
		}
		
		//race
		currentEditor = editor.raceEditor;
		currentWindowType = MainWindowTypeEnum.Race;
        foreach (RPGCharacterRace race in editor.raceEditor.Races)
		{
			currentString = "Race effects";
			CheckEffects(race.Effects);
		}
		
		//shop
		currentEditor = editor.shopEditor;
		currentWindowType = MainWindowTypeEnum.Shop;
		foreach(Shop shop in editor.shopEditor.Shops)
		{
			currentID = shop.ID;
			foreach(BaseLootCategory cc in shop.Categories)
			{
				CheckBaseLootCategory(cc);
			}
			
			foreach(BaseLootCategory cc in shop.SellCategories)
			{
				CheckBaseLootCategory(cc);
			}
			
			foreach(BaseLootItem bb in shop.Items)
			{
				CheckBaseLootItem(bb);
			}
		}
		
		//spell shop
		currentEditor = editor.spellShop;
		currentWindowType = MainWindowTypeEnum.SpellShop;
		foreach(RPGSpellShop sp in editor.spellShop.SpellShops)
		{
			currentID = sp.ID;
			foreach(SpellShopCategory spc in sp.Categories)
			{
				currentString = "Category conditions";
				CheckConditions(spc.Conditions);
			}
			
			foreach(SpellShopItem spc in sp.Items)
			{
				currentString = "Spell conditions";
				CheckConditions(spc.Conditions);
				
				currentString = "Spell";
				if (!e.spellEditor.CheckIDExist(spc.SpellID))
				{
					AddError();
				}
			}
		}
		
		//world object
		currentEditor = editor.worldObjectEditor;
		currentWindowType = MainWindowTypeEnum.WorldObject;
		foreach(RPGWorldObject wo in editor.worldObjectEditor.WorldObjects)
		{
			currentString = "Conditions";
			CheckConditions(wo.Conditions);
		}
		
		//quest
		currentEditor = editor.questEditor;
		currentWindowType = MainWindowTypeEnum.Quest;
		foreach(RPGQuest q in editor.questEditor.Quests)
		{
			currentID = q.ID;
			
			foreach(Reward r in q.Rewards)
			{
				currentString = "Rewards";
				CheckGiveItem(r.Preffix, r.ItemId);
			}
			
			currentString = "Tasks";
			foreach(QuestStep qs in q.QuestSteps)
			{
				foreach(Task t in qs.Tasks)
				{
					switch(t.TaskType)
					{
						case TaskTypeEnum.BringItem:
							if (t.PreffixTarget == PreffixType.ITEM)
							{
								if (!e.itemEditor.CheckIDExist(t.TaskTarget))
								{
									currentString += " - item - "; 
									AddError();
								}
							}
							else if (t.PreffixTarget == PreffixType.WEAPON)
							{
								if (!e.weaponEditor.CheckIDExist(t.TaskTarget))
								{
									currentString += " - weapon - "; 
									AddError();
								}
							}
							else if (t.PreffixTarget == PreffixType.ARMOR)
							{
								if (!e.armorEditor.CheckIDExist(t.TaskTarget))
								{
									currentString += " - armor - "; 
									AddError();
								}
							}
							break;
						
						case TaskTypeEnum.KillEnemy:
							if (!e.enemyEditor.CheckIDExist(t.TaskTarget))
							{
								currentString += " - task kill enemy - "; 
								AddError();
							}
							break;
						
						case TaskTypeEnum.ReachPartOfConversation:
							if (!e.conversationEditor.CheckIDExist(t.TaskTarget) && t.PreffixTarget == PreffixType.PARAGRAPH)
							{
								currentString += " - task conversation paragraph - "; 
								AddError();
							}
							else if (t.PreffixTarget == PreffixType.LINETEXT)
							{
								bool founded = false;
								foreach(RPGParagraph p in e.conversationEditor.Paragraphs)
								{
									foreach(LineText lt in p.LineTexts)
									{
										if (lt.ID == t.TaskTarget)
										{
											founded = true;
											break;
										}
									}
								}
								if (!founded)
								{
									currentString += " - task conversation line text - "; 
									AddError();
								}
							}
							break;
						
						case TaskTypeEnum.VisitArea:
							if (!e.worldObjectEditor.CheckIDExist(t.TaskTarget))
							{
								currentString += " - task visit area - "; 
								AddError();
							}
							break;
					}
				}
			}
		}
		
		//teleport 
		currentEditor = editor.teleportEditor;
		currentWindowType = MainWindowTypeEnum.Teleport;
		foreach(RPGTeleport t in editor.teleportEditor.Teleports)
		{
			currentID = t.ID;
			currentString = "Conditions";
			CheckConditions(t.Conditions);
		}
		
	}
	
	private void CheckBaseLootCategory(BaseLootCategory category)
	{
		currentString = "Category condition";
		CheckConditions(category.Conditions);
		
		currentString = "Category";
		if (!e.itemCategory.CheckIDExist(category.Category.ID))
		{
			AddError();
		}
	}
	
	private void CheckBaseLootItem(BaseLootItem item)
	{
		currentString = "Loot item conditions";
		CheckConditions(item.Conditions);
		
		if (item.Preffix == ItemTypeEnum.ITEM)
		{
			if (!e.itemEditor.CheckIDExist(item.ID))
			{
				currentString += " - item - "; 
				AddError();
			}
		}
		else if (item.Preffix == ItemTypeEnum.WEAPON)
		{
			if (!e.weaponEditor.CheckIDExist(item.ID))
			{
				currentString += " - weapon - "; 
				AddError();
			}
		}
		else if (item.Preffix == ItemTypeEnum.ARMOR)
		{
			if (!e.armorEditor.CheckIDExist(item.ID))
			{
				currentString += " - armor - "; 
				AddError();
			}
		}
	}
	
	private void CheckEquiped(Equiped equiped)
	{
		currentString = "wear conditions";
		CheckConditions(equiped.Conditions);
			
		currentString = "worn effects";
		CheckEffects(equiped.WornEffects);
			
		currentString = "equipment slots";
		CheckEquipmentSlot(equiped.EquipmentSlots);
	}
	
	private void CheckRPGItem(RPGItem rpgItem)
	{
		currentString = "item category";
		CheckItemCategory(rpgItem.Categories);
		
		CheckUsableItem(rpgItem);
	}
	
	private void CheckUsableItem(UsableItem usableItem)
	{
		currentString = "use conditions";
		CheckConditions(usableItem.UseConditions);
			
		currentString = "use effects";
		CheckEffects(usableItem.Effects);
	}
	
	private void CheckEffects(List<Effect> effects)
	{
		foreach(Effect effect in effects)
		{
			switch(effect.EffectType)
			{
				case EffectTypeEnum.Attribute:
					if (!e.attributeEditor.CheckIDExist(effect.ID))
					{
						currentString += " - Attribute - "; 
						AddError();
						continue;
					}
					break;
				
				case EffectTypeEnum.Skill:
					if (!e.skillEditor.CheckIDExist(effect.ID))
					{
						currentString += " - Skill - "; 
						AddError();
						continue;
					}
					break;
				
				case EffectTypeEnum.CastSpell:
					if (!e.spellEditor.CheckIDExist(effect.ID))
					{
						currentString += " - Spell - "; 
						AddError();
						continue;
					}
					break;
			}
		}
	}
	
	private void AddError()
	{
		XMLError error = new XMLError();
		error.ErrorText = currentEditor.EditorName + " " + currentID.ToString() + " " + currentString;
		error.ID = currentID;
		error.WindowType = currentWindowType;
		error.editor = currentEditor;
		Errors.Add(error);
	}
	
	private void CheckConditions(List<Condition> conditions)
	{
		foreach(Condition condition in conditions)
		{
			switch (condition.ConditionType)
			{
				case ConditionTypeEnum.QuestFailed:
				case ConditionTypeEnum.QuestCompleted:
				case ConditionTypeEnum.QuestFinished:
				case ConditionTypeEnum.QuestInProgress:
					if (!e.questEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Quest - "; 
						AddError();
						continue;
					}
					break;
				case ConditionTypeEnum.KillTarget:
					if (!e.enemyEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Enemy - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.TotalSkill:
				case ConditionTypeEnum.BaseSkill:
					if (!e.skillEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Skill - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.TotalAttribute:
				case ConditionTypeEnum.BaseAttribute:
					if (!e.attributeEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Attribute - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.RaceRequired:
				case ConditionTypeEnum.RaceNotAllowed:
					if (!e.raceEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Race - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.ClassRequired:
				case ConditionTypeEnum.ClassNotAllowed:
					if (!e.classEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Class - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.IsGuildMember:
				case ConditionTypeEnum.IsNotGuildMember:
					if (!e.guildEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Guild - "; 
						AddError();
						continue;
					}
					break;
				
				case ConditionTypeEnum.AlternatedQuestCompleted:
					if (!e.questEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Quest - "; 
						AddError();
						continue;
					}
				
					bool founded = false;
					foreach(RPGQuest q in e.questEditor.Quests)
					{
						if (q.ID == currentID && q.AlternateEnds.Count >= condition.SecondaryID)
							founded = true;
					}
				
					if (!founded)
					{
						currentString += " - not enough quest end - "; 
						AddError();
					}
					break;
				
				case ConditionTypeEnum.QuestStepInProgress:
					if (!e.questEditor.CheckIDExist(condition.ID))
					{
						currentString += " - Quest - "; 
						AddError();
						continue;
					}
				
					founded = false;
					foreach(RPGQuest q in e.questEditor.Quests)
					{
						if (q.ID == currentID && q.QuestSteps.Count >= condition.SecondaryID)
							founded = true;
					}
				
					if (!founded)
					{
						currentString += " - not enough quest step - "; 
						AddError();
					}
					break;
			}
		}
	}
	
	private void CheckActionEvent(List<ActionEvent> actionEvents)
	{
		foreach(ActionEvent ac in actionEvents)
		{
			switch(ac.ActionType)
			{
				case ActionEventType.QuestAlternateEnd:
					if (!e.questEditor.CheckIDExist(ac.Item))
					{
						currentString += " - Quest - "; 
						AddError();
						continue;
					}
					bool founded = false;
					foreach(RPGQuest q in e.questEditor.Quests)
					{
						if (q.ID == currentID && q.AlternateEnds.Count >= ac.Amount)
							founded = true;
					}
				
					if (!founded)
					{
						currentString += " - not enough quest end - "; 
						AddError();
					}
				
					break;
				case ActionEventType.QuestEnd:
				case ActionEventType.QuestFailed:
				case ActionEventType.QuestStart:
					if (!e.questEditor.CheckIDExist(ac.Item))
					{
						currentString += " - Quest - "; 
						AddError();
						continue;
					}
					break;
				
				case ActionEventType.RemoveWorldObject:
					if (!e.worldObjectEditor.CheckIDExist(ac.Item))
					{
						currentString += " - World object - "; 
						AddError();
						continue;
					}
					break;
				
				case ActionEventType.TakeItem:
					if (ac.PreffixItem == PreffixType.ITEM)
					{
						if (!e.itemEditor.CheckIDExist(ac.Item))
						{
							currentString += " - take item - "; 
							AddError();
							continue;
						}
					}
					else if (ac.PreffixItem == PreffixType.WEAPON)
					{
						if (!e.weaponEditor.CheckIDExist(ac.Item))
						{
							currentString += " - take weapon - "; 
							AddError();
							continue;
						}
					}
					else if (ac.PreffixItem == PreffixType.ARMOR)
					{
						if (!e.armorEditor.CheckIDExist(ac.Item))
						{
							currentString += " - take armor - "; 
							AddError();
							continue;
						}
					}
					break;
				case ActionEventType.GiveItem:
					CheckGiveItem(ac.PreffixItem, ac.Item);
					break;
			}
		}
	}
	
	private void CheckGiveItem(PreffixType preffix, int ID)
	{
		if (preffix == PreffixType.ITEM)
		{
			if (!e.itemEditor.CheckIDExist(ID))
			{
				currentString += " - give item - "; 
				AddError();
			}
		}
		else if (preffix == PreffixType.WEAPON)
		{
			if (!e.weaponEditor.CheckIDExist(ID))
			{
				currentString += " - give weapon - "; 
				AddError();
			}
		}
		else if (preffix == PreffixType.ARMOR)
		{
			if (!e.armorEditor.CheckIDExist(ID))
			{
				currentString += " - give armor - "; 
				AddError();
			}
		}
		else if (preffix == PreffixType.SKILL)
		{
			if (!e.skillEditor.CheckIDExist(ID))
			{
				currentString += " - give skill - "; 
				AddError();
			}
		}
		else if (preffix == PreffixType.QUEST)
		{
			if (!e.questEditor.CheckIDExist(ID))
			{
				currentString += " - give quest - "; 
				AddError();
			}
		}
		else if (preffix == PreffixType.ATTRIBUTE)
		{
			if (!e.attributeEditor.CheckIDExist(ID))
			{
				currentString += " - give attribute - "; 
				AddError();
			}
		}
	}
	
	private void CheckItemCategory(List<RPGItemCategory> categories)
	{
		foreach(RPGItemCategory category in categories)
		{
			if (!e.itemCategory.CheckIDExist(category.ID))
			{
				currentString += " - Category - "; 
				AddError();
				continue;
			}
		}
	}
	
	private void CheckEquipmentSlot(List<RPGEquipmentSlot> slots)
	{
		foreach(RPGEquipmentSlot slot in slots)
		{
			if (!e.equipmentSlotEditor.CheckIDExist(slot.ID))
			{
				currentString += " - Equipment slot - "; 
				AddError();
				continue;
			}
		}
	}
	
	public void DisplayErrors()
	{
		if (Errors == null)
			return;
		
		GUILayout.BeginArea(new Rect(5,125, position.width - 10, Screen.height - 200));
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		foreach(XMLError error in Errors)
		{
			if (GUILayout.Button(error.ErrorText))
			{
				e.MainWindowType = error.WindowType;
				error.editor.SelectItemBYID(error.ID);
				e.isValidateDisplay = false;
			}
		}
		
		EditorGUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
