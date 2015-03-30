using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlInclude(typeof(Effect))]
[XmlInclude(typeof(Condition))]
public class UsableItem : BasicItem
{
	public List<Effect> Effects;
	public List<Condition> UseConditions;
	public bool IsUsable;
	public UsageSkillType UsageSkill;
	public float Recharge; 
	public string IconPath;
	
	[XmlIgnore]
	public Texture2D Icon;
	
	
	public UsableItem()
	{
		Effects = new List<Effect>();
		UseConditions = new List<Condition>();
		IconPath = string.Empty;
	}
	
	public int UsableItemTooltipRows
	{
		get
		{
			int result = 0;
			
			if (IsUsable)
			{
				result += Effects.Count;
				result += UseConditions.Count;
				result+=2;
				
			}
			
			return result;
		}
	}
	
	protected virtual int TooltipTotalRows
	{
		get
		{
			return UsableItemTooltipRows;
		}
	}
	
	
	public void Tooltip(ItemInWorld item, GUISkin skin, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
		//first we will calculate position of the gui window box
		Vector3 mp = Input.mousePosition;
		
		float x = mp.x + 10;
		float y = Screen.height - mp.y;

        float height = (TooltipTotalRows + 2) * player.Hero.Settings.TooltipRowSize + 30;
		
		if (height < 110)
			height = 110;
		
		if (mp.x + 380 > Screen.width)
			x = mp.x - 380;
		
		if (y + height + 10 > Screen.height)
			y = y - (height + 10);


        ItemToolTip(x, y, height, skin, item, shop, isInventoryToolTip, priceModifier, player);
	}

    protected virtual void ItemToolTip(float x, float y, float height, GUISkin skin, ItemInWorld item, bool shop, bool isInventoryToolTip, float priceModifier, Player player)
	{
	}

    public void UsableHotBarTooltip(GUISkin skin, Player player)
	{
		//first we will calculate position of the gui window box
		Vector3 mp = Input.mousePosition;
		
		float x = mp.x + 10;
		float y = Screen.height - mp.y;

        float height = (TooltipTotalRows + 1) * player.Hero.Settings.TooltipRowSize + 35;
		
		if (height < 110)
			height = 110;
		
		if (mp.x + 380 > Screen.width)
			x = mp.x - 380;
		
		if (y + height + 10 > Screen.height)
			y = y - (height + 10);
		
		Rect rect = new Rect(x, y, 370, height);
		
		GUI.Box(rect,"", skin.box);
		
		rect = new Rect(x + 10, y+ 10,140, 20);
		string text = Name;
		GUI.Label(rect, text, skin.label);
		
		rect = new Rect(x + 280, y +10,80,80);
		GUI.DrawTexture(rect, Icon);
        y += player.Hero.Settings.TooltipRowSize + 10;

        UsableItemTooltip(x, y, height, skin, this, player);
	}
	
	protected float UsableItemTooltip(float x, float y, float height, GUISkin skin, UsableItem item, Player player)
	{
		if (!IsUsable || Effects.Count == 0)
			return y;
		
		string caption = "Effect";
		if (Effects.Count > 1)
			caption = "Use effects:";
        y = item.Effects[0].TooltipPart(x, y, skin, caption, item.Effects, player);
		
		if (item.UseConditions.Count > 0)
		{
            y = item.UseConditions[0].TooltipPart(x, y, skin, item.UseConditions, player);
		}
		
		return y;
	}
	
	public virtual bool UseItem()
	{
		return false;
	}

    public bool CheckCondition(Player player)
	{
		foreach(Condition condition in UseConditions)
		{
            if (!condition.Validate(player))
				return false;
			
		}
		return true;
	}

    public virtual bool CheckRequirements(Player player)
	{
        return CheckCondition(player);
	}
	
	public void LoadIcon()
	{
		if (Icon == null)
			Icon = (Texture2D)Resources.Load(IconPath, typeof(Texture2D));
	}
}

public enum UsageSkillType
{
	Combat = 0,
	Spell = 1,
	Item = 2
}

