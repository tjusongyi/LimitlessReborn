using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;


public class GatheringItemEditor : BaseEditorWindow
{
    public GatheringItemEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Gathering item";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
        List<RPGGatheringItem> list = Storage.Load<RPGGatheringItem>(new RPGGatheringItem());
		items = new List<IItem>();
        foreach (RPGGatheringItem category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
        currentItem = new RPGGatheringItem();
	}

    public List<RPGGatheringItem> GatheringItems
	{
		get
		{
            List<RPGGatheringItem> list = new List<RPGGatheringItem>();
			foreach(IItem category in items)
			{
                list.Add((RPGGatheringItem)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
        Storage.Save<RPGGatheringItem>(GatheringItems, new RPGGatheringItem());
	}
	
	protected override void EditPart()
	{
        RPGGatheringItem s = (RPGGatheringItem)currentItem;

        EditorUtils.Separator();

		if (GUILayout.Button("Add gathered product", GUILayout.Width(400)))
		{
            s.Products.Add(new GatheredProduct());
		}
		
		foreach(GatheredProduct gr in s.Products)
		{
            AddGatheredProduct(gr);
			
			if (GUILayout.Button("Delete", GUILayout.Width(100)))
			{
                s.Products.Remove(gr);
				break;
			}
			EditorUtils.Separator();
		}
		
		currentItem = s;
	}

    void AddGatheredProduct(GatheredProduct gr)
	{
		
		
	}
}
