using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SpellEditor : BaseEditorWindow 
{
	
	public SpellEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Spell";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGSpell> list = Storage.Load<RPGSpell>(new RPGSpell());
		items = new List<IItem>();
		foreach(RPGSpell category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGSpell();
	}
	
	public List<RPGSpell> Spells
	{
		get
		{
			List<RPGSpell> list = new List<RPGSpell>();
			foreach(IItem category in items)
			{
				list.Add((RPGSpell)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGSpell>(Spells, new RPGSpell());
	}
	
	protected override void EditPart()
	{
		RPGSpell s = (RPGSpell)currentItem;
		
		EditorUtils.Label("If you want to create damage spell effect must have negative number e.g.: -15");
		EditorUtils.Label("if you will put only 15 it will heal your enemy");
		
		ItemUtils.AddUsableItem(s, Data, EffectTypeUsage.Spell);
		
		s.PrefabName = EditorUtils.TextField(s.PrefabName, "Prefab name");
		
		s.IconPath = EditorUtils.TextField(s.IconPath, "Icon name");
		
		s.Level = EditorUtils.IntField(s.Level, "Level");
		
		s.Price = EditorUtils.IntField(s.Price, "Price");
		
		s.ManaCost = EditorUtils.IntField(s.ManaCost, "Mana cost");
		
		s.ProjectileSpeed = EditorUtils.FloatField(s.ProjectileSpeed, "Speed");
		
		s.SkillId = EditorUtils.IntPopup(s.SkillId, Data.skillEditor.items, "Skill", 100, FieldTypeEnum.WholeLine);
		
		s.SkillValue = EditorUtils.IntField(s.SkillValue, "Skill value");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
			
		EditorGUILayout.PrefixLabel("Spell type");
		s.Spelltype = (SpellTypeEnum)EditorGUILayout.EnumPopup(s.Spelltype, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
			
		EditorGUILayout.PrefixLabel("Target type");
		s.Target = (SpelLTargetType)EditorGUILayout.EnumPopup(s.Target, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		
		currentItem = s;
	}
}
