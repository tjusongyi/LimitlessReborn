using UnityEngine;
using System.Collections;

public class SpellbookGUI : BasicGUI 
{
	bool isItemActive;
	RPGSpell activeSpell;
	
	void Start()
	{
		Prepare();
	}

    void Update()
    {
        timeDelay += 0.01f;

        if (IsOpenKey(player.Hero.Controls.Spellbook))
        {
            ChangeSpellBookStatus();
        }
    }
	
	void OnGUI()
	{
		if (BasicGUI.isMainMenuDisplayed)
			return;
		
		
		if (BasicGUI.isSpellbookDisplayed)
			DisplaySpellbook();
		
		if (isItemActive && Player.currentItem != null)
		{
			Vector3 mousePos = Input.mousePosition;
			Rect rect = new Rect(mousePos.x,Screen.height - mousePos.y, activeSpell.Icon.width, activeSpell.Icon.height);
			GUI.Label(rect,activeSpell.Icon);
		}
		if (Player.currentItem == null)
			isItemActive = false;
	}
	
	void DisplaySpellbook()
	{
		RightBox("Spellbook");
		int spellIndex = 1;
        foreach (RPGSpell spell in player.Hero.Spellbook.Spells)
		{
			spell.LoadIcon();
			//display box for spell	
			Rect button = new Rect(RightTopX + 30, LeftTopY + 15 + (spellIndex * 50), WindowWidth - 100, 50); 
			if (Button(button, "", "TOOLTIP"))
			{
				SetItemActive(spell);
			}
			if (IsMouseOverRect(button))
			{
				InventoryGUI.spell = spell;
				InventoryGUI.IsSpell = true;
			}
			
			DrawTexture(RightTopX + 40, LeftTopY + 20 + (spellIndex * 50), 40, 40, spell.Icon);
			
			Label(RightTopX + 90, LeftTopY +20 + (spellIndex * 50), 200, 40, spell.Name);
			spellIndex++;
		}
	}
	
	void SetItemActive(RPGSpell item)
	{
		isItemActive = true;
		Player.currentItem = item;
		activeSpell = item;
	}
}
