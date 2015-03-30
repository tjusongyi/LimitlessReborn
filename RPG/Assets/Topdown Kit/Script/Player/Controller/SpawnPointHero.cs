/// <summary>
/// Spawn point hero.
/// This script use to spawn hero when first come to scene
/// </summary>

using UnityEngine;
using System.Collections;

public class SpawnPointHero : MonoBehaviour {
	
	public GameObject[] Hero;
	public static bool enableLoadData;
	
	private PlayerStatus playerStatus;
	private int indexHero;
	
	
	// Use this for initialization
	void Awake () {
		
		indexHero = PlayerPrefs.GetInt("pSelect",0);
		
		Hero[indexHero] = (GameObject)Instantiate(Hero[indexHero],transform.position,Quaternion.identity);
		if(enableLoadData)
		{
			LoadData();
			enableLoadData = false;
		}
	
	}
	
	void LoadData()
	{
		playerStatus = Hero[indexHero].GetComponent<PlayerStatus>();
		
		//Load name to character
		playerStatus.playerName = PlayerPrefs.GetString("pName");
		
	}
	
}
