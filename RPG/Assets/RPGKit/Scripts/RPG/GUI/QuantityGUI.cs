using UnityEngine;
using System;
using System.Collections;

public class QuantityGUI  : BasicGUI
{
	public static QuantityTypeEnum QuantityType;
	public static int MaximumValue;
	public static int CurrentValue = 1;
	
	void Start()
	{
		Prepare();
	}
	
	void OnGUI()
	{
		if (!BasicGUI.isQuantityActivated)
			return;
		
		string boxLabel = string.Empty;
		
		switch(QuantityType)
		{
			case QuantityTypeEnum.Buy : 
				if (NPCGUI.currentShop == null)
					return;
				boxLabel = "Buy";
				break;
			case QuantityTypeEnum.Drop : 
				boxLabel = "Drop";
				break;
			case QuantityTypeEnum.Sell: 
				if (!BasicGUI.isInventoryDisplayed)
					return;
				boxLabel = "Sell";
				break;
			case QuantityTypeEnum.BodyLoot : 
				boxLabel = "Loot";
				break;
			case QuantityTypeEnum.ContainerLoot:
				boxLabel = "Loot";
				break;
		}
		
		Box(QuantityLeftTopX, QuantityLeftTopY, QuantityWidth, QuantityHeight, boxLabel);
		
		DisplayBox();
	}
	
	void DisplayBox()
	{
		int index = 1;
		
		Rect controls = new Rect(QuantityLeftTopX + 30, QuantityLeftTopY +20 + (index * 40), 30, 30);
		if (Button(controls, " < "))
		{
			//add value
			if (CurrentValue > 1)
				CurrentValue--;
		}
		index++;
		controls = new Rect(QuantityLeftTopX + 30, QuantityLeftTopY +20 + (index * 40), 30, 30);
		CurrentValue = Convert.ToInt32(TextField(controls, CurrentValue.ToString()));
		
		index++;
		controls = new Rect(QuantityLeftTopX + 30, QuantityLeftTopY +20 + (index * 40), 30, 30);
		if (Button(controls, " > "))
		{
			//reduce value
			if (CurrentValue < MaximumValue)
				CurrentValue++;
		}
		
		index++;
		controls = new Rect(QuantityLeftTopX + 30, QuantityLeftTopY +20 + (index * 40), 30, 30);
		//maximum value
		if (Button(controls, "All"))
		{
			CurrentValue = MaximumValue;
		}
		
		index++;
		controls = new Rect(QuantityLeftTopX + 30, QuantityLeftTopY +20 + (index * 40), 30, 30);
		if (Button(controls, "Ok"))
		{
			RPGItem item = new RPGItem();
			try
			{
				item = (RPGItem)Player.currentItem;
			}
			catch
			{}
			//do action depend caller
			switch(QuantityType)
			{
				case QuantityTypeEnum.Buy:
					
					BuyTransaction result = NPCGUI.currentShop.BuyItem(item, CurrentValue, player);
					if (result == BuyTransaction.OK)
					{
						BasicGUI.isQuantityActivated = false;
					}
					CurrentValue = 1;
					break;
				case QuantityTypeEnum.Sell:
					if(NPCGUI.currentShop.SellItem(item, CurrentValue, player))
					{
						BasicGUI.isQuantityActivated = false;
					}
					CurrentValue = 1;
					break;
				case QuantityTypeEnum.Drop:
                    if (player.rpgContainer.IsContainerFull)
						return;
					//Player.rpgContainer.DropItem(
					break;
			}
		}
	}
}

public enum QuantityTypeEnum
{
	Buy = 0,
	Sell = 1,
	BodyLoot = 2,
	Drop = 3,
	Nothing = 4,
	ContainerLoot = 5
}
