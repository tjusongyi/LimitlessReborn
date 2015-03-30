using UnityEngine;
using System.Collections;

public class Teleport : BaseGameObject
{
	public int ID;
	private RPGTeleport teleport;
	
	public AudioSource source;

	
	// Use this for initialization
	void Start () 
	{
		if (ID == 0)
			return;
		
		teleport = Storage.LoadById<RPGTeleport>(ID, new RPGTeleport());
	}
	
    void UseTeleport(Player p)
    {
        ActionEvent ae = new ActionEvent();
        ae.Item = teleport.TargetTeleport;
        ae.ActionType = ActionEventType.UseTeleport;
        ae.DoAction(p);
    }
	
    public override string DisplayCloseInfo(Player player)
    {
        return "Press E go to " + teleport.Description;
    }

    public override string DisplayInfo(Player player)
    {
        return string.Empty;
    }

    public override void DoAction(Player player)
    {
        if (teleport.Validate(player))
        {
            UseTeleport(player);
        }
    }

    public override float GetActivateRange(Player player)
    {
        return player.Hero.Settings.ObjectActivateRange + 3;
    }
}
