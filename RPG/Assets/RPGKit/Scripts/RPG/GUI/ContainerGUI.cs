using UnityEngine;
using System.Collections;

public class ContainerGUI : BasicGUI 
{
	void Start()
	{
		Prepare();
	}
	
	public int PosX
	{
		get
		{
			return (ScreenWidth / 2) - (ScreenWidth/4);
		}
	}
	
	public int PosY
	{
		get
		{
			return ScreenHeight - ContainerHeight;
		}
	}
	
	void OnGUI()
	{
		if (BasicGUI.isMainMenuDisplayed)
			return;
		
		if (Input.GetKey(KeyCode.Escape))
		{
            CloseAllWindows();
			return;
		}
		
		timeDelay += Time.deltaTime;
		
		if (BasicGUI.isContainerDisplayed)
		{
			DisplayContainer();
		}
	}
	
	void PrepareSharedContainer()
	{
        if (!player.rpgContainer.SharedStash)
			return;
		foreach(RPGContainer container in player.Hero.SharedContainers)
		{
            if (container.ID == player.rpgContainer.ID)
                player.rpgContainer = container;
		}
	}

    void CloseContainer()
    {
        BasicGUI.isContainerDisplayed = false;
        player.rpgContainer = null;
        player.ChangeMouseControl(true);
    }
	
	void DisplayContainer()
	{
		PrepareSharedContainer();
		int widthRightPos = ScreenWidth/2 + 80;
		Box(PosX, PosY, widthRightPos, ContainerHeight,"");
		//close button
		Rect r = new Rect(PosX + widthRightPos - 75,PosY + 15, 60, 20);
		if (Button(r, "Close"))
		{
            CloseContainer();
			return;
		}
		
		//take all button
		r = new Rect(PosX + widthRightPos - 75, PosY + 60, 60, 20);
        if (player.rpgContainer.Items != null && player.rpgContainer.Items.Count > 0)
		{
			if (Button(r, "All"))
			{
                player.rpgContainer.TakeAll(player);
                if (player.rpgContainer.Items.Count == 0)
				{
                    CloseContainer();
					return;
				}
			}
		}
		
		for(int x = 0; x <= 9; x++)
		{
			for (int y = 0; y <= 1;y++)
			{
				int xpos = PosX + 10 + (x * (GUIItemButon + GUIItemIndent));
				int ypos = PosY + 35 + (y * (GUIItemButon + GUIItemIndent));
                ItemInWorld item = player.rpgContainer.GetByPosition(x + 1, y + 1);
				r = new Rect(xpos,ypos, GUIItemButon, GUIItemButon);
				if (item.IsItemLoaded())
				{
					if (Button(r,"", "TOOLTIP"))
					{
						if (Event.current.button == 1)
						{
							//pickup item
							if (item.CurrentAmount > 1)
							{
								BasicGUI.isQuantityActivated = true;
                                Player.currentItem = item.rpgItem;
								QuantityGUI.MaximumValue = item.CurrentAmount;
								QuantityGUI.QuantityType = QuantityTypeEnum.ContainerLoot;
							}
							else
							{
                                player.rpgContainer.TakeItem(item.rpgItem, player);
							}
                            if (player.rpgContainer.Items.Count == 0)
							{
                                CloseContainer();
								return;
							}
						}
					}
					DrawTexture(xpos+2, ypos+2, GUIItemButon-4, GUIItemButon-4, item.rpgItem.Icon);
					//show amount only if amount is greater than one
					if (item.CurrentAmount > 1)
					{
						r = new Rect(xpos+3, ypos + 15, 30,20);
						Label(r, item.CurrentAmount.ToString(), Skin.customStyles[0]);
					}
					
					if (IsMouseOverRect(r))
					{
						InventoryGUI.IsToolTip = true;
						InventoryGUI.tooltip = item;
					}
				}
				else
				{
					Button(new Rect(xpos,ypos, GUIItemButon, GUIItemButon), "");
				}
			}
		}
	}
}
