using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlInclude(typeof(ShopCategory))]
[XmlInclude(typeof(ShopItem))]
public class Shop : BasicItem
{
	public int CurrencyID;
	public float BuyPriceModifier;
	public float SellPriceModifier;
	public bool SellSameAsBuy;
	public ShopRespawnTimer RespawnTimer;
	public List<ShopCategory> SellCategories;
	public List<ShopCategory> Categories;
	public List<ShopItem> Items;
	
	[XmlIgnore]
	public List<ItemInWorld> ShopItems = new List<ItemInWorld>();
	
	public Shop()
	{
		SellCategories = new List<ShopCategory>();
		Categories = new List<ShopCategory>();
		SellSameAsBuy = true;
		RespawnTimer = ShopRespawnTimer.Monday;
		BuyPriceModifier = 1;
		SellPriceModifier = 1;
		Items = new List<ShopItem>();
		CurrencyID = GlobalSettings.GoldID;
        Name = string.Empty;
        Description = string.Empty;
        preffix = "SHOP";
	}
	
	public BuyTransaction BuyItem(RPGItem item, int Amount, Player player)
	{
		if (item.ID == 0)
			return BuyTransaction.SomeError;
		//calculate price
		int price = (int)(item.Value * BuyPriceModifier * Amount);
		//currency item
		RPGItem currencyItem = new RPGItem();
		currencyItem = Storage.LoadById<RPGItem>(CurrencyID, new RPGItem());
		//skill affecting price put modifier here
		//check if you have enough gold
		if (!player.Hero.Inventory.DoYouHaveThisItem(currencyItem.UniqueId, price))
			return BuyTransaction.NotEnoughGold;
		
		//check space in inventory
		if (!player.Hero.Inventory.DoYouHaveSpaceForThisItem(item, Amount))
		{
			return BuyTransaction.NotEnoughSpaceInInventory;
		}
		else
		{
			//add item to inventory
			player.Hero.Inventory.AddItem(item, Amount, player);
			
			//remove currency amount from inventory
			player.Hero.Inventory.RemoveItem(currencyItem, price, player);
			
			//remove item from current shop collection
			foreach(ItemInWorld shopItem in ShopItems)
			{
				if (shopItem.rpgItem.UniqueId == item.UniqueId)
				{
					shopItem.CurrentAmount = shopItem.CurrentAmount - Amount;
					if (shopItem.CurrentAmount == 0)
					{
						ShopItems.Remove(shopItem);
					}
					break;
				}
			}
			
			//add bio log info
			player.Hero.Log.BuyItem(Player.activeNPC, Amount, item.UniqueId);
			return BuyTransaction.OK;	
		}
	}
	
	public BuyTransaction BuyItem(RPGItem rpgItem, Player player)
	{
		return BuyItem(rpgItem, 1, player);
	}
	
	public bool SellItem(RPGItem item, int Amount, Player player)
	{
		if (item.ID == 0)
			return false;
		//calculate price
		int price = (int)(item.Value * SellPriceModifier * Amount);
		//currency item
		RPGItem currencyItem = new RPGItem();
		currencyItem = Storage.LoadById<RPGItem>(CurrencyID, new RPGItem());
		//space for gold
		if (!player.Hero.Inventory.DoYouHaveThisItem(currencyItem.UniqueId, price))
			return false;
		//remove item
		player.Hero.Inventory.RemoveItem(item, Amount, player);
		//add gold
		player.Hero.Inventory.AddItem(currencyItem, price, player);
		//add item to temp shop collection
		AddItem(item, Amount);
		return true;
	}
	
	private void AddItem(RPGItem item, int Amount)
	{
		foreach(ItemInWorld i in ShopItems)
		{
			if (i.rpgItem.UniqueId == item.UniqueId)
				return;
		}
		
		ItemInWorld itemInWorld = new ItemInWorld();
		itemInWorld.UniqueItemId = item.UniqueId;
		itemInWorld.rpgItem = item;
		itemInWorld.CurrentAmount = Amount;
		ShopItems.Add(itemInWorld);
	}
	
	public bool SellItem(RPGItem item, Player player)
	{
		return SellItem(item, 1, player);
	}
	
	private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = dt.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
                diff += 7;
        }

        return dt.AddDays(-1 * diff).Date;
    }
	
	public List<ItemInWorld> GetItems(int NPCId, Player player)
	{
		//load categories
		foreach(ShopCategory category in Categories)
            category.GetItems(ShopItems, player);
		//load items
		foreach(ShopItem shopItem in Items)
			shopItem.AddOneItem(ShopItems);

        BuyedItems(ShopItems, NPCId, player);
		
		return ShopItems;
	}
	
	private void BuyedItems(List<ItemInWorld> items, int NPCId, Player player)
	{
		//check if some item was sold in this period
		DateTime date = Weather.CurrentDateTime;
		
		DateTime startPeriodDate = date;
		//determine datum
		switch(RespawnTimer)
		{
			case ShopRespawnTimer.Never : return;
			case ShopRespawnTimer.Monday: 
				int diff = date.DayOfWeek - DayOfWeek.Monday;
				if (diff < 0)
					diff += 7;
				startPeriodDate = date.AddDays(-1 * diff).Date;
				break;
			case ShopRespawnTimer.NewMonth:
				startPeriodDate = date.AddDays(-1 * date.Day).Date; 
				break;
		}
		//search through log and remove buyed items in current period for current NPC
		foreach(LogEvent logEvent in player.Hero.Log.Events)
		{
			//log must be same as NPC id and buying item
			if (logEvent.EventType != LogEventType.BuyItem || NPCId != logEvent.NPCId)
				continue;
			if (startPeriodDate > logEvent.Date)
				continue;
			foreach(ItemInWorld item in items)
			{
				if (item.rpgItem.UniqueId == logEvent.UniqueId)
				{
					item.CurrentAmount -= logEvent.Amount;
					//if amount is ZERO remove from collection
					if (item.CurrentAmount == 0)
					{
						items.Remove(item);
						break;
					}
				}
			}
		}
	}
}

public enum ShopRespawnTimer
{
	Never = 0,
	Monday = 1,
	NewMonth = 2
}
	
public enum BuyTransaction
{
	SomeError = 0,
	OK = 1,
	NotEnoughSpaceInInventory = 2,
	NotEnoughGold = 3
}
	