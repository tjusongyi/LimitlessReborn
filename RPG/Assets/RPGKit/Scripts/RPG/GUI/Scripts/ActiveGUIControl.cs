using UnityEngine;
using System.Collections;

public class ActiveGUIControl
{
	public string Name { get; set;}
	public Rect ControlRect { get; set;}
	public Rect NormalRect { get; set;}
	
	public bool IsOutsideRect(Rect currentEventRect)
	{
		if (ControlRect.x > currentEventRect.x)
			return true;
		
		if (ControlRect.x + ControlRect.width < currentEventRect.x + currentEventRect.width)
			return true;
		
		if (ControlRect.y > currentEventRect.y)
			return true;
		
		if (ControlRect.y + ControlRect.height < currentEventRect.y + currentEventRect.height)
			return true;
		
		return false;
	}
	
	public bool IsMouseInsideRect(Vector3 mousePosition)
	{
		mousePosition.y = (Screen.height - mousePosition.y);
		
		if ((mousePosition.x < ControlRect.x) || (mousePosition.x > ControlRect.x + ControlRect.width))
			return false;
		
		if ((mousePosition.y < ControlRect.y) || (mousePosition.y > ControlRect.y + ControlRect.height))
			return false;
		
		return true;
	}
	
	public static ActiveGUIControl Create(Rect rect, string name)
	{
		ActiveGUIControl agc = new ActiveGUIControl();
		
		agc.ControlRect = rect;
		agc.Name = name;
		
		return agc;
	}
}
