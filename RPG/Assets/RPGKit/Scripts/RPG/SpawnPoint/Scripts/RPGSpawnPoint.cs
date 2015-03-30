using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


public class RPGSpawnPoint : BasicItem
{
	public bool IsOnlyEvent;
	public float Timer;
	public float MaxPlayerRange;
	public float MinPlayerRange;
	
	public int ChanceToSpawn;
	public List<Condition> SpawnConditions;
	public List<int> Enemies;
	
	
	public RPGSpawnPoint()
	{
		Timer = 140;
		ChanceToSpawn = 40;
		MaxPlayerRange = 100;
        preffix = "SPAWNPOINT";
		MinPlayerRange = 70;
		SpawnConditions = new List<Condition>();
		Enemies = new List<int>();
	}
	
	public void Prepare()
	{
		Timer = Timer + Random.Range(-30, 30);
	}
	
	public bool IsValidate(Player player)
	{
		foreach(Condition c in SpawnConditions)
		{
            if (!c.Validate(player))
			{
				return false;
			}
		}
		return true;
	}
	
	public bool IsSpawn(Player player)
	{
        if (!IsValidate(player))
            return false;

        int chance = 100 - ChanceToSpawn;
        int random = Random.Range(1, 100);
        if (random > chance)
            return true;

        return false;
	}
	
	public int SelectCreature
	{
		get
		{
			if (Enemies.Count == 0)
				return -1;
			
			if (Enemies.Count == 1)
				return Enemies[0];
			int random =  Random.Range(0, Enemies.Count-1);
			
			return Enemies[random];
		}
	}
}


