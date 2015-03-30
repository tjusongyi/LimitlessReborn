using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class ActionEvent  
{
	
	[XmlAttribute (AttributeName = "PI")]
	public PreffixType PreffixItem;
	
	[XmlAttribute (AttributeName = "ITEM")]
	public int Item;
	
	[XmlAttribute (AttributeName = "AT")]
	public ActionEventType ActionType;
	
	[XmlAttribute (AttributeName = "AM")]
	public int Amount;
	
	[XmlAttribute (AttributeName = "TX")]
	public string Text;
	
	public ActionEvent()
	{
		Text = string.Empty;
	}
	
	//do action based on parameters
	public void DoAction(Player player)
	{
		switch(ActionType)
		{
			//quest start
			case ActionEventType.QuestStart:
			 	player.Hero.Quest.StartQuest(Item);
				break;
			//quest end
			case ActionEventType.QuestEnd:
                player.Hero.Quest.EndQuest(Item, player);
				break;
			//give item
			case ActionEventType.GiveItem:
				PreffixSolver.GiveItem(PreffixItem, Item, Amount, player);
				break;
			//take item from inventory
			case ActionEventType.TakeItem:
				RPGItem rpgItem = new RPGItem();
				if (PreffixItem == PreffixType.ITEM)
				{
					rpgItem = Storage.LoadById<RPGItem>(Item, new RPGItem());
				}
				else if (PreffixItem == PreffixType.WEAPON)
				{
					rpgItem = Storage.LoadById<RPGWeapon>(Item, new RPGWeapon());
				}
				else if (PreffixItem == PreffixType.ARMOR)
				{
					rpgItem = Storage.LoadById<RPGArmor>(Item, new RPGArmor());
				}
				if (Amount == 0)
					Amount = 1;
                player.Hero.Inventory.RemoveItem(rpgItem, Amount, player);
				break;
			//end conversation
			case ActionEventType.EndConversation:
				BasicGUI.isConversationDisplayed = false;
				break;
			//remove worldobject
			case ActionEventType.RemoveWorldObject:
                player.Hero.ActionsToDo.Add(this);
                player.doEvents = true;
				break;
			//quest - alternate end
			case ActionEventType.QuestAlternateEnd:
                player.Hero.Quest.AlternateEndQuest(Item, Amount);
				break;
			//spawn creature
			case ActionEventType.SpawnCreature:
                player.Hero.ActionsToDo.Add(this);
                player.doEvents = true;
				break;
			//note display
			case ActionEventType.NoteDisplay:
                player.Hero.ActionsToDo.Add(this);
                player.doEvents = true;
				break;
			//use teleport
			case ActionEventType.UseTeleport:
                player.Hero.ActionsToDo.Add(this);
                player.doEvents = true;
				break;
            //add race
            case ActionEventType.AddRace:
                player.Hero.RaceID = Item;
                break;

			
		}
	}
}

public enum ActionEventType
{
	QuestStart = 0,
	QuestEnd = 1,
	GiveItem = 2,
	TakeItem = 3,
	EndConversation = 4,
	QuestFailed = 5,
	RemoveWorldObject = 6,
	QuestAlternateEnd = 7,
	NoteDisplay = 8,
	SpawnCreature = 9,
	UseTeleport = 10,
    AddRace = 11
}