/// <summary>
/// Game setting.
/// This script use to setting effect,mouse cursor,etc. in game
/// </summary>

using UnityEngine;
using System.Collections;

public class GameSetting : MonoBehaviour {
	
	public static GameSetting Instance; //this script
	
	public Texture2D cursorNormal; //cursor normal
	public Texture2D cursorAttack; //cursor attack
	public Texture2D cursorSkill; //cursor skill
	public Texture2D cursorPick; //cursor pick item
	public Texture2D cursorNpc; //cursor when over on npc
	public GameObject areaSkillCursor; //area skill cursor
	[HideInInspector]
	public GameObject areaSkillCursorObj;
	
	public GameObject mousefxNormal; //mouse effect when click to ground
	public GameObject mousefxAttack; //mouse effect when click to enemy
	public GameObject levelUpEffect; //level up effect
	public GameObject castEffect; //cast skill effect
	
	public float deadExpPenalty; //death penalty , when you character dead it will decrease exp%deadExpPenalty
	
	public float logTimer; //show log timer
	public string logSettingNoMP; //show "logSettingNoMP" when you cast skill and no mp to cast
	public string logSettingCantBuy; // show "logSettingCantBuy" when you no money to buy item
	
	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpot = Vector2.zero;
	
	// Use this for initialization
	void Awake () {
		
		Instance = this;

	}
	
	//Set mouse cursor
	public void SetMouseCursor(int cursorType)
	{
		if(cursorType == 0)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			
			Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
		}
		
		if(cursorType == 1)
		{
			Cursor.SetCursor(cursorAttack, hotSpot, cursorMode);
		}
		
		if(cursorType == 2)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			
			Cursor.SetCursor(cursorSkill, hotSpot, cursorMode);
		}
		
		if(cursorType == 3)
		{
			areaSkillCursorObj = (GameObject)Instantiate(areaSkillCursor,transform.position,Quaternion.identity);
		}
		
		if(cursorType == 4)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			Cursor.SetCursor(cursorNpc, hotSpot, cursorMode);
		}
		
		if(cursorType == 5)
		{
			if(areaSkillCursorObj != null)
				Destroy(areaSkillCursorObj);
			Cursor.SetCursor(cursorPick, hotSpot, cursorMode);
		}
		
	}
	
}
