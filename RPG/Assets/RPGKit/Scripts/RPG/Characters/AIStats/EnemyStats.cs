using UnityEngine;
using System.Collections;

public class EnemyStats 
{
	private int Level;
	private RPGEnemy enemy;
	private CombatSkillType attackType;
    private Player player;

    private EnemyStats(int level, RPGEnemy Enemy, CombatSkillType combatSkillType, Player Player)
	{
		Level = level;
		enemy = Enemy;
		attackType = combatSkillType;
        player = Player;
	}
	
	/// <summary>
	/// Calculate dmg done to player according level and enemy ID
	/// -1 = miss
	/// </summary>
	public static int CalculateDmg(int level, RPGEnemy enemy, CombatSkillType combatSkillType, Player player)
	{
		EnemyStats enemyStats = new EnemyStats(level, enemy, combatSkillType, player);

		return enemyStats.Calculate();
	}
	
	private void PrepareEnemyStats()
	{
		if (enemy.ID == 0 || enemy.UseDefaultValues)
		{
			enemy.Level = Level;
			enemy.CalculateStats();
		}
	}
	
	private int Calculate()
	{
		PrepareEnemyStats();
		//chance to hit
		if (!IsSuccessfullHit())
			return -1;
		
		int damage = enemy.MaximumDmg - enemy.MinimumDmg;
		
		//armor reduction
		damage = BasicRandom.Instance.Next(damage) + enemy.MinimumDmg;
        damage -= (int)System.Math.Round(player.Hero.Settings.DamageReductionPerPoint * player.Hero.Equip.ArmorClassValue);
		return damage;
	}
	
	private bool IsSuccessfullHit()
	{
        if (player.Hero.Settings.EnemyAlwaysHit)
			return true;
		
		int chanceToHit = enemy.ChanceToHit + StaticCountChanceToHit();
		
		if (BasicRandom.Instance.Next(100) > chanceToHit - PlayerAttackStats.ChanceToEvade(attackType, player))
			return true;
		else
			return false;
	}
	
	//counting chance to hit
	private int StaticCountChanceToHit()
	{
		//we ignore levle differences in chance to hit
		if (player.Hero.Settings.IgnoreLevelDifferences)
			return 0;
		
		//calculate differences between levels
        int chance = enemy.Level - player.Hero.CurrentLevel;
		//calculate how will change chane to hit  
        return chance * player.Hero.Settings.ChancePerLevel;
	}
}
