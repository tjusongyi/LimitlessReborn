using UnityEngine;
using System.Collections;

public class GlobalSettings 
{
    public static int StartPlayerHitPoint = 100;

    public static int DefaultBotWeapon = 2;

	// Maximum level in the game
	public int MaximumLevel = 100;
	public static int MaxLevel
	{
		get
		{
			GlobalSettings gs = new GlobalSettings();
			return gs.MaximumLevel;
		}
	}
	
	public GameStyleType Game = GameStyleType.FirstPerson;
	public static GameStyleType GameStyle
	{
		get
		{
			GlobalSettings gs = new GlobalSettings();
			return gs.Game;
		}
	}
	
	public static int CurrencyID = 1;
	public string CurrencyKeyword = "gold";
	
	//leveling system 
	public LevelingSystem Leveling = LevelingSystem.Skill;
	
	/* start leveling system */
	
	//how many xp do you need for gain first level
	public int FirstLevelXp = 1000;
	
	//all levels have same amount xp
	public bool AllLevelsSameXp = false;
	
	//how much percent will increase new level (for experience leveling system)
	public int NextLevelXp = 20;
	
	//how many skills gain is neccessary to increase level (Elder scroll style)
	public int SkillsToGainLevel = 2;
	
	//usage will increase skills
	public bool UsingIsIncreasingSkills = true;

    /* player start attributes*/
    public int StartHitPoint = 50;

    public int StartMana = 50;

    public float StartHitPointRegeneration = 0;

    public float StartManaPointRegeneration = 0.5f;
	
	/* level reward*/
	public int HitPointPerLevel = 10;
	
	public int AttributePointPerLevel = 3;
	
	public int SkillPointPerLevel = 0;
	
	public int ManaPointPerLevel = 5;

    public bool IsNewLevelHealPlayer = true;
	
	/* end leveling system */
	
	/* start minimap*/
	
	public bool UseMinimap = true;
	
	/* end minimap*/
	
	/* start character race */
	
	public bool UseCharacterRace = true;
	
	/* end character race */
	
	/* start durability item */
	
	//destroying item turned on/off
	public bool IsDurabilityOn = true;
	
	//how much will one point durability lose per one hit
	public float DurabilityPerHit = 2;
	
	//how much will one point durability per one hit from monster
	public float DurabilityPerHitFromMonster = 0.15f;
	
	//percentage price of item
	public float MaximumReparingPrice = 0.5f;
	
	//if enemy hit will damage all items
	public bool DurabilityDamageDestroyAllArmor = true;
		
	
	/* end durability item */
	
	/* start damage */
	
	//basic player damage at level one
	public int BasePlayerMinimumDamage = 1;
	
	//basic player damage at level one
	public int BasePlayerMaximumDamage = 2;
	
	//basic enemy damage at level one
	public int BaseEnemyDamage = 5;
	
	//allow melee critial hit
	public bool AllowMeleeCrit = true;
	
	//allow range critical hit
	public bool AllowRangeCrit = true;
	
	//allow magical crit
	public bool AllowMagicCrit = true;
	
	//basic melee critical value
	public int BaseMeleeCrit = 1;
	
	//basic range critical value
	public int BaseRangeCrit = 1;
	
	//basic magical critical value
	public int BaseMagicCrit = 1;
	
	//basic melee critical modifier (in percent)
	public int MeleeCritModifier = 50;
	
	//basic range critical modifier (in percent)
	public int RangeCritModifier = 50;
	
	//basic magic critical modifier (in percent)
	public int MagicCritModifier = 50;
	
	public int EnemyMinDmgPerLevel = 3;
	
	public int EnemyMaxDmgPerLevel = 4;
	
	public float EnemyExperiencePercentPerLevel = 20;
	
	public int EnemyFirstLevelExperience = 100;
	
	/* end damage */
	
	/* start armor*/
	
	//armor increase for enemy per level
	public int EnemyArmorPerLevel = 3;
	
	//how much will reduce one point of armor
	public float DamageReductionPerPoint = 1;
	
	/* end enemy armor*/
	
	/* start ranged comabt */
	
	//what is chance that projectile will stay on impact in percent
	public int ChanceArrowStay = 100;
	
	//if ranged combat needs ammo
	public bool UnilimitedAmmo = false;
	
	//basic projectile speed
	public float BasicProjectileSpeed = 15;
	
	//prefab location for default arrow
	public string DefaultArrow = "Items/Arrows/Arrow";
	
	public bool DeadBodyWithoutItemVanish = true; 
	
	/* end ranged combat */
	
	/* start magic system */
	
	//basic magic projectile speed
	public float BasicMagicProjectileSpeed = 18;
	
	/* end magic system */
	
	/* start hit chance */
	
	// Every player's melee hits are successfull
	//if this true it will override all other attributes 
	public bool PlayerAlwaysHit = false;
	
	// Every melee hit by enemy is successfull
	//if this true it will override player's chance to evade 
	public bool EnemyAlwaysHit = false;
	
	//if player is higher or lower level than creature
	//it will increase chance to hit to lower enemies or lower to higher enemies
	public bool IgnoreLevelDifferences = false;
	
	//how much will increase/decrease chance to hit for each level
	public int ChancePerLevel = 2;
	
	//base chance that player will hit enemy
	public int PlayerHitChance = 100;
	
	//base chance that enemy will hit player
	public int EnemyHitChance = 70;
	
	//enemy chance per level
	public int EnemyChanceToHitPerLevel = 2;
	
	//enemy chance to evade per level
	public int EnemyChanceToEvadePerLevel = 2;
	
	public int EnemyHitPointPerLevel = 10;
	
	/* end hit chance */
		
	/* start inventory */	
	
	//equiped items stay in inventory
	public bool EquipedItemInInventory = false;
	
	// Items are moved to the inventory automatically
	public bool AutomaticPickup = true;
	
	//allowing grabing object using "G" key
	public bool AllowGrabObject = true;

    //if is neccessary to have ammo equiped in hands
    public bool MustBeAmmoEquiped = false;
	
	/* end inventory */		
	
	//From range which can Objects be activated (pickup, activate NPC...)
	public int ObjectActivateRange = 2;

    //from range which can object display their info
    public int ObjectInfoRange = 3;
	
	//if all reputation are loaded in the beginning of the game
	public bool IsAllReputationLoaded = false;
	
	public int TooltipRowSize = 22;
	
	
	//start position for player in first scene
	public Vector3 StartPosition = new Vector3(20 ,3f,20f);
	public Quaternion StartRotation = new Quaternion(0,250,0,0);
	public int StartScene = 1;
	
	

		
	/* characters animation */
	public static string animWalk = "walk"; 
	public static string animRun = "run";
	public static string animAttack1 = "attack1";
	public static string animAttack2 = "attack2";
	public static string animHit1 = "hit1";
	public static string animHit2 = "hit2";
	public static string animIdle = "idle";
	public static string animDeath1 = "death1";
	public static string animDeath2 = "death2";
	
	//container GUI
	
	//hotbar
	//public static int HotbarWidth = 470;
	public static int HotbarHeight = 80;
	
	//shop settings
	//curreny ID (gold)
	public static int GoldID = 1;
	
	public bool IsShopConversation = true;
	
}

public enum LevelingSystem
{
	//no levels = Ultima online
	NoLevels = 0,
	//to gain new level you need xp
	XP = 1,
	//to gain new level you need increase some skills (Elder Scroll)
	Skill = 2
}

public enum GameStyleType
{
	ThirdPerson = 1,
	FirstPerson = 2
}