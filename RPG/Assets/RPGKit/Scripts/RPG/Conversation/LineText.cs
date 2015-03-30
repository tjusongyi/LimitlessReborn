using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlInclude(typeof(Condition))]
[XmlInclude(typeof(ActionEvent))]
[XmlInclude(typeof(LongLineText))]
public class LineText : IItem {

	public string Text;
	public int Order;
	
	public List<ActionEvent> GameEvents;
	public List<Condition> Conditions;
	public List<LongLineText> AdditionalTexts;
	
	public LineText()
	{
		Text = string.Empty;
		GameEvents = new List<ActionEvent>();
		Conditions = new List<Condition>();
		AdditionalTexts = new List<LongLineText>();
	}
		
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
	
	protected string preffix = "LINETEXT";
	public string Preffix
	{
		get
		{
			return description;
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
	
	public void DoEvents(Player player)
	{		
		if (GameEvents == null || GameEvents.Count == 0)
			return;
		
		foreach(ActionEvent action in GameEvents)
		{
            action.DoAction(player);
		}
	}
	
	public bool CanYouDisplay(Player player)
	{
		foreach(Condition condition in Conditions)
		{
            if (condition.Validate(player) == false)
				return false;
		}
		return true;
	}
	
	public int TooltipEventsCount
	{
		get
		{
			int result = 0;
			foreach(ActionEvent ae in GameEvents)
			{
				if (ae.ActionType == ActionEventType.GiveItem || ae.ActionType == ActionEventType.TakeItem || ae.ActionType == ActionEventType.UseTeleport)
					result++;
			}
			return result;
		}
	}
}
