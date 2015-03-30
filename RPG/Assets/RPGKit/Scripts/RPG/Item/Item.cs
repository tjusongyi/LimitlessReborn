using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : BaseGameObject 
{
    public int ID;
    public RPGItem rpgItem;
    public int Amount = 1;

    // Use this for initialization
    void Start()
    {
        rpgItem = Storage.LoadById<RPGItem>(ID, new RPGItem());
    }

    public override string DisplayCloseInfo(Player player)
    {
        return rpgItem.Name + " - press E (take " + rpgItem.Name + ")";
    }

    public override string DisplayInfo(Player player)
    {
        return rpgItem.Name;
    }

    public override void DoAction(Player player)
    {
        if (string.IsNullOrEmpty(rpgItem.Name))
            return;

        //add item to inventory
        if (!player.Hero.Inventory.AddItem(rpgItem, Amount, player))
        {
            //display error GUI message = no space in inventory	
        }

        //destroy object
        Destroy(this.gameObject);
    }
}
