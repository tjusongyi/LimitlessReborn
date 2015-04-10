using UnityEngine;
using System.Collections;

public class ItemEntity : BaseEntity
{

    public CommonDef.ItemType ItemType = CommonDef.ItemType.Default;
    public string Description { set; get; }
    public int OwnerId { set; get; }

    public Texture2D Icon;
 
    

}
