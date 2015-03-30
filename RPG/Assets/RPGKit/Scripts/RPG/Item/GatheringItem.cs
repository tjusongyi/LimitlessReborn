using UnityEngine;
using System.Collections;

public class GatheringItem : BaseGameObject
{
    public int ID;

    [HideInInspector]
    public RPGGatheringItem harvestingItem;

    [HideInInspector]
    public RPGItem Item;

    private float currentTimer;
    private bool IsActivated;
    private int currentAmount = 0;

    private bool IsTooltip;
    private float tooltipTimer;

    private Player p;

    // Use this for initialization
    void Start()
    {
        harvestingItem = Storage.LoadById<RPGGatheringItem>(ID, new RPGGatheringItem());
        if (harvestingItem == null)
        {
            throw (new System.Exception("Harvesting item not defined or check ID!"));
        }
    }

    void Update()
    {
        if (IsActivated)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer > harvestingItem.Timer)
            {
                GetItem();        
            }
        }

        if (IsTooltip)
        {
            tooltipTimer += Time.deltaTime;

            if (tooltipTimer > 5)
            {
                IsTooltip = false;
            }
        }
    }

    private void GetItem()
    {
        IsActivated = false;
        Item = harvestingItem.GatherItem(p);
        p.ChangeMouseControl(true);
        currentAmount++;
        if (Item != null)
        {
            IsTooltip = true;
            tooltipTimer = 0;
        }

    }

    public override string DisplayCloseInfo(Player player)
    {
        return harvestingItem.Name + " - press E (gather " + harvestingItem.Name + ")";
    }

    public override string DisplayInfo(Player player)
    {
        return harvestingItem.Name;
    }

    public override void DoAction(Player player)
    {
        if (IsActivated || currentAmount >= harvestingItem.MaximumCount)
        {
            return;
        }

        if (harvestingItem.CanYouHarvest(player))
        {
            currentTimer = 0;
            p = player;
            IsActivated = true;
            p.ChangeMouseControl(false);
        }
        else
        { 
            //gui message - cannot harvest
        }
    }
}
