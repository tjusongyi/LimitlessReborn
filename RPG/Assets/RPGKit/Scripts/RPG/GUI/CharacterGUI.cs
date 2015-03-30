using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterGUI : BasicGUI 
{
	CharacterMenuEnum menuType = CharacterMenuEnum.Stats;
	int topIndent = 90;
	public Texture2D HpBar; 
	public Texture2D ManaBar;
	
	void Start()
	{
		Prepare();
		HpBar = (Texture2D)Resources.Load("GUI/Images/HP");
		ManaBar = (Texture2D)Resources.Load("GUI/Images/Mana");
	}

    void Update()
    {
        timeDelay += 0.01f;

        if (IsOpenKey(player.Hero.Controls.Character))
        {
            ChangeCharacterStatus();
        }
    }
	
	void OnGUI()
	{
		if (BasicGUI.isMainMenuDisplayed)
			return;
		
		if (BasicGUI.isCharacterDisplayed)
		{
			BasicGUI.isConversationDisplayed = false;
			DisplayCharacterWindow();
		}
		else if (!BasicGUI.isConversationDisplayed)
		{
			DisplayPlayerBasicStats();
		}
	}
	
	void DisplayPlayerBasicStats()
	{
		
		int maximum = 250;
		float hpBarSize = maximum * (player.Hero.CurrentHp / player.Hero.TotalHp);
		if (hpBarSize > maximum)
			hpBarSize = maximum;
		
		DrawTexture(5, 5, hpBarSize, 20, HpBar);
		
		float manaBarSize = maximum * (player.Hero.CurrentMana / player.Hero.TotalMana);
		if (manaBarSize > maximum)
			manaBarSize = maximum;
		DrawTexture(5, 30, manaBarSize, 20, ManaBar);
	
	}
	
	void DisplayCharacterWindow()
	{
		LeftBox("Character details");
		
		if (Button (new Rect (LeftTopX + 30,LeftTopY + 50,80,25), "Character"))
		{
			menuType = CharacterMenuEnum.Stats;
		}
		
		if (Button(new Rect (LeftTopX + 150,LeftTopY + 50,80,25), "Skills"))
		{
			menuType = CharacterMenuEnum.Skills;
		}
		
		if (Button(new Rect (LeftTopX + 260,LeftTopY + 50,80,25), "Reputation"))
		{
			menuType = CharacterMenuEnum.Reputation;
		}
		
		if (menuType == CharacterMenuEnum.Stats)
		{
			DisplayAttributes();
		}
		else if (menuType == CharacterMenuEnum.Skills)
		{
			DisplaySkills();
		}
		else
		{
			DisplayReputation();
		}
	}
	
	void DisplayReputation()
	{
		Rect rect;
		int buttonCount = 0;
		int rowSize = 25;
		
		foreach(HeroReputation hr in player.Hero.Reputations)
		{
			
			if (!string.IsNullOrEmpty(hr.TextBefore))
			{
				rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,150,20);
				Label(rect, hr.TextBefore);
				buttonCount++;
			}
			
			rect = new Rect(LeftTopX +40, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
			Label(rect, hr.Name);
			
			rect = new Rect(LeftTopX +150, LeftTopY + topIndent + (rowSize * buttonCount) ,70,20);
			Label(rect, hr.Rank);
			
			buttonCount++;
		}
	}
	
	void DisplayAttributes()
	{
		int rowSize = 25;
		
		int buttonCount = 0;
		Rect rect;
		
		//displaying level
		if (player.Hero.Settings.Leveling != LevelingSystem.NoLevels)
		{
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
			Label(rect, "Level: ");
			rect = new Rect(LeftTopX +120, LeftTopY + topIndent + (rowSize * buttonCount) ,80,20);
			Label(rect, player.Hero.CurrentLevel.ToString());
			buttonCount++;
			
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
			Label(rect, "Progress: ");
			rect = new Rect(LeftTopX +120, LeftTopY + topIndent + (rowSize * buttonCount) ,80,20);
			Label(rect, player.Hero.CurrentXP + " / " + player.Hero.CurrentLevelXP);
			buttonCount++;
		}
		
		player.Hero.Bonuses.Calculate(player);
		//attributes
		for(int index = 0; index < player.Hero.Attributes.Count; index++)
		{
			RPGAttribute atr = player.Hero.Attributes[index];
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,80,20);
			Label(rect, atr.Name);
			
			rect = new Rect(LeftTopX +105, LeftTopY + topIndent + (rowSize * buttonCount) ,30,20);
			Label(rect, atr.Value.ToString());
			
			rect = new Rect(LeftTopX +135, LeftTopY + topIndent + (rowSize * buttonCount) ,30,20);
			Label(rect, player.Hero.Bonuses.TotalAttributes[index].Value.ToString());

            if (player.Hero.AttributePoint > 0 && atr.Value < atr.Maximum)
			{
				rect = new Rect(LeftTopX +170, LeftTopY + topIndent + (rowSize * buttonCount) ,27,20);
				if (GUI.Button(rect , "+"))
				{
					atr.Add(1, player);
					PlayGUISound(GUISound.SkillUp);
					player.Hero.AttributePoint--;
				}
			}
			buttonCount++;
		}
		//add attribute point
        if (player.Hero.AttributePoint > 0)
		{
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,170,20);
			Label(rect, "You have " + player.Hero.AttributePoint + " attribute points");
			buttonCount++;
		}
		//player damage
		rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
		Label(rect, "Damage: ");
		rect = new Rect(LeftTopX +120, LeftTopY + topIndent + (rowSize * buttonCount) ,80,20);
		Label(rect, PlayerAttackStats.MinimumDamage + " - " + PlayerAttackStats.MaximumDamage);
		buttonCount++;
		//hit chance
		if (player.Hero.Settings.PlayerAlwaysHit == false)
		{
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
			Label(rect, "Chance to hit: ");
			rect = new Rect(LeftTopX +120, LeftTopY + topIndent + (rowSize * buttonCount) ,80,20);
			Label(rect, PlayerAttackStats.ChanceToHit + "%");
			buttonCount++;
		}
		//display equip
		DisplayEquip();
	}
	
	void DisplayEquip()
	{
		Rect rect;
		int leftTopX = LeftTopX +  WindowWidth /2 + 10;
		Box(leftTopX, LeftTopY + topIndent, (WindowWidth/2) - 20, WindowHeight - topIndent - 10, "Equip");
		
		foreach(RPGEquipmentSlot slot in Player.Data.Slots)
		{
			rect = new Rect(leftTopX + slot.PosX, slot.PosY + LeftTopY + topIndent+ 10, GUIItemButon, GUIItemButon);
			bool founded = false;
			EquipedItem e = new EquipedItem();
			if (player.Hero.Equip.Items != null)
			{
				foreach(EquipedItem equiped in player.Hero.Equip.Items)
				{
					
					foreach(RPGEquipmentSlot id in equiped.Slots)
					{
						if (id.ID == slot.ID)
						{
							e = equiped;
							founded = true;
							
						}
					}
				}
			}
			if (!founded)
				Button(rect, "");
			else
			{
				if (Button(rect, e.rpgItem.Icon, "TOOLTIPICON"))
				{
					if (Event.current.button == 1)
					{
						player.Hero.Equip.UnEquipItem(slot, player);
						player.Hero.CalculateDamage(player);
					}
				}
				
				if (IsMouseOverRect(rect))
				{
					InventoryGUI.IsToolTip = true;
					InventoryGUI.tooltip = e;
				}
			}
		}
		
	}
	
	void DisplaySkills()
	{
		int rowSize = 35;
		int buttonCount = 0;
		Rect rect;
		bool displayAddSkillPoint = false;
		if (player.Hero.SkillPoint > 0)
			displayAddSkillPoint = true;
		
		foreach(RPGSkill skill in player.Hero.Skills)
		{
			rect = new Rect(LeftTopX +20, LeftTopY + topIndent + (rowSize * buttonCount) ,100,20);
			Label(rect, skill.Name);
			
			rect = new Rect(LeftTopX +125, LeftTopY + topIndent + (rowSize * buttonCount) ,30,20);
			Label(rect, ((int)skill.Value).ToString());
			
			if (displayAddSkillPoint && skill.Value < skill.Maximum)
			{
				rect = new Rect(LeftTopX +160, LeftTopY + topIndent + (rowSize * buttonCount) ,27,20);
				if (Button(rect , "+"))
				{
					skill.Add(1, player);
					PlayGUISound(GUISound.SkillUp);
					player.Hero.SkillPoint--;
				}
			}
			buttonCount++;
		}
	}
}

public enum CharacterMenuEnum
{
	Stats,
	Skills,
	Reputation
}
