using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class GUIUtils {

	public static Rect MainButton()
	{
		Rect buttonRect;
		buttonRect = EditorGUILayout.BeginHorizontal();
		buttonRect.x = 10;
		buttonRect.width = 200;
		buttonRect.height = 18;
		return buttonRect;
	}
	
	public static void AddBasicInformation(IItem item)
	{
		//bug inside GUI value cannot be NULL :-/
		if (string.IsNullOrEmpty(item.Name))
			item.Name = string.Empty;
		
		if (string.IsNullOrEmpty(item.Description))
			item.Description = string.Empty;
		
		if (string.IsNullOrEmpty(item.SystemDescription))
			item.SystemDescription = string.Empty;
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("ID");
			
		
		item.ID = EditorGUILayout.IntField(item.ID, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Name");
		item.Name = EditorGUILayout.TextField(item.Name, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Description");
		item.Description = EditorGUILayout.TextField(item.Description, GUILayout.Width(600));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("System description");
		item.SystemDescription = EditorGUILayout.TextField(item.SystemDescription, GUILayout.Width(600));
		EditorGUILayout.EndHorizontal();
	}
	
	public static void curveFromTo(Rect wr, Rect wr2, Color color, Color shadow)
    {
        Drawing.bezierLine(
            new Vector2(wr.x + wr.width, wr.y + 3 + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + 3 + wr.height / 2),
            new Vector2(wr2.x, wr2.y + 3 + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + 3 + wr2.height / 2), shadow, 5, false,20);
        Drawing.bezierLine(
            new Vector2(wr.x + wr.width, wr.y + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + wr.height / 2),
            new Vector2(wr2.x, wr2.y + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + wr2.height / 2), color, 2, false,20);
    }

    public static void EventsUtils(List<ActionEvent> Events, MainWindowEditor Data)
    {
        foreach (ActionEvent action in Events)
        {
            EventUtils.DisplayEvent(action, Data);

            if (GUILayout.Button("Remove", GUILayout.Width(150)))
            {
                Events.Remove(action);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add event", GUILayout.Width(90)))
        {
            Events.Add(new ActionEvent());
        }
        EditorGUILayout.EndHorizontal();
    }
	
	public static void ConditionsEvents(List<Condition> Conditions, List<ActionEvent> Events, MainWindowEditor Data)
	{
		ConditionsUtils.Conditions(Conditions, Data);

        EventsUtils(Events, Data);
	}
	
	public static int NewAttributeID<T>(List<T> items)
	{
		int maximum = 0;
		foreach(IItem p in items)
		{
			if (p.ID > maximum)
				maximum = p.ID;
		}
		maximum++; 
		return maximum;
	}
}
