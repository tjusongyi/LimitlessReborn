using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class RPGSpell : UsableItem
{
	public int ManaCost;
	//mana cost can be changed lowered / increased

	public SpellTypeEnum Spelltype;
	public SpelLTargetType Target;
	public int SkillId;
	//required skill value
	public int SkillValue;
	public string PrefabName;
	public int Level;
	public float ProjectileSpeed;
	public int Price;
	
	[XmlIgnore]
	public GameObject Prefab;
	
	public RPGSpell()
	{
		PrefabName = string.Empty;
		ProjectileSpeed = 15;
		IsUsable = true;
		preffix = "SPELL";
		Target = SpelLTargetType.Character;
	}
	
	//casting spell = use item
	public override bool CheckRequirements (Player player)
	{
		//check mana cost 
		//TODO current mana in future mana cost can reduced
        if (player.Hero.CurrentMana < ManaCost)
			return false;

        if (!CheckCondition(player))
			return false;
		return true;
	}

	public void LoadPrefab()
	{
		if (Prefab == null)
			Prefab = (GameObject)Resources.Load(PrefabName);
	}
	
	public int ItemTooltipRows
	{
		get
		{
			int result = 0;
			
			result += UsableItemTooltipRows;
			//name, mana cost, price
			result += 3;
			
			if (SkillId != 0)
				result++;
		
			return result;
		}
	}
	
	public void Tooltip(GUISkin skin, Player player)
	{
		//first we will calculate position of the gui window box
		Vector3 mp = Input.mousePosition;
		
		float x = mp.x + 10;
		float y = Screen.height - mp.y;

        float height = ItemTooltipRows * player.Hero.Settings.TooltipRowSize + 25;
		
		if (height < 90)
			height = 90;
		
		if (mp.x + 380 > Screen.width)
			x = mp.x - 380;
		
		if (y + height + 10 > Screen.height)
			y = y - (height + 10);
		
		
		ItemToolTip(x, y, height, skin, player);
	}
	
	private void ItemToolTip(float x, float y, float height, GUISkin skin, Player player)
	{
		Rect rect = new Rect(x, y, 370, height);
		
		GUI.Box(rect,"", skin.box);
		
		rect = new Rect(x + 10, y+ 10,140, 20);
		GUI.Label(rect, Name, skin.label);
		
		rect = new Rect(x + 280, y +10,80,80);
		GUI.DrawTexture(rect, Icon);
		
		y += 10;
        y += player.Hero.Settings.TooltipRowSize;
		rect = new Rect(x + 10, y+ 10,140, 20);
		GUI.Label(rect, "Mana cost: " + ManaCost, skin.label);

        y += player.Hero.Settings.TooltipRowSize;
		rect = new Rect(x + 10, y+ 10,140, 20);
		
		foreach(RPGSkill skill in Player.Data.Skills)
		{
			if (SkillId == skill.ID)
			{
				GUI.Label(rect, "Required : " + SkillValue + " " + skill.Name, skin.label);
				break;
			}
		}

        y += player.Hero.Settings.TooltipRowSize;
		UsableItemTooltip(x, y, height, skin, this, player);
	}
}

public enum SpellTypeEnum
{
	Projectile = 0,
	SelfSpell = 1,
	TargetSpell = 2
}

public enum SpelLTargetType
{
	None = 0,
	Character = 1
}