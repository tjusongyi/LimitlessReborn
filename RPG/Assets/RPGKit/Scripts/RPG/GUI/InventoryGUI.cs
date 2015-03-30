using UnityEngine;
using System.Collections;

public class InventoryGUI : BasicGUI 
{
	InventoryItem activeItem;
	bool isItemActive;
	
	public static bool IsToolTip;
	public static ItemInWorld tooltip;
	
	public static bool IsSpell;
	public static RPGSpell spell;
	
	private bool isInventoryTooltip;
	
	void Start()
	{
		Prepare();
	}
	
	void OnGUI()
	{
		if (BasicGUI.isMainMenuDisplayed)
			return;
		
		if (Input.GetKey(KeyCode.Escape))
		{
			BasicGUI.isInventoryDisplayed = false;
			isItemActive = false;
			return;
		}
		if (BasicGUI.isInventoryDisplayed)
		{
			DisplayInventory();
		}
		
		if (isItemActive && Player.currentItem != null)
		{
			Rect r = new Rect(Input.mousePosition.x -GUIItemButon/2 ,Screen.height - Input.mousePosition.y-GUIItemButon/2, GUIItemButon, GUIItemButon);
			GUI.DrawTexture(r,activeItem.rpgItem.Icon);
		}
		
		if (Player.currentItem == null)
			isItemActive = false;
		
		
		if (!isItemActive && IsToolTip)
		{
			bool shop = false;
			float priceModifier = 1;
			if (NPCGUI.MenuMode == NPCMenuType.Shop)
			{
				shop = true;
				
				if (isInventoryTooltip)
				{
					priceModifier = NPCGUI.currentShop.SellPriceModifier;
				}
				else
				{
					priceModifier = NPCGUI.currentShop.BuyPriceModifier;
				}
			}
			
			
			tooltip.rpgItem.Tooltip(tooltip, Skin, shop, isInventoryTooltip, priceModifier, player);
		}
		
		if (IsSpell)
		{
			spell.Tooltip(Skin, player);
		}
	}
	
	void LateUpdate()
	{
		IsToolTip = false;
		tooltip = null;
		
		spell = null;
		IsSpell = false;
		isInventoryTooltip = false;
	}

    void Update()
    {
        timeDelay += 0.01f;

        if (IsOpenKey(player.Hero.Controls.Inventory))
        {
            ChangeIventoryStatus();
        }
    }
	
	void DisplayInventory()
	{
		RightBox("Inventory");
		if (isItemActive)
		{
			//only if item can be dropped on the ground
			if (activeItem.rpgItem.Droppable && !string.IsNullOrEmpty(activeItem.rpgItem.PrefabName))
			{
				Rect groundButton = new Rect(RightTopX  + WindowWidth - 130, LeftTopY + WindowHeight - 50, 60, 40);
				if (Button(groundButton, "Ground"))
				{
					//set position for droping
					Vector3 newPosition = transform.position + transform.forward * 1;
					isItemActive = false;
					//remove item from inventory
					player.Hero.Inventory.DropItem(activeItem, player);
					//create game object
					GameObject go = (GameObject)Instantiate(Resources.Load(activeItem.rpgItem.PrefabName), newPosition, transform.rotation);
					//attach correct script
					activeItem.AttachScript(go);
				}
			}
		}
		int count = 0;
		
		for(int x = 0; x < player.Hero.Inventory.sizeXOfInventory; x++)
		{
			for (int y = 0; y < player.Hero.Inventory.sizeYOfInventory;y++)
			{
				count++;
				int xpos = RightTopX + 15 + (x * (GUIItemButon + GUIItemIndent));
				int ypos = LeftTopY +  45 + (y * (GUIItemButon + GUIItemIndent));
				InventoryItem item = player.Hero.Inventory.GetByPosition(x, y);
				Rect r = new Rect(xpos,ypos, GUIItemButon, GUIItemButon);
				if (item.IsItemLoaded(Player.sellPriceModifier))
				{
					//button events
					if (Button(r, "", "TOOLTIPICON"))
					{
						
						//button is active
						if (Event.current.button == 0)
						{
							//move item to new position
							if (isItemActive)
							{
								MoveItem(x, y);
							}
							else
							{
								SetItemActive(item);
							}
						}
						else if (Event.current.button == 1)
						{
							RightMouseButtonEvent(item);
						}
					}
					DrawTexture(xpos+2, ypos+2, GUIItemButon-4, GUIItemButon-4, item.rpgItem.Icon);
					//show amount only if amount is greater than one
					if (item.CurrentAmount > 1)
					{
						Label(xpos+ 5, ypos  + 20, 40,20, item.CurrentAmount.ToString(), Skin.customStyles[0]);
					}
					
					if (IsMouseOverRect(r))
					{
						IsToolTip = true;
						tooltip = item;
						isInventoryTooltip = true;
					}
				}
				else
				{
					if (Button(r, ""))
					{
						if (isItemActive)
						{
							MoveItem(x, y);
						}
					}
				}
				
			}
		}
	}
	
	void MoveItem(int x, int y)
	{
		player.Hero.Inventory.MoveItem(activeItem, x,y);
		isItemActive = false;
		Player.currentItem = null;
	}
	
	void RightMouseButtonEvent(InventoryItem item)
	{
		if (item.CurrentAmount > 1)
		{
			BasicGUI.isQuantityActivated = true;
			QuantityGUI.MaximumValue = item.CurrentAmount;
			Player.currentItem = item.rpgItem;
		}
		
		//if shop displayed sell item
		if (BasicGUI.isShopActivated && item.rpgItem.Value > 0)
		{
			if (item.CurrentAmount > 1)
				QuantityGUI.QuantityType = QuantityTypeEnum.Sell;
			else
			{
				NPCGUI.currentShop.SellItem(item.rpgItem, player);	
				PlayGUISound(GUISound.Gold);
			}
		}
		//drop item into container
		else if (BasicGUI.isContainerDisplayed && !player.rpgContainer.OnlyLoot)
		{
			
			if (item.CurrentAmount > 1)
				QuantityGUI.QuantityType = QuantityTypeEnum.Drop;
			else
			{
                if (player.rpgContainer.DropItemFromInventory(item.rpgItem, player))
					player.Hero.Inventory.RemoveItem(item.rpgItem, player);
			}
		}
		else
		{
			if (item.rpgItem.Preffix == PreffixType.ARMOR.ToString())
			{
				PlayGUISound(GUISound.EquipArmor);
			}
			else
			{
				PlayGUISound(GUISound.EquipWeapon);
			}
			//equip item
			player.Hero.Inventory.EquipItem(item, player);
			player.Hero.CalculateDamage(player);
		}
	}
	
	void SetItemActive(InventoryItem item)
	{
		isItemActive = true;
		Player.currentItem = item.rpgItem;
		activeItem = item;
	}
}
