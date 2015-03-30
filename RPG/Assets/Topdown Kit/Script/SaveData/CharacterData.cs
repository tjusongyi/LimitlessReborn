/// <summary>
/// Character data.
/// This script use to record save data / load data
/// </summary>

using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {
	
	public static string characterName;
	public static string enableLoadData;
	public static PlayerStatus playerStatus;
	
	void Start()
	{
		//Auto load data when come to scene
		enableLoadData = PlayerPrefs.GetString("Enable Load");
		
		if(enableLoadData == "True" && LoadGameButton.loadData)
		{
			Invoke("LoadData",0.2f);
			enableLoadData = "False";
		}
	}
	
	
	//Save data method
	public static void SaveData()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");  //Find player
		playerStatus = go.GetComponent<PlayerStatus>();

		playerStatus.Save();
		GUI_Menu.instance.Save();
		PlayerPrefs.SetString("Enable Load","True");
		
	}
	
	//Load data method
	public void LoadData()
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");  //Find player
		playerStatus = go.GetComponent<PlayerStatus>();
		
		playerStatus.Load();
		GUI_Menu.instance.Load();
		playerStatus.UpdateAttribute();
		
	}
	
	
}
