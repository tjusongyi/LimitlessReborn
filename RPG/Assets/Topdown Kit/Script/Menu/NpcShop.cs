/// <summary>
/// Npc shop.
/// This script use to create a shop to sell item
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcShop : MonoBehaviour {
	
	public List<int> itemID = new List<int>();
	
	void Start()
	{
		if(this.gameObject.tag == "Untagged")
			this.gameObject.tag = "Npc_Shop";
	}
	
}


