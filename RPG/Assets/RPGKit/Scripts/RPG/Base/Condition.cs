using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

public class Condition  
{
	[XmlAttribute (AttributeName = "ITH")]
	public string ItemToHave;
	[XmlAttribute (AttributeName = "ATR")]
	public int AmountToReach = 1;
	[XmlAttribute (AttributeName = "CT")]
	public ConditionTypeEnum ConditionType;
	[XmlAttribute (AttributeName = "SI")]
	public int SecondaryID;
	
	//only for GUI operations, not used in the game
	[XmlIgnore]
	public string SearchString;
	[XmlIgnore]
	public string SearchId;
	[XmlIgnore]
	public List<IItem> displayedItems;
	[XmlIgnore]
	public bool FirstLoad;
	
	[XmlIgnore]
	public int ID
	{
		get
		{
			return Convert.ToInt32(ItemToHave);
		}
	}
	
	public Condition()
	{
		displayedItems = new List<IItem>();
		Clear();
		FirstLoad = true;
	}
	
	public bool Validate(Player player)
	{
		switch(ConditionType)
		{
			//items must be in inventory
			case ConditionTypeEnum.SomeItemMustBeInInventory :
				return player.Hero.Inventory.DoYouHaveThisItem(ItemToHave, AmountToReach);
			
			//quest not started
			case ConditionTypeEnum.QuestNotStarted: 
				return !player.Hero.Quest.IsQuestStarted(Convert.ToInt32(ItemToHave));
			
			//quest startet not finished (some of the tasks are not completed)
			case ConditionTypeEnum.QuestInProgress: 
				return player.Hero.Quest.IsQuestInProgress(Convert.ToInt32(ItemToHave));
			
			//quest finished = you can end it now
			case ConditionTypeEnum.QuestFinished: 
				return player.Hero.Quest.IsQuestFinished(Convert.ToInt32(ItemToHave));
			
			//quest completed = in quest log "quest completed"
			case ConditionTypeEnum.QuestCompleted:
				return player.Hero.Quest.IsQuestCompleted(Convert.ToInt32(ItemToHave));	
			
			//maximum level
			case ConditionTypeEnum.LevelMaximum:
				if (player.Hero.CurrentLevel <= AmountToReach)
					return true;
				break;
			
			//minimum level
			case ConditionTypeEnum.LevelMinimum:
				if (player.Hero.CurrentLevel >= AmountToReach)
					return true;
                break;

			//killed enemy in history
			case ConditionTypeEnum.KillTarget:	
				return player.Hero.Log.IsTargetKilled(Convert.ToInt32(ItemToHave), AmountToReach);
			
			//attribute point
			case ConditionTypeEnum.AttributePoint:
				if (player.Hero.AttributePoint >= AmountToReach)
					return true;
				break;
			
			//skill point
			case ConditionTypeEnum.SkillPoint:
				if (player.Hero.SkillPoint >= AmountToReach)
					return true;
				break;
			
			//base attribute
			case ConditionTypeEnum.BaseAttribute:
				foreach(RPGAttribute atr in player.Hero.Attributes)
				{
					if (atr.ID == Convert.ToInt32(ItemToHave) && atr.Value >= AmountToReach)
						return true;
				}
				break;
			
			//total attribute
			case ConditionTypeEnum.TotalAttribute:
				foreach(RPGAttribute atr in player.Hero.Bonuses.TotalAttributes)
				{
					if (atr.ID == Convert.ToInt32(ItemToHave) && atr.Value >= AmountToReach)
						return true;
				}
				break;
			
			//base skill
			case ConditionTypeEnum.BaseSkill:
				foreach(RPGSkill skill in player.Hero.Skills)
				{
					if (skill.ID == Convert.ToInt32(ItemToHave) && skill.Value >= AmountToReach)
						return true;
				}
				break;
			
			//total skill
			case ConditionTypeEnum.TotalSkill:
				foreach(RPGSkill skill in player.Hero.Bonuses.TotalSkills)
				{
					if (skill.ID == Convert.ToInt32(ItemToHave) && skill.Value >= AmountToReach)
						return true;
				}
				break;
			
			//total number of quests completed
			case ConditionTypeEnum.CompletedQuestsCount:
				if (player.Hero.Quest.CompletedQuests.Count >= AmountToReach)
					return true;
				break;
			
			//required race
			case ConditionTypeEnum.RaceRequired:
				if (player.Hero.RaceID == Convert.ToInt32(ItemToHave))
					return true;
				break;
			
			//race is not allowed
			case ConditionTypeEnum.RaceNotAllowed:
				if (player.Hero.RaceID != Convert.ToInt32(ItemToHave))
					return true;
				break;
			
			//required class
			case ConditionTypeEnum.ClassRequired:
				if (player.Hero.ClassID == Convert.ToInt32(ItemToHave))
					return true;
				break;
			
			//race is not allowed
			case ConditionTypeEnum.ClassNotAllowed:
				if (player.Hero.ClassID != Convert.ToInt32(ItemToHave))
					return true;
				break;
			
			//guild member
			case ConditionTypeEnum.IsGuildMember:
				if (player.Hero.Guild.IsMemberGuild(Convert.ToInt32(ItemToHave)))
					return true;
				break;
				
			//not guild member				
			case ConditionTypeEnum.IsNotGuildMember:
                if (!player.Hero.Guild.IsMemberGuild(Convert.ToInt32(ItemToHave)))
					return true;
				break;
			
			//alternate quest completed
			case ConditionTypeEnum.AlternatedQuestCompleted:
				if (player.Hero.Quest.IsAlternateQuestCompleted(Convert.ToInt32(ItemToHave), SecondaryID))
					return true;
				break;
			
			//alternate quest completed
			case ConditionTypeEnum.QuestFailed:
				if (player.Hero.Quest.IsQuestFailed(Convert.ToInt32(ItemToHave)))
					return true;
				break;
			
			//quest step in progress
			case ConditionTypeEnum.QuestStepInProgress:
				if (player.Hero.Quest.IsQuestStepInProgress(Convert.ToInt32(ItemToHave), SecondaryID))
					return true;
				break;
			
			//adding reputation
			case ConditionTypeEnum.ReputationValue:
				if (player.Hero.IsReputationValue(Convert.ToInt32(ItemToHave), AmountToReach))
					return true;
				break;

            //armor equiped
            case ConditionTypeEnum.ArmorEquiped:
                foreach (EquipedItem item in player.Hero.Equip.Items)
                {
                    if (item.UniqueItemId == PreffixType.ARMOR.ToString() + ItemToHave)
                        return true;
                }
                break;
            
                //weapon equiped
            case ConditionTypeEnum.WeaponEquiped:
                foreach (EquipedItem item in player.Hero.Equip.Items)
                {
                    if (item.UniqueItemId == PreffixType.WEAPON.ToString() + ItemToHave)
                        return true;
                }
                break;
			
		}
		
		return false;
	}
	
	public float TooltipPart(float x, float y, GUISkin skin, List<Condition> conditions, Player player)
	{
		foreach(Condition c in conditions)
		{
			Rect rect = new Rect(x + 10, y, 340, 20);
            string text = string.Empty;
			switch(c.ConditionType)
			{
				case ConditionTypeEnum.BaseAttribute:
				
					foreach(RPGAttribute atr in Player.Data.Attributes)
					{
						if (atr.ID.ToString() == ItemToHave)
						{
                            text = "Required base " + atr.Name + ": " + AmountToReach.ToString();
							break;
						}
					}
					
					break;
				
				case ConditionTypeEnum.TotalAttribute:
				
					foreach(RPGAttribute atr in Player.Data.Attributes)
					{
						if (atr.ID.ToString() == ItemToHave)
						{
                            text = "Required " + atr.Name + ": " + AmountToReach.ToString();
							break;
						}
					}
					break;
					
				case ConditionTypeEnum.BaseSkill:	
				
					foreach(RPGSkill s in Player.Data.Skills)
					{
						if (s.ID.ToString() == ItemToHave)
						{
							text = "Required base " + s.Name + ": " + AmountToReach.ToString();
							break;
						}
					}
					break;
				
				case ConditionTypeEnum.TotalSkill:
					foreach(RPGSkill s in Player.Data.Skills)
					{
						if (s.ID.ToString() == ItemToHave)
						{
							text = "Required " + s.Name + ": " + AmountToReach.ToString();
							break;
						}
					}
					break;
				
				case ConditionTypeEnum.CompletedQuestsCount:
                    text = "Required completed quests: " + AmountToReach.ToString();
					break;
				
				case ConditionTypeEnum.IsGuildMember:
					foreach(RPGGuild g in Player.Data.Guilds)
					{
						if (g.ID.ToString() == ItemToHave)
						{
							text = "Required: " + g.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.IsNotGuildMember:
					foreach(RPGGuild g in Player.Data.Guilds)
					{
						if (g.ID.ToString() == ItemToHave)
						{
							text = "Not allowed: " + g.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.ClassRequired:
					foreach(RPGCharacterClass cl in Player.Data.Classes)
					{
						if (cl.ID.ToString() == ItemToHave)
						{
							text = "Required: " + cl.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.ClassNotAllowed:
					foreach(RPGCharacterClass cl in Player.Data.Classes)
					{
						if (cl.ID.ToString() == ItemToHave)
						{
							text = "Not allowed: " + cl.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.RaceRequired:
                    foreach (RPGCharacterRace cl in Player.Data.Races)
					{
						if (cl.ID.ToString() == ItemToHave)
						{
							text = "Required: " + cl.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.RaceNotAllowed:
                    foreach (RPGCharacterRace cl in Player.Data.Races)
					{
						if (cl.ID.ToString() == ItemToHave)
						{
							text = "Not allowed: " + cl.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.ReputationValue:
					foreach(RPGReputation cl in Player.Data.Reputations)
					{
						if (cl.ID.ToString() == ItemToHave)
						{
							text = "Repuation: " + cl.Name;
							break;
						}
					} 
					break;
				
				case ConditionTypeEnum.LevelMaximum:
                    text = "Level less than " + AmountToReach.ToString();
					break;
				
				case ConditionTypeEnum.LevelMinimum:
                    text = "Required level: " + AmountToReach.ToString();
					break;

                case ConditionTypeEnum.ArmorEquiped:
                    foreach (RPGArmor cl in Player.Data.Armors)
                    {
                        if (cl.ID.ToString() == ItemToHave)
                        {
                            text = cl.Name + " must be equiped";
                            break;
                        }
                    } 
                    break;

                case ConditionTypeEnum.WeaponEquiped:
                    foreach (RPGWeapon cl in Player.Data.Weapons)
                    {
                        if (cl.ID.ToString() == ItemToHave)
                        {
                            text = cl.Name + " must be equiped";
                            break;
                        }
                    }
                    break;
			}
            DisplayCondition(rect, skin, text, player);	
			y += player.Hero.Settings.TooltipRowSize;
		}
		return y;
	}
	
	private void DisplayCondition(Rect rect, GUISkin skin, string text, Player player)
	{
		GUIStyle style = skin.label;
		Color previousColor = style.normal.textColor;
        if (Validate(player))
		{
			style.normal.textColor = Color.green;
		}
		else
		{
			style.normal.textColor = Color.red;
		}
		
		GUI.Label(rect, text, style);
		style.normal.textColor = previousColor;
	}
	
	public void Clear()
	{
		SearchString = string.Empty;
		SearchId = string.Empty;
		ItemToHave = string.Empty;
	}
	
	public void Search(List<IItem> items)
	{
		displayedItems = new List<IItem>();
		foreach(IItem item in items)
		{
			if (item.Name.ToLower().IndexOf(SearchString.ToLower()) != -1)
			{
				displayedItems.Add(item);
			}
		}
	}
	
	public void PrepareSearch(string temp, List<IItem> list)
	{
		if (temp != SearchString && string.IsNullOrEmpty(temp))
		{
			displayedItems = list;
			Clear();
		}
		
		if (temp != SearchString)
		{
			SearchString = temp;
			Search(list);
		}
	}
}

public enum ConditionTypeEnum
{
	SomeItemMustBeInInventory = 0,
	QuestNotStarted = 1,
	QuestInProgress = 2,
	QuestFinished = 3,
	QuestCompleted = 4,
	KillTarget = 5,
	TargetObject = 6,
	LevelMinimum = 7,
	LevelMaximum = 8,
	AttributePoint = 9,
	SkillPoint = 10,
	BaseAttribute = 11,
	BaseSkill = 12,
	TotalAttribute = 13,
	TotalSkill = 14,
	CompletedQuestsCount = 15,
	RaceRequired = 16,
	RaceNotAllowed = 17,
	ClassRequired = 18,
	ClassNotAllowed = 19,
	IsGuildMember = 20,
	IsNotGuildMember = 21,
	QuestStepInProgress = 22,
	AlternatedQuestCompleted = 23,
	QuestFailed = 24,
	ReputationValue = 25,
    ArmorEquiped = 26,
    WeaponEquiped = 27
}