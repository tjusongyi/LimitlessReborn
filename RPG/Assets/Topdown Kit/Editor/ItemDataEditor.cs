using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Item_Data))]
public class ItemDataEditor : Editor {
	
	private bool showEquip = true,showUsable = true,showEtc = true,showGold = true;
	
	Item_Data itemData;

	public override void OnInspectorGUI()
	{
		itemData = (Item_Data)target;
		
		//Usable Item
		showUsable = EditorGUILayout.Foldout(showUsable,"Usable Data");
		EditorGUI.indentLevel++;
		if(showUsable)
		{
			itemData.sizeUsable = EditorGUILayout.IntField("Useable Size",itemData.sizeUsable);
			
			while(itemData.sizeUsable != itemData.item_usable_set.Count)
			{
				if(itemData.sizeUsable > itemData.item_usable_set.Count)
				{
					itemData.item_usable_set.Add(new Item_Data.Item());
					itemData.showUsableSize.Add(true);
				}
				else
				{
					itemData.item_usable_set.RemoveAt(itemData.item_usable_set.Count-1);
					itemData.showUsableSize.RemoveAt(itemData.showUsableSize.Count-1);
				}
			}
			
			for(int i = 0;i<itemData.item_usable_set.Count;i++)
			{
				itemData.showUsableSize[i] = EditorGUILayout.Foldout(itemData.showUsableSize[i],itemData.item_usable_set[i].item_Name);
				
				if(itemData.showUsableSize[i])
				{
					EditorGUILayout.LabelField("Item ID",itemData.item_usable_set[i].item_ID.ToString());
					itemData.item_usable_set[i].item_ID = 1000 + (i+1);
					
					itemData.item_usable_set[i].item_Img = (Texture2D)EditorGUILayout.ObjectField("Item Icon",itemData.item_usable_set[i].item_Img,typeof(Texture2D),true);
					itemData.item_usable_set[i].item_Name = EditorGUILayout.TextField("Item Name",itemData.item_usable_set[i].item_Name);
					itemData.item_usable_set[i].item_Type = EditorGUILayout.TextField("Item Type",itemData.item_usable_set[i].item_Type);
					if(itemData.item_usable_set[i].item_Type == "" || itemData.item_usable_set[i].item_Type == "Null")
						itemData.item_usable_set[i].item_Type = "Potion";
					
					itemData.item_usable_set[i].price = EditorGUILayout.IntField("Price",itemData.item_usable_set[i].price);
					
					itemData.item_usable_set[i].potion = true;
					
					EditorGUILayout.LabelField("Add Attribute","");
					itemData.item_usable_set[i].hp = EditorGUILayout.IntField("HP",itemData.item_usable_set[i].hp);
					itemData.item_usable_set[i].mp = EditorGUILayout.IntField("MP",itemData.item_usable_set[i].mp);
					
					itemData.item_usable_set[i].item_Effect = (GameObject)EditorGUILayout.ObjectField("Item Effect",itemData.item_usable_set[i].item_Effect,typeof(GameObject),true);
					itemData.item_usable_set[i].item_Sfx = (AudioClip)EditorGUILayout.ObjectField("Item Sound Effect",itemData.item_usable_set[i].item_Sfx,typeof(AudioClip),true);
					
					GUIStyle style = new GUIStyle();
					if(!UnityEditorInternal.InternalEditorUtility.HasPro())
					style.normal.textColor = Color.black;
					else
					style.normal.textColor = Color.gray;
					EditorGUILayout.LabelField("Description", "");
					itemData.item_usable_set[i].description = EditorGUILayout.TextArea(itemData.item_usable_set[i].description);
					
					itemData.item_usable_set[i].gold = false;
				}
					
			}
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		
		//Equipment
		showEquip = EditorGUILayout.Foldout(showEquip,"Equipment Data");
		EditorGUI.indentLevel++;
		if(showEquip)
		{
			itemData.sizeEquip = EditorGUILayout.IntField("Equipment Size",itemData.sizeEquip);
			
			while(itemData.sizeEquip != itemData.item_equipment_set.Count)
			{
				if(itemData.sizeEquip > itemData.item_equipment_set.Count)
				{
					itemData.item_equipment_set.Add(new Item_Data.Item());
					itemData.showEquipSize.Add(true);
				}
				else
				{
					itemData.item_equipment_set.RemoveAt(itemData.item_equipment_set.Count-1);
					itemData.showEquipSize.RemoveAt(itemData.showEquipSize.Count-1);
				}
			}
			
			for(int i = 0;i<itemData.item_equipment_set.Count;i++)
			{
				itemData.showEquipSize[i] = EditorGUILayout.Foldout(itemData.showEquipSize[i],itemData.item_equipment_set[i].item_Name);
				
				if(itemData.showEquipSize[i])
				{
					EditorGUILayout.LabelField("Item ID",itemData.item_equipment_set[i].item_ID.ToString());
					itemData.item_equipment_set[i].item_ID = 2000 + (i+1);
					
					itemData.item_equipment_set[i].item_Img = (Texture2D)EditorGUILayout.ObjectField("Item Icon",itemData.item_equipment_set[i].item_Img,typeof(Texture2D),true);
					itemData.item_equipment_set[i].item_Name = EditorGUILayout.TextField("Item Name",itemData.item_equipment_set[i].item_Name);
					itemData.item_equipment_set[i].equipment_Type = (Item_Data.Equipment_Type)EditorGUILayout.EnumPopup("Equipment Type",itemData.item_equipment_set[i].equipment_Type);
					itemData.item_equipment_set[i].item_Type = EditorGUILayout.TextField("Item Type",itemData.item_equipment_set[i].item_Type);
					if(itemData.item_equipment_set[i].item_Type == "" || itemData.item_equipment_set[i].item_Type == "Null")
						itemData.item_equipment_set[i].item_Type = itemData.item_equipment_set[i].equipment_Type.ToString();
					
					itemData.item_equipment_set[i].require_Class = (ClassType)EditorGUILayout.EnumPopup("Require Class",itemData.item_equipment_set[i].require_Class);
					itemData.item_equipment_set[i].price = EditorGUILayout.IntField("Price",itemData.item_equipment_set[i].price);
					
					EditorGUILayout.LabelField("Add Attribute","");
					itemData.item_equipment_set[i].hp = EditorGUILayout.IntField("HP",itemData.item_equipment_set[i].hp);
					itemData.item_equipment_set[i].mp = EditorGUILayout.IntField("MP",itemData.item_equipment_set[i].mp);
					itemData.item_equipment_set[i].atk = EditorGUILayout.IntField("Attack",itemData.item_equipment_set[i].atk);
					itemData.item_equipment_set[i].def = EditorGUILayout.IntField("Defend",itemData.item_equipment_set[i].def);
					itemData.item_equipment_set[i].hit = EditorGUILayout.IntField("Hit",itemData.item_equipment_set[i].hit);
					itemData.item_equipment_set[i].spd = EditorGUILayout.IntField("Speed",itemData.item_equipment_set[i].spd);
					itemData.item_equipment_set[i].criPercent = EditorGUILayout.FloatField("Critical Rate",itemData.item_equipment_set[i].criPercent);
					itemData.item_equipment_set[i].atkSpd = EditorGUILayout.FloatField("Attack Speed",itemData.item_equipment_set[i].atkSpd);
					itemData.item_equipment_set[i].atkRange = EditorGUILayout.FloatField("Attack Range",itemData.item_equipment_set[i].atkRange);
					itemData.item_equipment_set[i].moveSpd = EditorGUILayout.FloatField("Move Speed",itemData.item_equipment_set[i].moveSpd);
					
					GUIStyle style = new GUIStyle();
					if(!UnityEditorInternal.InternalEditorUtility.HasPro())
					style.normal.textColor = Color.black;
					else
					style.normal.textColor = Color.gray;
					EditorGUILayout.LabelField("Description", "");
					itemData.item_equipment_set[i].description = EditorGUILayout.TextArea(itemData.item_equipment_set[i].description);
					
					itemData.item_equipment_set[i].gold = false;
				}
					
			}
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		
		//Etc
		showEtc = EditorGUILayout.Foldout(showEtc,"Etc Data");
		EditorGUI.indentLevel++;
		if(showEtc)
		{
			itemData.sizeEtc = EditorGUILayout.IntField("Etc Size",itemData.sizeEtc);
			
			while(itemData.sizeEtc != itemData.item_etc_set.Count)
			{
				if(itemData.sizeEtc > itemData.item_etc_set.Count)
				{
					itemData.item_etc_set.Add(new Item_Data.Item());
					itemData.showEtcSize.Add(true);
				}
				else
				{
					itemData.item_etc_set.RemoveAt(itemData.item_etc_set.Count-1);
					itemData.showEtcSize.RemoveAt(itemData.showEtcSize.Count-1);
				}
			}
			
			for(int i = 0;i<itemData.item_etc_set.Count;i++)
			{
				itemData.showEtcSize[i] = EditorGUILayout.Foldout(itemData.showEtcSize[i],itemData.item_etc_set[i].item_Name);
				
				if(itemData.showEtcSize[i])
				{
					EditorGUILayout.LabelField("Item ID",itemData.item_etc_set[i].item_ID.ToString());
					itemData.item_etc_set[i].item_ID = 3000 + (i+1);
					
					itemData.item_etc_set[i].item_Img = (Texture2D)EditorGUILayout.ObjectField("Item Icon",itemData.item_etc_set[i].item_Img,typeof(Texture2D),true);
					itemData.item_etc_set[i].item_Name = EditorGUILayout.TextField("Item Name",itemData.item_etc_set[i].item_Name);
					itemData.item_etc_set[i].item_Type = EditorGUILayout.TextField("Item Type",itemData.item_etc_set[i].item_Type);
					if(itemData.item_etc_set[i].item_Type == "" || itemData.item_etc_set[i].item_Type == "Null")
						itemData.item_etc_set[i].item_Type = "Etc";
					itemData.item_etc_set[i].price = EditorGUILayout.IntField("Price",itemData.item_etc_set[i].price);
					
					
					GUIStyle style = new GUIStyle();
					if(!UnityEditorInternal.InternalEditorUtility.HasPro())
					style.normal.textColor = Color.black;
					else
					style.normal.textColor = Color.gray;
					EditorGUILayout.LabelField("Description", "");
					itemData.item_etc_set[i].description = EditorGUILayout.TextArea(itemData.item_etc_set[i].description);
					
					itemData.item_etc_set[i].gold = false;
				}
					
			}
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		
		//Gold Setup
		showGold = EditorGUILayout.Foldout(showGold,"Gold Data");
		EditorGUI.indentLevel++;
		if(showGold)
		{
			EditorGUILayout.LabelField("Item ID",itemData.item_gold[0].item_ID.ToString());
			itemData.item_gold[0].item_ID = 4001;		
			itemData.item_gold[0].item_Img = (Texture2D)EditorGUILayout.ObjectField("Item Icon",itemData.item_gold[0].item_Img,typeof(Texture2D),true);
			itemData.item_gold[0].item_Name = EditorGUILayout.TextField("Item Name",itemData.item_gold[0].item_Name);
			itemData.item_gold[0].gold = true;
		}

		if(GUI.changed)
			EditorUtility.SetDirty(itemData);
	}
}
