using UnityEngine;
using System.Collections;

public class Armor : BaseGameObject 
{
    public int ID;
	public RPGArmor rpgItem;
	
	// Use this for initialization
	void Start () 
	{
        rpgItem = Storage.LoadById<RPGArmor>(ID, new RPGArmor());
	}

    public override string DisplayCloseInfo(Player player)
    {
        return rpgItem.Name + " - press E (take " + rpgItem.Name + ")";
    }

    public override string DisplayInfo(Player player)
    {
        return rpgItem.Name;
    }

	public override void  DoAction(Player player)
    {
		if (string.IsNullOrEmpty(rpgItem.Name))
			return;
		
		//add item to inventory
        if (!player.Hero.Inventory.AddItem(rpgItem, player))
		{
			//display error GUI message = no space in inventory	
		}
		
		//destroy object
		Destroy(this.gameObject);
	}
}
