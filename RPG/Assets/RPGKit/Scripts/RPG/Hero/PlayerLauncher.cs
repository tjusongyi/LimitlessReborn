using UnityEngine;
using System.Collections;

public class PlayerLauncher : MonoBehaviour 
{
	public bool IsWeather = false;
	public bool IsUbrinCharacter = false;
	
	
	// Use this for initialization
	void Start () 
	{
		Player p = gameObject.AddComponent<Player>();
		gameObject.AddComponent<PlayerAttack>();

		if (IsUbrinCharacter)
		{
			gameObject.AddComponent<PlayerEquip>();
			PlayerEquip.IsUbrin = IsUbrinCharacter;
		}

        PlayerController pc = gameObject.AddComponent<PlayerController>();
        pc.player = p;
		gameObject.AddComponent<MainMenuGUI>();
		gameObject.AddComponent<InventoryGUI>();
		gameObject.AddComponent<CharacterGUI>();
		gameObject.AddComponent<QuestLogGUI>();

		if (IsWeather)
			gameObject.AddComponent<Weather>();

		gameObject.AddComponent<QuantityGUI>();
		gameObject.AddComponent<ContainerGUI>();
		gameObject.AddComponent<HotBarGUI>();
		gameObject.AddComponent<NPCGUI>();
		gameObject.AddComponent<SpellbookGUI>();

		gameObject.AddComponent<AudioSource>();

		Destroy(this);
	}	
}
