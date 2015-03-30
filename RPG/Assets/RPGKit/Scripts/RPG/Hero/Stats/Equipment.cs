using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlInclude(typeof(Equiped))]
public class Equipment
{
	//public int MinimumRange;
	public int MaximumRange;
	private bool NeedAmmo;
	public List<EquipedItem> Items;
	
	[XmlIgnore]
	public RPGWeapon Weapon;

	public Equipment()
	{
		Items = new List<EquipedItem>();
		Weapon = new RPGWeapon();
	}
	
	public int ArmorClassValue
	{
		get
		{
			int armor = 0;
			foreach(EquipedItem equiped in Items)
			{
				if (equiped.rpgItem.Preffix == PreffixType.ARMOR.ToString())
				{
					RPGArmor a = (RPGArmor)equiped.rpgItem;
					armor += a.ArmorClassValue;
				}
			}
			return armor;
		}
	}
	
	public List<Effect> GetEffects()
	{
		List<Effect> effects = new List<Effect>();
		
		foreach(EquipedItem equiped in Items)
		{
			if (equiped.rpgItem is Equiped)
			{
				Equiped equip = (Equiped)equiped.rpgItem;
				foreach(Effect e in equip.WornEffects)
				{
					effects.Add(e);
				}
			}	
		}
		return effects;
	}

    public void OnHitEffects(Enemy enemy, Player player)
	{
		foreach(EquipedItem equiped in Items)
		{
			if (equiped.rpgItem is RPGArmor)
			{
				RPGArmor armor = (RPGArmor)equiped.rpgItem;
				
				foreach(Effect effect in armor.EffectsOnHit)
				{
					if (effect.Target == TargetType.Self)
                        player.Hero.Buffs.AddEffect(effect);
				}
			}
		}
	}
	
	public void HitEffects(Enemy enemy, Player player)
	{
		foreach(EquipedItem equiped in Items)
		{
			if (equiped.rpgItem is RPGWeapon)
			{
				RPGWeapon armor = (RPGWeapon)equiped.rpgItem;
				
				foreach(Effect effect in armor.EffectsHit)
				{
					if (effect.Target == TargetType.Self)
                        player.Hero.Buffs.AddEffect(effect);
				}
			}
		}
	}
	
	private bool IsEquipmentSlotUsed(List<RPGEquipmentSlot> slots)
	{
		foreach(RPGEquipmentSlot newEquipmentID in slots)
		{
			foreach(EquipedItem equiped in Items)
			{
				foreach(RPGEquipmentSlot s in equiped.Slots)
				{
					if(newEquipmentID.ID == s.ID)
					{
						return true;
					}
				}
			}
		}
		return false;
	}
	
	private List<EquipedItem> GetUnequipingItems(RPGEquipmentSlot slot)
	{
		List<RPGEquipmentSlot> slotsID = new List<RPGEquipmentSlot>();
		slotsID.Add(slot);
		return GetUnequipingItems(slotsID);
	}
	
	private List<EquipedItem> GetUnequipingItems(List<RPGEquipmentSlot> slots)
	{
		List<EquipedItem> items = new List<EquipedItem>();
		
		foreach(RPGEquipmentSlot newEquipmentID in slots)
		{
			foreach(EquipedItem equiped in Items)
			{
				foreach(RPGEquipmentSlot es in equiped.Slots)
				{
					if (es.UniqueId == newEquipmentID.UniqueId)
						items.Add(equiped);
				}
			}
		}
		return items;
	}
	
	// Unequip one item
	public bool UnEquipItem(RPGEquipmentSlot equipmentSlot, Player player)
	{
		List<EquipedItem> dropingItems = GetUnequipingItems(equipmentSlot);
		PlayerEquip.itemsToUnequip = dropingItems;
		//only if items doest not remain in inventory
        if (!player.Hero.Settings.EquipedItemInInventory)
		{
			foreach(EquipedItem e in dropingItems)
			{
                player.Hero.Inventory.AddItem(e.rpgItem, player);
			}
		}
		RemoveItem(dropingItems);
		
		return true;
	}
	
	// Remove item from all collections
	private void RemoveItem(List<EquipedItem> dropingItems)
	{
		for (int x = 1; x <= dropingItems.Count;x++)
		{
			foreach(EquipedItem eq in dropingItems)
			{
				//equiped list (used for character GUI)
				foreach(EquipedItem e in Items)
				{
					if (e == eq)
					{
						Items.Remove(e);
						break;
					}
				}
			}
		}
	}
	
	private void SetWeapon(RPGWeapon weapon)
	{
		if (weapon.IsAmmo)
			return;
		Weapon = weapon;
		NeedAmmo = weapon.NeedAmmo;
	}
	
	public bool EquipItem(InventoryItem item, Player player)
	{
		EquipedItem equiped = new EquipedItem() ;
		equiped.CurrentAmount = item.CurrentAmount;
		equiped.CurrentDurability = item.CurrentDurability;
		equiped.UniqueItemId = item.UniqueItemId;
		equiped.rpgItem = item.rpgItem;
		equiped.rpgItem.LoadIcon();
		//get equipment slots
		Equiped e = (Equiped)item.rpgItem;
		equiped.Slots = e.EquipmentSlots;
		
		if (!e.CanYouEquip(player))
		{
			//TODO display error message
			return false;
		}
		//if equipment slot is used
		if (IsEquipmentSlotUsed(equiped.Slots))
		{
			List<EquipedItem> dropingItems = GetUnequipingItems(equiped.Slots);
			//only if items does not remain in inventory
            if (!player.Hero.Settings.EquipedItemInInventory)
			{
				foreach(EquipedItem equip in dropingItems)
				{
                    player.Hero.Inventory.AddItem(equip.rpgItem, player);	
				}
			}
			//remove dropping items from equip
			RemoveItem(dropingItems);
			PlayerEquip.itemsToUnequip = dropingItems;
		}
		if (item.rpgItem.Preffix == "WEAPON")
		{
			RPGWeapon w = (RPGWeapon)item.rpgItem;
			SetWeapon(w);
		}
		if (PlayerEquip.IsUbrin)
			PlayerEquip.itemToEquip.Add(equiped);
		
		Items.Add(equiped);
		return true;
	}
	
	public void EquipAll()
	{
		foreach(EquipedItem ei in Items)
		{
			EquipedItem newItem = new EquipedItem();
			newItem.rpgItem = ei.rpgItem;
			PlayerEquip.itemToEquip.Add(newItem);
		}
	}
	
	// Loading all items after loading game
	public void LoadItems()
	{
		foreach(EquipedItem item in Items)
		{
			if (item.UniqueItemId.StartsWith("WEAPON"))
			{
				int id = Convert.ToInt32(item.UniqueItemId.Replace("WEAPON", string.Empty));
				item.rpgItem = Storage.LoadById<RPGWeapon>(id, new RPGWeapon());
			}
			if (item.UniqueItemId.StartsWith("ARMOR"))
			{
				int id = Convert.ToInt32(item.UniqueItemId.Replace("ARMOR", string.Empty));
				item.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
			}
			item.rpgItem.Icon = (Texture2D)Resources.Load(item.rpgItem.IconPath, typeof(Texture2D)); 
		}
	}
	
	public void DamageItem(Player player)
	{
        if (!player.Hero.Settings.IsDurabilityOn)
		    return;
		
		//all items per hit
        if (player.Hero.Settings.DurabilityDamageDestroyAllArmor)
		{
			foreach(EquipedItem equiped in Items)
			{
				if (equiped.rpgItem.Preffix == PreffixType.WEAPON.ToString())
					continue;

                equiped.CurrentDurability -= player.Hero.Settings.DurabilityPerHitFromMonster;
				
				if (equiped.CurrentDurability < 0)
					equiped.CurrentDurability = 0;
				
				equiped.rpgItem.CurrentDurability = equiped.CurrentDurability;
			}
		}
		else
		{
			int itemCount = Items.Count;
			
			int itemIndex = UnityEngine.Random.Range(0, itemCount - 1);
			EquipedItem equiped = Items[itemIndex];
            equiped.CurrentDurability -= player.Hero.Settings.DurabilityPerHitFromMonster;
			if (equiped.CurrentDurability < 0)
					equiped.CurrentDurability = 0;
		}
	}
	
	public void DamageWeapon(Player player)
	{
		RPGWeapon weapon = GetCurrentWeapon;
		if (weapon == null || weapon.CurrentDurability == 0)
			return;
		
		foreach(EquipedItem item in Items)
		{
			if (item.rpgItem.Preffix == "WEAPON")
			{
                item.CurrentDurability -= player.Hero.Settings.DurabilityPerHit;
				
				if (item.CurrentDurability <= 0)
				{
					item.CurrentDurability = 0;
				}
				item.rpgItem.CurrentDurability = item.CurrentDurability;
			}
		}
		
	}
	
	//check if it is possible attack, it will check if weapon needs ammo 
	public bool IsPossibleToAttack(Player player)
	{
        RPGWeapon currentWeapon = GetCurrentWeapon;
        if (currentWeapon == null || currentWeapon.CurrentDurability == 0)
            return false;
        //if it is melee weapon or does not need ammo
        if (NeedAmmo == false || (NeedAmmo && player.Hero.Settings.UnilimitedAmmo))
            return true;


        foreach (EquipedItem item in Items)
        {
            if (item.rpgItem.Preffix == "WEAPON")
            {
                RPGWeapon weapon = (RPGWeapon)item.rpgItem;
                if (weapon.IsAmmo && weapon.WeaponType == currentWeapon.WeaponType)
                    return true;
            }
        }
        return false;
	}
	
	public RPGWeapon GetCurrentWeapon
	{
		get
		{
			RPGWeapon currentWeapon = new RPGWeapon();
			
			foreach(EquipedItem item in Items)
			{
				if (item.rpgItem.Preffix == "WEAPON")
				{
					currentWeapon = (RPGWeapon)item.rpgItem;
					if (currentWeapon.IsAmmo == false)
						return currentWeapon;
				}
			}
			return null;
		}
	}
	
	public RPGWeapon GetCurrentAmmo
	{
		get
		{
			RPGWeapon currentWeapon = new RPGWeapon();
			
			foreach(EquipedItem item in Items)
			{
				if (item.rpgItem.Preffix == "WEAPON")
				{
					currentWeapon = (RPGWeapon)item.rpgItem;
					if (currentWeapon.IsAmmo == true)
						return currentWeapon;
				}
			}
			return new RPGWeapon();
		}
	}
	
	public void RemoveOneAmmo(Player player)
	{
        if (player.Hero.Settings.UnilimitedAmmo)
			return;
		RPGWeapon weapon = GetCurrentAmmo;
		foreach(EquipedItem equiped in Items)
		{
			if (equiped.rpgItem.UniqueId != weapon.UniqueId)
				continue;
			equiped.CurrentAmount--; 
		}
		foreach(EquipedItem e in Items)
		{
			if (e.rpgItem.UniqueId == weapon.UniqueId && e.CurrentAmount == 0)
			{
				Items.Remove(e);
				break;
			}
		}
	}
}
