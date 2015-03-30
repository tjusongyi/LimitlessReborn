using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class RPGNPC : IItem
{
	#region implementing interface IItem
	private int id;
	public int ID
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}
	
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
	
	private string description;
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}
	
	private string systemDescription;
	public string SystemDescription
	{
		get
		{
			return systemDescription;
		}
		set
		{
			systemDescription = value;
		}
	}
	
	protected string preffix = "NPC";
	public string Preffix
	{
		get
		{
			return preffix;
		}
	}
	
	public string UniqueId
	{
		get
		{
			return preffix + ID.ToString();
		}
	}
	#endregion
	
	public int ShopID;
	public List<Condition> ShopConditions;
	public int SpellShopID;
	public List<Condition> SpellShopConditions;
	public bool Repairing;
	public List<Condition> ReparingConditions;
	public float RepairPriceModifier;
	public int RepairCurrencyID;
	
	public List<Condition> TeleportConditions;
	public List<NPCTeleportTarget> Targets;
	
	public List<int> GeneralConversationID;
	
	public bool IsGuildMember;
	public int GuildID;
	public bool IsRecruit;
	public int AdvanceRankLevel;
	
	public RPGNPC()
	{
		ShopConditions = new List<Condition>();
		SpellShopConditions = new List<Condition>();
		ReparingConditions = new List<Condition>();
		TeleportConditions = new List<Condition>();
		RepairPriceModifier = 2;
		RepairCurrencyID = 1;
		Name = string.Empty;
		SystemDescription = string.Empty;
		Description = string.Empty;
		Targets = new List<NPCTeleportTarget>();
		TeleportConditions = new List<Condition>();
		GeneralConversationID = new List<int>();
	}
	
	public bool IsShop(Player player)
	{
		foreach(Condition c in ShopConditions)
		{
            if (!c.Validate(player))
			{
				return false;
			}
		}
		return true;
	}
	
	public bool IsSpellShop(Player player)
	{
        foreach (Condition c in SpellShopConditions)
        {
            if (!c.Validate(player))
            {
                return false;
            }
        }
        return true;
	}
	
    public bool IsRepair(Player player)
	{
        foreach (Condition c in ReparingConditions)
        {
            if (!c.Validate(player))
            {
                return false;
            }
        }
        return true;
	}

    public bool IsTeleport(Player player)
	{
		foreach(Condition c in TeleportConditions)
		{
            if (!c.Validate(player))
			{
				return false;
			}
		}
		return true;
	}
}


