using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class RPGEnemy : BasicItem
{
	public RPGEnemy()
	{
        preffix = "ENEMY";
		Difficulty = DifficultyEnum.Normal;
		AttackSpeed = 3f;
		Container = new RPGContainer();
		PrefabPath = string.Empty;
		Container.IsEnemyContainer = true;
		Container.OnlyLoot = false;
	}
	
	public string PrefabPath;
	
	public DifficultyEnum Difficulty;
	public int Level;
	public bool UseDefaultValues;
	public RPGContainer Container;
	
	[XmlIgnore]
	public GameObject Prefab;
	
	public int ChanceToEvade;
	public int ChanceToHit;
	public int MaximumHp;
	public int Armor;
	public int MinimumDmg;
	public int MaximumDmg;
	public int Experience;
	public float AttackSpeed;
	
	
	public void LoadPrefab()
	{
        if (Prefab == null)
        {
            Prefab = (GameObject)Resources.Load(PrefabPath);
        }
	}
	
	public void CalculateStats()
	{
		if (Level == 0)
			return;
		
		GlobalSettings settings = new GlobalSettings();
		
		
		float difficulty = ((float)Difficulty) / (float)100;
		
		if (difficulty == 0)
			difficulty = 1;
		    
		MaximumHp = (int)(settings.EnemyHitPointPerLevel * Level * difficulty);
		ChanceToHit = (int)(settings.EnemyHitChance + (settings.EnemyChanceToHitPerLevel * Level) * difficulty);
		ChanceToEvade = (int)(settings.EnemyChanceToEvadePerLevel * Level * difficulty);
		Armor = (int)(settings.EnemyArmorPerLevel * Level * difficulty);
		MinimumDmg = (int)(settings.EnemyMinDmgPerLevel * Level * difficulty);
		MaximumDmg = (int)(settings.EnemyMaxDmgPerLevel * Level * difficulty);
		
		Experience = settings.EnemyFirstLevelExperience;
		for(int i = 1; i < Level; i++)
		{
			int increment = (int)((settings.EnemyExperiencePercentPerLevel/100) * settings.EnemyFirstLevelExperience);
			Experience += increment;
		}
		Experience = (int)(Experience * difficulty);
	}
}

public enum DifficultyEnum
{
	NotSelected = 0,
	VeryEasy = 80,
	Easy = 90,
	Normal = 100,
	Hard = 110,
	VeryHard = 120
}
