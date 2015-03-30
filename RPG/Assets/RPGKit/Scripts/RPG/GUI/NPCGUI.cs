using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class NPCGUI : BasicGUI 
{
	
	void Start()
	{
		Prepare();
	}
	
	private RPGParagraph paragraph = new RPGParagraph();
	private int previousParagraphId;
	private int previousNPCId;
	private int currentLongLineTextId = 0;
	private LineText lineText;
	private RPGNPC currentNPC;
	public static Shop currentShop;
	private RPGSpellShop currentSpellShop;
	private RPGGuild guild;
	private string byeText = "Farewell";
	private string beginConversation = "Lets discuss something else";
	
	int textWidth
	{
		get
		{
			return WindowWidth - 50;
		}
	}
	
	public static NPCMenuType MenuMode = NPCMenuType.Conversation;
	
	List<ItemInWorld> shopItems = new List<ItemInWorld>();
	
	void Awake()
	{
		paragraph = new RPGParagraph();
	}
	
	void LoadParagraph()
	{
		currentNPC = Storage.LoadById<RPGNPC>(Player.activeNPC, new RPGNPC());
        paragraph = new RPGParagraph();
		paragraph = paragraph.LoadByOwner(currentNPC, player);

        if (paragraph == null)
            paragraph = new RPGParagraph();

		paragraph.AddGeneralConversation(currentNPC);

        if (paragraph == null || paragraph.ID == 0)
            return;
		
		paragraph.DoEvents(player);
		previousNPCId = Player.activeNPC;
		player.Hero.Quest.CheckParagraph(paragraph.ID);
		
		if (currentNPC == null)
			currentNPC = new RPGNPC();
		if (currentNPC.ShopID > 0)
		{
			currentShop = Storage.LoadById<Shop>(currentNPC.ShopID, new Shop());
			shopItems = currentShop.GetItems(Player.activeNPC, player);
		}
		if (currentNPC.SpellShopID > 0)
		{
			currentSpellShop = Storage.LoadById<RPGSpellShop>(currentNPC.SpellShopID, new RPGSpellShop());
			currentSpellShop.LoadSpells(player);
			foreach(RPGSpell spell in currentSpellShop.Spells)
			{
				spell.LoadIcon();
			}
		}
	}
	
	void OnGUI()
	{
		//conversation must be activated
		if (!BasicGUI.isConversationDisplayed)
		{
			paragraph = new RPGParagraph();
			BasicGUI.isShopActivated = false;
			Player.sellPriceModifier = 0;
			return;
		}
		//if paragraph not loaded
		if (paragraph == null || paragraph.ID == 0 || previousNPCId != Player.activeNPC)
			LoadParagraph();
		
		if (paragraph == null)
			return;
		//box for NPC
		LeftBox(Player.ActiveNPCName);	
		
		if (MenuMode == NPCMenuType.Conversation)
		{
			//PlaySound();
			DisplayConversation();
		}
			
		if (MenuMode == NPCMenuType.Shop)
			DisplayShop();
		
		if (MenuMode == NPCMenuType.SpellShop)
			DisplaySpellShop();
		
		if (MenuMode == NPCMenuType.Repairing)
			DisplayRepair();
		
		if (MenuMode == NPCMenuType.Guild)
			DisplayGuild();
		
		if (MenuMode == NPCMenuType.LongLineTextConversation)
			AdditionalTexts();
	}
	
	void AdditionalTexts()
	{
		LongLineText lt = lineText.AdditionalTexts[currentLongLineTextId];
		
		Label(new Rect(LeftTopX + 20,LeftTopX +55,textWidth,90), lt.NPCText);
		string buttonText = lt.PlayerText;
		
		if (string.IsNullOrEmpty(buttonText))
			buttonText = "Continue please...";
		
		if (Button(new Rect(LeftTopX + 20,LeftTopY + 180, textWidth,35), buttonText))
		{
			if (currentLongLineTextId == lineText.AdditionalTexts.Count - 1)
			{
				LoadNextParagraph(lineText);
				MenuMode = NPCMenuType.Conversation;
			}
			else
			{
				currentLongLineTextId++;
			}
		}
		
		//display goodbye
		if (currentLongLineTextId == lineText.AdditionalTexts.Count - 1 && lt.IsEnd)
		{
			if (Button(new Rect(LeftTopX + 20,LeftTopY + 220, textWidth,35), byeText))
			{
				CloseConversation();
			}
		}
		
		DisplayNPCButtons();
	}
	
	void LoadNextParagraph(LineText lt)
	{
		player.Hero.Quest.CheckLineText(lt.ID);
        //do line text action
        lt.DoEvents(player);
        RPGParagraph p = RPGParagraph.LoadByParentLineText(lt.ID);

        if (p.ID == paragraph.ID)
            return;

        if (!string.IsNullOrEmpty(p.ParagraphText))
        {
            paragraph = p;
            paragraph.DoEvents(player);
            player.Hero.Quest.CheckParagraph(paragraph.ID);
        }
	}
	
	void DisplayConversation()
	{
		int playerLineTextIndex = 0;
		//display NPC sentence
		Label(new Rect(LeftTopX + 20,LeftTopX +55,textWidth,50), paragraph.ParagraphText);
		
		//display Player text
		foreach(LineText lt in paragraph.LineTexts)
		{
			//check if you can display that line text for player in current state
			if (lt.CanYouDisplay(player) && !string.IsNullOrEmpty(lt.Text))
			{
				playerLineTextIndex++;
				//player pressed that line text
				if (Button(new Rect(LeftTopX + 20,LeftTopY + 75 + (playerLineTextIndex * 40), textWidth,35), lt.Text))
				{
					//redirection for additional text (long line text) 
					if (lt.AdditionalTexts.Count > 0)
					{
						currentLongLineTextId = 0;
						lineText = lt;
						MenuMode = NPCMenuType.LongLineTextConversation;
						return;
					}
					
					LoadNextParagraph(lt);
				}
			}
		}
		
		//display conversation return (conversation return cannot be for first parargraph)
		if (paragraph.CanReturn && paragraph.ParentLineTextId > 0)
		{
			playerLineTextIndex++;
			if (Button(new Rect(LeftTopX + 20,LeftTopY + 75 + (playerLineTextIndex * 40), textWidth,35), beginConversation))
			{
                paragraph = paragraph.LoadByOwner(currentNPC, player);
				paragraph.AddGeneralConversation(currentNPC);
			}
		}	
		//display goodbye
		if (paragraph.CanEndParagraph)
		{
			playerLineTextIndex++;
			if (Button(new Rect(LeftTopX + 20,LeftTopY + 75 + (playerLineTextIndex * 40), textWidth,35), byeText))
			{
				CloseConversation();
			}
		}
		
		DisplayNPCButtons();
	}
	
	void DisplayNPCButtons()
	{
		int buttonIndex = 0;
		Rect downButtons;
		//shop button
		if (currentNPC.ShopID > 0 && currentNPC.IsShop(player))
		{
			downButtons = new Rect(LeftTopX + 20 + (buttonIndex * 50), LeftTopY + WindowHeight - 50, 40,40);
			if (Button(downButtons, "Shop"))
			{
				//set global values for calculating price for selling items
				MenuMode = NPCMenuType.Shop;
				BasicGUI.isShopActivated = true;
				Player.sellPriceModifier = currentShop.SellPriceModifier;
				isInventoryDisplayed = true;
			}
			buttonIndex++;
		}
		
		//spell button
        if (currentNPC.SpellShopID > 0 && currentNPC.IsSpellShop(player))
		{
			downButtons = new Rect(LeftTopX + 20 + (buttonIndex * 50), LeftTopY + WindowHeight - 50, 40,40);
			if (Button(downButtons, "Spell"))
			{
				//set global values for calculating price for selling items
				MenuMode = NPCMenuType.SpellShop;
			}
			buttonIndex++;
		}
		
		//repair button
        if (currentNPC.Repairing && currentNPC.IsRepair(player))
		{
			downButtons = new Rect(LeftTopX + 20 + (buttonIndex * 50), LeftTopY + WindowHeight - 50, 40,40);
			if (Button(downButtons, "Repair"))
			{
				//set global values for calculating price for selling items
				MenuMode = NPCMenuType.Repairing;
			}
			buttonIndex++;
		}
		
		//guild button
		if (currentNPC.GuildID > 0)
		{
			downButtons = new Rect(LeftTopX + 20 + (buttonIndex * 50), LeftTopY + WindowHeight - 50, 40,40);
			if (Button(downButtons, "Guild"))
			{
				guild = Storage.LoadById<RPGGuild>(currentNPC.GuildID, new RPGGuild());
				MenuMode = NPCMenuType.Guild;
			}
		}
	}
	
	void CloseConversation()
	{
		//audio.PlayOneShot(audio.clip);
		BasicGUI.isConversationDisplayed = false;
		BasicGUI.isShopActivated = false;
		Player.sellPriceModifier = 0;
		Player.currentItem = null;
		currentShop = null;
		currentSpellShop = null;
        player.ChangeMouseControl(true);
		MenuMode = NPCMenuType.Conversation;
	}
	
	bool BasicControls(string label)
	{
		Rect rectLabel = new Rect(LeftTopX + 10, LeftTopY +10, 300, 23);
		Label(rectLabel, label);
		float rightTopX = LeftTopX + WindowWidth - GUIItemButon - 10;
		Rect downButtons = new Rect(rightTopX, LeftTopY + WindowHeight - GUIItemButon - 10, GUIItemButon,GUIItemButon);
		if (Button(downButtons, "Back"))
		{
			MenuMode = NPCMenuType.Conversation;
		}
		downButtons = new Rect(rightTopX, LeftTopY+10, GUIItemButon,GUIItemButon);
		if (Button(downButtons, "End"))
		{
			CloseConversation();
			return true;
		}
		return false;
	}
	
	void DisplayShop()
	{
		//there is no shop
		if (currentNPC.ShopID == 0 || currentShop == null)
			return;
		if (BasicControls("You can buy these items"))
			return;
		
		int maxY = shopItems.Count / player.Hero.Inventory.sizeXOfInventory;
		if (maxY < player.Hero.Inventory.sizeYOfInventory)
			maxY = player.Hero.Inventory.sizeYOfInventory;
		else if (maxY > player.Hero.Inventory.sizeYOfInventory)
		{
			//begin scroll
		}
		int index =0;  
		//same grid like for inventory
		for(int x = 1; x <= player.Hero.Inventory.sizeXOfInventory; x++)
		{
			for (int y = 1; y <= player.Hero.Inventory.sizeYOfInventory;y++)
			{
				//button position
				int xpos = LeftTopX + 10 + (x * (GUIItemButon + 5));
				int ypos = LeftTopY + (y * (GUIItemButon + 5)) - 10;
				
				if (shopItems.Count >= index +1)
				{
					ItemInWorld item = shopItems[index];
					if (!item.IsItemLoaded(currentShop.BuyPriceModifier))
						continue;
					Rect r = new Rect(xpos,ypos, GUIItemButon, GUIItemButon);
					Texture2D texture = (Texture2D)item.rpgItem.Icon;
					
					if (Button(r, texture, "TOOLTIP"))
					{
						
						//right mouse button is shopping
						if (Event.current.button == 1)
						{
							//sell multiple items
							if (item.CurrentAmount > 1)
							{
								BasicGUI.isQuantityActivated = true;
								QuantityGUI.MaximumValue = item.CurrentAmount;
								QuantityGUI.QuantityType = QuantityTypeEnum.Buy;
								Player.currentItem = item.rpgItem;
							}
							else
							{
								//buy item
								BuyTransaction buyTransaction = currentShop.BuyItem(item.rpgItem, 1, player);
								if (buyTransaction == BuyTransaction.NotEnoughGold)
								{
									//display error
									return;
								}
								if (buyTransaction == BuyTransaction.OK)
								{
									PlayGUISound(GUISound.Gold);
								}
							}
						}
					}
					if (IsMouseOverRect(r))
					{
						InventoryGUI.tooltip = item;
						InventoryGUI.IsToolTip = true;
					}
					//show amount
					if (item.CurrentAmount > 1)
					{
						r = new Rect(xpos+3, ypos + GUIItemButon -15, 30,20);
						Label(r, item.CurrentAmount.ToString());
					}
					
				}
				else
				{
					//empty shopping slot
					Button(new Rect(xpos,ypos, GUIItemButon, GUIItemButon), "slot");
				}
				index++;
			}
		}
	}
	
	void DisplaySpellShop()
	{
		//there is no shop
		if (currentSpellShop == null || currentSpellShop.ID == 0)
			return;
		
		//can press back or end button
		if (BasicControls("You can buy these spells"))
			return;
		
		if (currentSpellShop.Spells == null || currentSpellShop.Spells.Count == 0)
		{
			Rect rectLabel = new Rect(LeftTopX + 20, LeftTopY +60, 200, 20);
			Label(rectLabel, "No spells");
			return;
		}
		
		if (currentSpellShop.Spells.Count == 0)
			return;
		int spellIndex = 1;
		foreach(RPGSpell spell in currentSpellShop.Spells)
		{
			//display box for spell	
			Rect button = new Rect(LeftTopX + 30, LeftTopY + 15 + (spellIndex * 50), WindowWidth - 130, 45); 
			if (Button(button, "", "TOOLTIP"))
			{
				if (Event.current.button == 1)
				{
					//buy spell
					if (currentSpellShop.BuySpell(spell, player))
						PlayGUISound(GUISound.Gold);
					currentSpellShop.LoadSpells(player);
					if (currentSpellShop.Spells.Count == 0)
						MenuMode = NPCMenuType.Conversation;
					return;
				}
			}
			DrawTexture(LeftTopX + 40, LeftTopY +20 + (spellIndex * 50), 40, 40, spell.Icon);
			
			Rect spellName = new Rect(LeftTopX + 90, LeftTopY +20 + (spellIndex * 50), 200, 40);
			Label(spellName, spell.Name + " " + (int)(spell.Price * currentSpellShop.BuyPriceModifier) + " " + player.Hero.Settings.CurrencyKeyword);
			spellIndex++;
			
			if (IsMouseOverRect(button))
			{
				InventoryGUI.spell = spell;
				InventoryGUI.IsSpell = true;
			}
		}
	}
	
	void DisplayRepair()
	{
		if (!currentNPC.Repairing)
			return;
		
		if (BasicControls("Repairing"))
			return;
		
		int itemIndex =1;
		
		foreach(EquipedItem equiped in player.Hero.Equip.Items)
		{
			Equiped item = (Equiped)equiped.rpgItem;
			if (equiped.CurrentDurability == item.Durability)
				continue;
			equiped.IsItemLoaded();
			
			Rect button = new Rect(LeftTopX + 30, LeftTopY + 15 + (itemIndex * 50), WindowWidth - 100, 45); 
			
			float percentage;
			if (equiped.CurrentDurability == 0)
			{
				percentage = 1;
			}
			else
			{
				percentage =  equiped.CurrentDurability / item.Durability;
			}
			int price = (int)(equiped.rpgItem.Value * currentNPC.RepairPriceModifier * percentage);
			//display price and name
			
			if (Button(button, ""))
			{
				//repair
				RepairItem(equiped, item, price);
			}
			
			DrawTexture(LeftTopX + 40, LeftTopY +20 + (itemIndex * 50), 40, 40, item.Icon);
			
			Rect name = new Rect(LeftTopX + 90, LeftTopY +20 + (itemIndex * 50), 200, 40);
			Label(name, item.Name + " " + price + " " + player.Hero.Settings.CurrencyKeyword);
			
			itemIndex++;
		}
		
		foreach(InventoryItem i in player.Hero.Inventory.Items)
		{
			if (i.rpgItem.Preffix == PreffixType.ITEM.ToString())
				continue;
			
			
			Equiped item = (Equiped)i.rpgItem;
			if (i.CurrentDurability == item.Durability)
				continue;
			
			Rect button = new Rect(LeftTopX + 30, LeftTopY + 15 + (itemIndex * 50), WindowWidth - 100, 50); 
			
			float percentage;
			if (i.CurrentDurability == 0)
			{
				percentage = 1;
			}
			else
			{
				percentage =  i.CurrentDurability / item.Durability;
			}
			int price = (int)(i.rpgItem.Value * currentNPC.RepairPriceModifier * percentage);
			//display price and name
			
			if (Button(button, ""))
			{
				//repair
				RepairItem(i, item, price);
			}
			
			DrawTexture(LeftTopX + 40, LeftTopY +20 + (itemIndex * 50), 40, 40, item.Icon);
			
			Rect name = new Rect(LeftTopX + 90, LeftTopY +20 + (itemIndex * 50), 200, 40);
			Label(name, item.Name + " " + price + " " + player.Hero.Settings.CurrencyKeyword);
			
			
			itemIndex++;
		}
	}
	
	private bool RepairItem(ItemInWorld equiped, Equiped item, int price)
	{
		RPGItem currencyItem = new RPGItem();
		currencyItem = Storage.LoadById<RPGItem>(currentNPC.RepairCurrencyID, new RPGItem());
		
		if (!player.Hero.Inventory.DoYouHaveThisItem(currencyItem.UniqueId, price))
			return false;
		
		equiped.CurrentDurability = item.Durability;
		PlayGUISound(GUISound.Repair);
		return true;
	}
	
	
	private void DisplayGuild()
	{
		//display name of guild
		if (BasicControls(guild.Name))
			return;
		
		//current player progress in this guild
		GuildProgress gp = player.Hero.Guild.GetGuildProgress(guild.ID);
		
		Rect rectLabel = new Rect(LeftTopX + 10, LeftTopY +40, 400, 25);
		//display current rank or display possibilty to join or cannot join
		string label = "You are NOT member of the " + guild.Name;
		if (gp != null)
		{
			label = "You are member of the " + guild.Name;
		}
		Label(rectLabel, label);
		
		if (gp != null && gp.GuildID == guild.ID)
		{
			rectLabel = new Rect(LeftTopX + 10, LeftTopY +70, 400, 25);
			Label(rectLabel, "Your current rank is " + guild.ActualRank(gp.GuilRankID).Name);
		}
		//display if can advance / join
		rectLabel = new Rect(LeftTopX + 10, LeftTopY +110, 400, 25);
		
		//is not member of the guild
		if (gp == null || gp.GuildID == 0)
		{
			//this NPC can recruit
			if (currentNPC.IsRecruit)
			{
				//if player meets condition
				if (guild.CanJoinGuild(player))
				{
					label = "You can join our ranks";
					Label(rectLabel, label);
					rectLabel = new Rect(LeftTopX + 10, LeftTopY +150, 400, 25);
					//join the guild button
					if (Button(rectLabel, "Join " + guild.Name))
					{
						player.Hero.Guild.JoinGuild(guild.ID);
					}
				}
				else
				{
					label = "You are not worthy to join our ranks";
					Label(rectLabel, label);
				}
			}
			else
			{
				label = "If you want to join us, find recruiter";
				Label(rectLabel, label);
			}
		}
		else 
		{
			if (gp.GuilRankID < currentNPC.AdvanceRankLevel)
			{
				if (guild.IsPreparedForNextRank(gp.GuilRankID, player))
				{
					Label(rectLabel, "I can promote you to next rank");
					rectLabel = new Rect(LeftTopX + 10, LeftTopY +150, 400, 25);
					//join the guild button
					if (Button(rectLabel, "Advance to next rank"))
					{
						player.Hero.Guild.UpdateRank(guild, player);
					}
				}
				else
				{
					Label(rectLabel, "You are not prepared for next rank");
				}
			}
			else
			{
				Label(rectLabel, "You are not prepared for next rank");
			}
		}
	}
}

public enum NPCMenuType
{
	Conversation,
	Shop,
	SpellShop,
	Repairing,
	Guild,
	LongLineTextConversation
}