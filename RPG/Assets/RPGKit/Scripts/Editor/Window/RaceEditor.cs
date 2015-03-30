using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RaceEditor : BaseEditorWindow 
{
	public RaceEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Race";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGCharacterRace> list = Storage.Load<RPGCharacterRace>(new RPGCharacterRace());
		items = new List<IItem>();
        foreach (RPGCharacterRace category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
        currentItem = new RPGCharacterRace();
	}

    public List<RPGCharacterRace> Races
	{
		get
		{
            List<RPGCharacterRace> list = new List<RPGCharacterRace>();
			foreach(IItem category in items)
			{
                list.Add((RPGCharacterRace)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
        Storage.Save<RPGCharacterRace>(Races, new RPGCharacterRace());
	}
	
	protected override void EditPart()
	{
        RPGCharacterRace s = (RPGCharacterRace)currentItem;
		
		EffectUtils.EffectsEditor(s.Effects, Data, EffectTypeUsage.Equiped);
		
		currentItem = s;
	}
}
