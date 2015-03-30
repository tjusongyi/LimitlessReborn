using UnityEngine;
using System;
using System.Collections;

public class NPC : BaseGameObject 
{
	public RPGNPC character;
    public int ID;

    void Start()
    {
		character = Storage.LoadById<RPGNPC>(ID, new RPGNPC());
    }

    public override string DisplayCloseInfo(Player player)
    {
        return character.Name + " - press E (talk to " + character.Name + ")";
    }

    public override string DisplayInfo(Player player)
    {
        return character.Name;
    }

    public override void DoAction(Player player)
    {
        BasicGUI.isConversationDisplayed = true;
        Player.activeNPC = this.ID;
    }
}
