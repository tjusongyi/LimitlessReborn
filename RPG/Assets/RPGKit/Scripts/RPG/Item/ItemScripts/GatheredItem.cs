using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class GatheredProduct
{
    public int ItemID;
    public List<Condition> Conditions;
    public int Chance = 100;
    public int MinAmount = 1;
    public int MaxAmount = 1;

    public GatheredProduct()
    {
        Conditions = new List<Condition>();
    }

    public bool CanYouGather(Player player)
    {
        foreach (Condition condition in Conditions)
        {
            if (!condition.Validate(player))
                return false;
        }
        return true;
    }
}
