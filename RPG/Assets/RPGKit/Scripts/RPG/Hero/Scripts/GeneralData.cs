using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralData
{
	public List<RPGAttribute> Attributes;
	public List<RPGSkill> Skills;
	public List<RPGGuild> Guilds;
	public List<RPGCharacterClass> Classes;
	public List<RPGCharacterRace> Races;
	public List<RPGReputation> Reputations;
	public List<RPGItem> Items;
	public List<RPGWeapon> Weapons;
	public List<RPGArmor> Armors;
	public List<RPGSpell> Spells;
	public List<RPGEquipmentSlot> Slots;
	public List<RPGScene> Scenes;
	public List<RPGSpawnPoint> SpawnPoints;
	public List<RPGTeleport> Teleports;
    public List<RPGEnemy> Enemies;
	
	public GeneralData()
	{
		Attributes = Storage.Load<RPGAttribute>(new RPGAttribute());
		Skills = Storage.Load<RPGSkill>(new RPGSkill());
		Guilds = Storage.Load<RPGGuild>(new RPGGuild());
		Classes = Storage.Load<RPGCharacterClass>(new RPGCharacterClass());
		Races = Storage.Load<RPGCharacterRace>(new RPGCharacterRace());
		Reputations = Storage.Load<RPGReputation>(new RPGReputation());
		Items = Storage.Load<RPGItem>(new RPGItem());
		Weapons = Storage.Load<RPGWeapon>(new RPGWeapon());
		Armors = Storage.Load<RPGArmor>(new RPGArmor());
		Spells = Storage.Load<RPGSpell>(new RPGSpell());
		Slots = Storage.Load<RPGEquipmentSlot>(new RPGEquipmentSlot());
		Scenes = Storage.Load<RPGScene>(new RPGScene());
		SpawnPoints = Storage.Load<RPGSpawnPoint>(new RPGSpawnPoint());
		Teleports = Storage.Load<RPGTeleport>(new RPGTeleport());
        Enemies = Storage.Load<RPGEnemy>(new RPGEnemy());
		foreach(RPGSpell s in Spells)
		{
			s.LoadIcon();
		}
	}
	
	public RPGWeapon GetWeaponByID(int ID)
	{
		foreach(RPGWeapon s in Weapons)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
	
	public RPGArmor GetArmorByID(int ID)
	{
		foreach(RPGArmor s in Armors)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
	
	public RPGItem GetItemByID(int ID)
	{
		foreach(RPGItem s in Items)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
	
	public RPGSpell GetSpellByID(int ID)
	{
		foreach(RPGSpell s in Spells)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
	
	public RPGTeleport GetTeleportByID(int ID)
	{
		foreach(RPGTeleport s in Teleports)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
	
	public RPGSpawnPoint GetSpawnPointByID(int ID)
	{
		foreach(RPGSpawnPoint s in SpawnPoints)
		{
			if (s.ID == ID)
				return s;
		}
		return null;
	}
}
