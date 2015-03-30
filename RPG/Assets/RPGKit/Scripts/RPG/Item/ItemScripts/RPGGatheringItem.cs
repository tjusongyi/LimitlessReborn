using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RPGGatheringItem : BasicItem 
{
    public List<GatheredProduct> Products;
    public int MaximumCount = 3;
    public int MinimumCount = 3;
    public float Timer = 2;

    public List<Condition> Conditions;
    public List<ActionEvent> Events;

    public RPGGatheringItem()
    {
        preffix = "HARVITEM";
        Products = new List<GatheredProduct>();
        Conditions = new List<Condition>();
        Events = new List<ActionEvent>();
    }

    public bool CanYouHarvest(Player player)
    {
        foreach (Condition condition in Conditions)
        {
            if (!condition.Validate(player))
                return false;
        }
        return true;
    }

    public void DoEvents(Player player)
    {
        foreach (ActionEvent e in Events)
        {
            e.DoAction(player);
        }
    }

    public RPGItem GatherItem(Player player)
    {
        List<GatheredProduct> possibleProducts = new List<GatheredProduct>();

        //add to temp collection product possible to gather
        foreach (GatheredProduct p in Products)
        {
            if (p.CanYouGather(player))
                possibleProducts.Add(p);
        }

        foreach (GatheredProduct gp in possibleProducts)
        {
            int randomChance = Random.Range(0, 100);

            if (randomChance > gp.Chance)
            {
                RPGItem item = Player.Data.GetItemByID(gp.ItemID);

                int amount = Random.Range(gp.MinAmount, gp.MaxAmount);
                if (gp.MinAmount == gp.MaxAmount)
                    amount = gp.MinAmount;

                player.Hero.Inventory.AddItem(item, amount, player);
                return item;
            }
        }

        return null;
    }
}
