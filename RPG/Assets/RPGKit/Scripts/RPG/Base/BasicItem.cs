using UnityEngine;
using System.Collections;

public class BasicItem : IItem
{

    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SystemDescription { get; set; }

    protected string preffix;

    public string Preffix 
    {
        get
        {
            return preffix;
        }
    }

    public string UniqueId
    {
        get
        {
            return Preffix + ID.ToString();
        }
    }
}
