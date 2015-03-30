using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WorldObjectEditor : BaseEditorWindow 
{
	public WorldObjectEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "World object";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGWorldObject> list = Storage.Load<RPGWorldObject>(new RPGWorldObject());
		items = new List<IItem>();
		foreach(RPGWorldObject category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGWorldObject();
	}
	
	public List<RPGWorldObject> WorldObjects
	{
		get
		{
			List<RPGWorldObject> list = new List<RPGWorldObject>();
			foreach(IItem category in items)
			{
				list.Add((RPGWorldObject)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGWorldObject>(WorldObjects, new RPGWorldObject());
	}
	
	protected override void EditPart()
	{
		RPGWorldObject s = (RPGWorldObject)currentItem;

        s.OnlyOnce = EditorUtils.Toggle(s.OnlyOnce, "Only once");

        EditorUtils.Label("Conditions for activating this world object");

        ConditionsUtils.Conditions(s.Conditions, Data);

        EditorUtils.Label("Events = what will happen when you will visit or activate this world object");

        GUIUtils.EventsUtils(s.Events, Data);

        EditorUtils.Separator();

        s.IsActivated = EditorUtils.Toggle(s.IsActivated, "Is activated");

        EditorUtils.Separator();

        EditorUtils.Label("Effect area is trigger every one second");

        s.IsEffectArea = EditorUtils.Toggle(s.IsEffectArea, "Effect area");

        EditorUtils.Separator();

        EffectUtils.EffectsEditor(s.Effects, Data, EffectTypeUsage.EffectArea);

        EditorUtils.Label("Avoid conditions = if you want to avoid effect for this world object");

        ConditionsUtils.Conditions(s.AvoidConditions, Data);

		currentItem = s;
	}
}
