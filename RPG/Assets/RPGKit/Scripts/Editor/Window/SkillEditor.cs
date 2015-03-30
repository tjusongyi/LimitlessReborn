using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class SkillEditor : BaseEditorWindow 
{
	public SkillEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Skill";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGSkill> list = Storage.Load<RPGSkill>(new RPGSkill());
		items = new List<IItem>();
		foreach(RPGSkill category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGSkill();
	}
	
	public List<RPGSkill> Skills
	{
		get
		{
			List<RPGSkill> list = new List<RPGSkill>();
			foreach(IItem category in items)
			{
				list.Add((RPGSkill)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGSkill>(Skills, new RPGSkill());
	}
	
	protected override void EditPart()
	{
		RPGSkill s = (RPGSkill)currentItem;
		
		s.IncreasePerUse = EditorUtils.FloatField(s.IncreasePerUse, "Increase per use");
		
		StatisticIncreaseUtils.DisplayForm(s, false);
		
		currentItem = s;
	}
}
