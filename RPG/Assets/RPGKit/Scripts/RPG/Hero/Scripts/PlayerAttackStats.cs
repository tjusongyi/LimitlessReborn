using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackStats 
{
	private int targetLevel;
	private RPGEnemy enemy;
	private WeaponCombatSkillType attackType;
	private CombatSkillType combatSkill;
	
	private int baseDamage;
	private int baseMaxDamage;

    private Player player;
	
	//simulated damage for character GUI
	public static int MinimumDamage;
	public static int MaximumDamage;
	public static int ChanceToHit;
		 
	public PlayerAttackStats(int TargetLevel, RPGEnemy Enemy, Player Player)
	{
		targetLevel = TargetLevel;
		enemy = Enemy;
        player = Player;
	}
	
	public static int CalculateDmg(int targetLevel, RPGEnemy target, Player player)
	{
		PlayerAttackStats playerAttack = new PlayerAttackStats(targetLevel, target, player);
		return playerAttack.CalculateDamage();
	}
	
	private void PrepareAttackType()
	{
		attackType = player.Hero.Equip.Weapon.Attack;
		combatSkill = CombatSkillType.Melee;
		if (attackType == WeaponCombatSkillType.Range)
		{
			combatSkill = CombatSkillType.Range;
		}
	}
	
	public static int ChanceToEvade(CombatSkillType attackType, Player player)
	{
		float chanceToEvade = 0;
		//add bonus from attribute
		foreach(RPGAttribute atr in player.Hero.Attributes)
			chanceToEvade += atr.AddAvoidance(attackType);
		//add bonus from skills
		foreach(RPGSkill skill in player.Hero.Skills)
			chanceToEvade += skill.AddAvoidance(attackType);
		return (int)chanceToEvade;
	}
	
	private void PrepareEnemyStats()
	{
		if (enemy.ID == 0 || enemy.UseDefaultValues)
		{
			if (enemy.Level == 0)
				targetLevel = player.Hero.CurrentLevel;
			enemy.Level = targetLevel;
			enemy.CalculateStats();
		}
	}
	
	private int CalculateDamage()
	{
		if (!player.Hero.Equip.IsPossibleToAttack(player))
			return 0;
		//determine attack type (range / melee / magic)
		PrepareAttackType();
		
		//calculate miss chance
		if (!IsSuccessfullHit())
			return 0;
		
		baseDamage = player.Hero.Equip.Weapon.MinimumDmg;
		baseMaxDamage = player.Hero.Equip.Weapon.MaximumDmg;
		
		//add atribute and skills
		baseDamage = AddAttributesAndSkills(baseDamage);
		baseMaxDamage = AddAttributesAndSkills(baseMaxDamage);
		
		//reduce damage by armor of enemy
		int damageReduction = (int)System.Math.Round(enemy.Armor * player.Hero.Settings.DamageReductionPerPoint);
		baseDamage -= damageReduction;
		if (baseDamage < 0)
			return 0;
		
		//attack can increase skill
		if (player.Hero.Settings.UsingIsIncreasingSkills)
		{
			foreach(RPGSkill skill in player.Hero.Skills)
				skill.IncreaseSkill(combatSkill, player);
		}
		
		int differences = baseMaxDamage - baseDamage;
		int random = BasicRandom.Instance.Next(differences);
		
		//TODO calculate critical chance
		
		
		return baseDamage + random;
	}
	
	private int AddAttributesAndSkills(int damage)
	{
		//calculate all bonus stats
		player.Hero.Bonuses.Calculate(player);
		
		//add bonus from attribute
		foreach(RPGAttribute atr in player.Hero.Bonuses.TotalAttributes)
			damage += atr.AddDamage(combatSkill);
		//add bonus from skills
		foreach(RPGSkill skill in player.Hero.Bonuses.TotalSkills)
			damage += skill.AddDamage(combatSkill);
		
		return damage;
	}
	
	public int SimulateMinimum()
	{
		PrepareAttackType();
		baseDamage = player.Hero.Equip.Weapon.MinimumDmg;
		return AddAttributesAndSkills(baseDamage);
	}
	
	public int SimulateMaximum()
	{
		PrepareAttackType();
		baseMaxDamage = player.Hero.Equip.Weapon.MaximumDmg;
		return AddAttributesAndSkills(baseMaxDamage);
	}
	
	/// <summary>
	/// Calculate if player hit enemy
	/// </summary>
	private bool IsSuccessfullHit()
	{
		//if is in settings
		if (player.Hero.Settings.PlayerAlwaysHit)
			return true;
		//chance to miss
		int chanceToMiss = 100 - CalculateChanceToHit();
		
		if (BasicRandom.Instance.Next(100) > chanceToMiss)
			return true;
		else
			return false;
	}
	
	public int CalculateChanceToHit()
	{
		//chance to miss
		int chanceToHit = player.Hero.Settings.PlayerHitChance;
		
		//calculate level differences
		chanceToHit += StaticCountChanceToHit(targetLevel);
		
		
		//calculate defense of the enemy
		if (enemy != null)
			chanceToHit = chanceToHit - enemy.ChanceToEvade;
		
		//gain bonus from attributes
		foreach(RPGAttribute atr in player.Hero.Attributes)
			chanceToHit += atr.ChanceToHit(combatSkill);
		
		//gain bonus from skills
		foreach(RPGSkill atr in player.Hero.Skills)
			chanceToHit += atr.ChanceToHit(combatSkill);
		
		return chanceToHit;
	}
	
	//counting chance to hit
	private int StaticCountChanceToHit(int targetLevel)
	{
		//we ignore levle differences in chance to hit
		if (player.Hero.Settings.IgnoreLevelDifferences)
			return 0;
		
		//calculate differences between levels
		int chance = player.Hero.CurrentLevel - targetLevel;
		//calculate how will change chane to hit  
		return chance * player.Hero.Settings.ChancePerLevel;
	}
	
	public static int CalculateSpellDamage(int targetLevel, RPGEnemy target, List<Effect> effects)
	{
		int result = 0;
		foreach(Effect effect in effects)
		{
			if (effect.Target == TargetType.Other)
			{
				result += effect.Value;
			}
		}
		return result * -1;
	}
}

public enum CombatSkillType
{
	None = 0,
	MeleeDefense = 1,
	RangeDefense = 2,
	MagicDefense = 3,
	Shield = 4,
	Melee = 5,
	Range = 6,
	Magic = 7
}


