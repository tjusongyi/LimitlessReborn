using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TeleportEditor : BaseEditorWindow 
{

	public TeleportEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Teleport";
		
		Init(guiSkin, data);
	}
	
	protected override void LoadData()
	{
		List<RPGTeleport> list = Storage.Load<RPGTeleport>(new RPGTeleport());
		items = new List<IItem>();
		foreach(RPGTeleport category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGTeleport();
	}
	
	public List<RPGTeleport> Teleports
	{
		get
		{
			List<RPGTeleport> list = new List<RPGTeleport>();
			foreach(IItem category in items)
			{
				list.Add((RPGTeleport)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGTeleport>(Teleports, new RPGTeleport());
	}
	
	protected override void EditPart()
	{
		RPGTeleport s = (RPGTeleport)currentItem;

        s.OnlyArrive = EditorUtils.Toggle(s.OnlyArrive, "Only arrive");

        if (!s.OnlyArrive)
		{
			s.TargetTeleport = EditorUtils.IntPopup(s.TargetTeleport, Data.teleportEditor.items, "Target teleport");
		
			s.NeedActivateKey = EditorUtils.Toggle(s.NeedActivateKey, "Activate key", 50, FieldTypeEnum.WholeLine);
		
			s.MustTarget = EditorUtils.Toggle(s.MustTarget, "Must face", 50, FieldTypeEnum.WholeLine);
			
		}
		
		EditorUtils.Label("ID defined in build settings for current scene of this teleport");
		s.SceneId = EditorUtils.IntField(s.SceneId,"Scene build ID", 50, FieldTypeEnum.WholeLine);
		
		s.ArriveX = EditorUtils.FloatField(s.ArriveX, "Arrive X", 50, FieldTypeEnum.WholeLine);
		
		s.ArriveY = EditorUtils.FloatField(s.ArriveY, "Arrive Y", 50, FieldTypeEnum.WholeLine);
		
		s.ArriveZ = EditorUtils.FloatField(s.ArriveZ, "Arrive Z", 50, FieldTypeEnum.WholeLine);
		
		s.FixedRotation = EditorUtils.Toggle(s.FixedRotation, "Fixed rotation", 50, FieldTypeEnum.WholeLine);
		
		if (s.FixedRotation)
		{
			s.ArriveX = EditorUtils.FloatField(s.ArriveX, "Y rotation", 50, FieldTypeEnum.WholeLine);
		}
		
		ConditionsUtils.Conditions(s.Conditions, Data);
		
		currentItem = s;
	}
}
