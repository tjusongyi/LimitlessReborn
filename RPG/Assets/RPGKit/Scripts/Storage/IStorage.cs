using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public interface IStorage
{
	void SaveAll(IItem itemInformation, string content);
	List<T> Load<T>(IItem itemInformation);
}

public class Storage
{
	public static IStorage Get()
	{
		return new XMLStorage();
	}
	
	public static void SaveAll(IItem item, string content)
	{
		IStorage storage = Storage.Get();
		storage.SaveAll(item, content);
	}
	
	public static void Save<T>(List<T> items, IItem item)
	{
		XmlSerializer ser = new XmlSerializer(typeof(List<T>));
		StringWriter xml = new StringWriter();
		
		ser.Serialize(xml, items);
		string content = xml.ToString();
		Storage.SaveAll(item, content);
	}
	
	public static List<T> Load<T>(IItem item)
	{
		XMLStorage xmlStorage = new XMLStorage();
		return xmlStorage.Load<T>(item);
	}
	
	public static T LoadById<T>(int id, IItem item)
	{
		List<T> items = Storage.Load<T>(item);
		foreach(IItem i in items)
		{
			if (i.ID == id)
				return (T)i;
		}
		return default(T);
	}
	
	public static T LoadbyUniqueId<T>(string uniqueId, IItem item)
	{
		List<T> items = Storage.Load<T>(item);
		foreach(IItem i in items)
		{
			if (i.UniqueId == uniqueId)
				return (T)i;
		}
		return default(T);
	}
}