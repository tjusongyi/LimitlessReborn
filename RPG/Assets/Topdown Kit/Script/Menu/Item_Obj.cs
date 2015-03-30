/// <summary>
/// Item_ object.
/// This script is use for display item when drop in to scene
/// </summary>


using UnityEngine;
using System.Collections;

public class Item_Obj : MonoBehaviour {
	
	public int item_id;
	public int item_amount;
	public Item_Data.Equipment_Type euipmentType;
	public string item_name;
	public GameObject item_img_obj;
	
	[HideInInspector]
	public bool gold;
	[HideInInspector]
	public int gold_price;
	
	void OnEnable(){
		SetItem();	
	}
	
	void OnCollisionEnter(Collision c)
	{
		this.rigidbody.isKinematic = true;
	}
	
	public void SetItem(){
		
		Item_Data.Item item = Item_Data.instance.Get_Item(item_id);
		if(item != null){
			gold = item.gold;
			euipmentType = item.equipment_Type;
			item_img_obj.renderer.material.mainTexture = item.item_Img;
			item_name = item.item_Name;
			
		}
	}
	
}
