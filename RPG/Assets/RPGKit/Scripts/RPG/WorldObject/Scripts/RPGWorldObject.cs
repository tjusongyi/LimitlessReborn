using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(Condition))]
public class RPGWorldObject : BasicItem
{
	public bool Static;
    public bool OnlyOnce;

	public List<Condition> Conditions;
	public List<ActionEvent> Events;

    public bool IsActivated;
    public List<ActionEvent> ActivateEvents;

    public bool IsEffectArea;
    public List<Effect> Effects;
    public List<Condition> AvoidConditions;
		
	public RPGWorldObject()
	{
		Conditions = new List<Condition>();
		Events = new List<ActionEvent>();
        ActivateEvents = new List<ActionEvent>();
        preffix = "WORLDOBJECT";
        Effects = new List<Effect>();
        AvoidConditions = new List<Condition>();
	}
	
	public bool Validate(Player player)
	{
		foreach(Condition condition in Conditions)
		{
            if (!condition.Validate(player))
				return false;
		}
		return true;
	}

    public void DoEvents(Player player)
	{
		foreach(ActionEvent ae in Events)
		{
            ae.DoAction(player);
		}
	}

    public void DoActivateEvents(Player player)
    {
        foreach (ActionEvent ae in ActivateEvents)
        {
            ae.DoAction(player);
        }
    }

    public bool IsEffect(Player player)
    {
        if (AvoidConditions.Count == 0)
            return true;

        bool result = false;
        foreach (Condition condition in AvoidConditions)
        {
            if (!condition.Validate(player))
                result = true;
        }
        return result;
    }
}
