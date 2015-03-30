/// <summary>
/// Drop item.
/// This script use to control a enemy drop item
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropItem : MonoBehaviour {
	[System.Serializable]
	public class Drop{
		public int item_id;
		public float percentDrop;
	}
	public GameObject item_Obj_Pref;
	public int gold_min, gold_max;
	public List<Drop> item_Drop_List = new List<Drop>();
	
	public float powerImpulse = 1;

	
	public void UseDropItem(){
		
		
		for(int i = 0; i < item_Drop_List.Count; i++){
			float percentResult = Random.Range(0,100);
			if(percentResult < item_Drop_List[i].percentDrop){
				GameObject go = (GameObject)Instantiate(item_Obj_Pref,transform.position+(Vector3.up),Quaternion.identity);
				go.rigidbody.AddForce(new Vector3(Random.Range(-1,1)*1.5f*powerImpulse,1*3*powerImpulse,Random.Range(-1,1)*1.5f*powerImpulse),ForceMode.Impulse);
				Item_Obj item_obj = go.GetComponent<Item_Obj>();
				item_obj.item_id = item_Drop_List[i].item_id;
				item_obj.SetItem();
				if(item_obj.gold){
					item_obj.gold_price = Random.Range(gold_min, gold_max);
				}	
			}
		}
	}
	
	
}
