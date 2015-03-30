using UnityEngine;
using System.Collections;

public class WorldObject : BaseGameObject 
{
    public RPGWorldObject worldObject;
	public int ID;
    public bool IsDisplayInfo;
	bool mouseOver;
    private float Timer;

	// Use this for initialization
	void Start () 
	{
		if (ID == 0)
			return;

        worldObject = Storage.LoadById<RPGWorldObject>(ID, new RPGWorldObject());
	}

	void OnTriggerStay(Collider other) 
	{
		GameObject go = other.gameObject;

        CollisionEvent(go);
	}
	
	void OnTriggerEnter(Collider other) 
	{
		GameObject go = other.gameObject;

        CollisionEvent(go);
	}
	
	private void CollisionEvent(GameObject go)
	{
		//be sure that collision is from game object
		if (go == null)
			return;
		Player player = (Player)go.GetComponent<Player>();
		//collision only for player
		if (player == null)
			return;
        Timer += Time.deltaTime;

        if (worldObject.IsActivated)
            return;

        if (worldObject.IsEffectArea && Timer < 1)
        {
            return;
        }

        Timer = 0;
        if (worldObject.IsEffect(player))
        {
            
            foreach (Effect e in worldObject.Effects)
            {
                player.Hero.Buffs.AddEffect(e);
            }
        }
		
        player.Hero.Quest.VisitArea(ID);

        if (worldObject.Validate(player))
        {
            worldObject.DoEvents(player);
        }

        if (worldObject.OnlyOnce)
        {
            Destroy(gameObject);
        }
	}

    public override void DoAction(Player player)
    {
        if (!worldObject.IsActivated)
            return;

        if (!worldObject.Validate(player))
        {
            return;
        }

        if (worldObject.IsEffect(player))
        {
            foreach (Effect e in worldObject.Effects)
            {
                player.Hero.Buffs.AddEffect(e);
            }
        }

        worldObject.DoEvents(player);

        if (worldObject.OnlyOnce)
        {
            Destroy(gameObject);
        }
    }

	
	void OnCollisionEnter(Collision collision)
	{
		GameObject go = collision.gameObject;
		CollisionEvent(go);
	}

    public override string DisplayCloseInfo(Player player)
    {
        if (IsDisplayInfo)
            return worldObject.Name;
        else
            return string.Empty;
    }

    public override string DisplayInfo(Player player)
    {
        if (IsDisplayInfo)
            return worldObject.Name;
        else
            return string.Empty;
    }

    public override float GetActivateRange(Player player)
    {
        return player.Hero.Settings.ObjectActivateRange + 1;
    }
}
