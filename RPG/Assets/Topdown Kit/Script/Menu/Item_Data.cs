/// <summary>
/// Item_data.
/// This script use to create an item
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Data : MonoBehaviour {
	
	public enum Equipment_Type{
		Null = 0, Head_Gear = 1, Armor = 2, Shoes = 3, Accessory = 4, Left_Hand = 5, Right_Hand = 6, Two_Hand = 7 	
	}
	
	
	[System.Serializable]
	public class Item{
		public string item_Name = "Item Name";
		public string item_Type = "Item Type";
		[Multiline]
		public string description = "Description Here";
		public int item_ID;
		public Texture2D item_Img;
		public GameObject item_Effect;
		public AudioClip item_Sfx;
		
		public bool gold;
		public Equipment_Type equipment_Type;
		public ClassType require_Class;
		public bool potion;
		
		public int price;
		public int hp, mp, atk, def, spd, hit;
		public float criPercent, atkSpd, atkRange, moveSpd;
	}
	
	public List<Item> item_equipment_set = new List<Item>();
	public List<Item> item_usable_set = new List<Item>();
	public List<Item> item_etc_set = new List<Item>();
	public Item[] item_gold = new Item[1];
	
	public static Item_Data instance;
	
	//Editor Variable
	[HideInInspector]
	public int sizeEquip= 0;
	[HideInInspector]
	public List<bool> showEquipSize = new List<bool>();
	[HideInInspector]
	public int sizeUsable= 0;
	[HideInInspector]
	public List<bool> showUsableSize = new List<bool>();
	[HideInInspector]
	public int sizeEtc= 0;
	[HideInInspector]
	public List<bool> showEtcSize = new List<bool>();
	
	
	
	public void Start(){
		instance = this;	
	}
	
	public Item Get_Item(int item_id){
		int i = 0;
		bool isEnd = false;
		if(isEnd == false){
			while(i < item_equipment_set.Count){
				if(item_id == item_equipment_set[i].item_ID){
					return item_equipment_set[i];
					i = item_equipment_set.Count;
					isEnd = true;
				}
				i++;
			}
		}
		i = 0;
		if(isEnd == false){
			while(i < item_usable_set.Count){
				if(item_id == item_usable_set[i].item_ID){
					return item_usable_set[i];
					i = item_usable_set.Count;
					isEnd = true;
				}
				i++;
			}
		}
		i = 0;
		if(isEnd == false){
			while(i < item_etc_set.Count){
				if(item_id == item_etc_set[i].item_ID){
					return item_etc_set[i];
					i = item_etc_set.Count;
					isEnd = true;
				}
				i++;
			}
		}
		i = 0;
		if(isEnd == false){
			while(i < item_gold.Length){
				if(item_id == item_gold[i].item_ID){
					return item_gold[i];
					i = item_gold.Length;
					isEnd = true;
				}
				i++;
			}
		}
		
		
		return null;
	}
	
}
