using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(Condition))]
public class RPGTeleport : IItem
{
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
	
	protected string preffix = "TELEPORT";
	public string Preffix
	{
		get
		{
			return preffix;
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
	
	public int TargetTeleport;
	public bool NeedActivateKey;
	public bool MustTarget;
	public float ArriveX;
	public float ArriveZ;
	public float ArriveY;
	public bool FixedRotation;
	public float YRotation;
	public int SceneId;
	public bool OnlyArrive;
	public List<Condition> Conditions;
	
	public RPGTeleport()
	{
		Conditions = new List<Condition>();
		YRotation = 0;
		ArriveX = 0;
		ArriveZ = 0;
		ArriveY = 0;
		SceneId = -1;
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
}
