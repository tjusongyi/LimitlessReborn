using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory 
{
    private List<ItemEntity> itemList;
    private List<EquipmentEntity> equipList;


    public int Size { set; get; }

	
    public void AddItem(ItemEntity item)
    {
        itemList.Add(item);
    }

    public void DeleteItem(ItemEntity item)
    {
        itemList.Remove(item);
    }

    public ItemEntity GetItem(string id)
    {
        return itemList.Find((item) => { return id == item.objectId; });
    }
}
