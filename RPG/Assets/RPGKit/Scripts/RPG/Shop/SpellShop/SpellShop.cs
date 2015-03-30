using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

public class RPGSpellShop : BasicItem
{
	public int CurrencyID;
	public float BuyPriceModifier;
	public bool SellSameAsBuy;
	public List<SpellShopCategory> Categories;
	public List<SpellShopItem> Items;
	
	[XmlIgnore]
	public List<RPGSpell> Spells = new List<RPGSpell>();
	
	public RPGSpellShop()
	{
		Categories = new List<SpellShopCategory>();
		Items = new List<SpellShopItem>();
		BuyPriceModifier = 2;
        preffix = "SPELLSHOP";
        Name = string.Empty;
        Description = string.Empty;
	}

    public void LoadSpells(Player player)
	{
		List<RPGSpell> allSpells = Storage.Load<RPGSpell>(new RPGSpell());
		
		//adding spell shop category
		foreach(SpellShopCategory spc in Categories)
            spc.AddSpells(allSpells, Spells, player);
		
		//ading spell
		foreach(SpellShopItem ssi in Items)
			ssi.AddSpell(allSpells, Spells, player);

        RemoveDuplicateSpells(player);
	}
	
	//remove spell what player own
	public void RemoveDuplicateSpells(Player player)
	{
        foreach (RPGSpell s in player.Hero.Spellbook.Spells)
		{
			foreach(RPGSpell spell in Spells)
			{
				if (s.ID == spell.ID)
				{
					Spells.Remove(spell);
					break;
				}
			}
		}
	}
	
	public bool BuySpell(RPGSpell spell, Player player)
	{
		//check amount of money
		int price = (int)(spell.Price * BuyPriceModifier);
		
		//deduct the money
		RPGItem currencyItem = new RPGItem();
		currencyItem = Storage.LoadById<RPGItem>(CurrencyID, new RPGItem());

        if (!player.Hero.Inventory.DoYouHaveThisItem(currencyItem.UniqueId, price))
			return false;
		
		//remove currency item
        player.Hero.Inventory.RemoveItem(currencyItem, price, player);
		//add spell to spellbook
        player.Hero.Spellbook.AddSpell(spell);
		
		return true;
	}
}
