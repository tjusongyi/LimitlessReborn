using UnityEngine;
using System.Collections;

public class GeneralPlayerSounds
{
	public AudioClip goldCoin;
	public AudioClip skillUp;
	public AudioClip equipArmor;
	public AudioClip equipWeapon;
	public AudioClip repair;
	
	private string defaultDir = "Sounds/";
	
	public GeneralPlayerSounds()
	{
		goldCoin = (AudioClip)Resources.Load(defaultDir + "coin");
		skillUp = (AudioClip)Resources.Load(defaultDir + "SkillUp");
		equipArmor = (AudioClip)Resources.Load(defaultDir + "EquipItem");
		equipWeapon = (AudioClip)Resources.Load(defaultDir + "EquipWeapon");
		repair = (AudioClip)Resources.Load(defaultDir + "Repair");
		
		
	}
}
