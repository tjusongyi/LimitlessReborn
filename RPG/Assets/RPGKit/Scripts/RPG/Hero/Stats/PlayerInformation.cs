using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class PlayerInformation  {
	
	public List<RPGContainer> SharedContainers;
	public List<RPGAttribute> Attributes;
	public List<RPGSkill> Skills;
	public List<ActionEvent> ActionsToDo;
	public List<HeroReputation> Reputations;
	
	public ActiveBuffs Buffs;
	public BonusCollection Bonuses;
	public QuestLog Quest;
	public AllLog Log;
	public Inventory Inventory;
	public GlobalSettings Settings;
	public InputControls Controls;
	public ObjectPosition HeroPosition;
	public Equipment Equip;
	public GameTime gameTime;
	public HotBar Bar;
	public RPGCharacterRace characterRace;
	public PlayerSpellbook Spellbook;
	public PlayerGuild Guild;
	
	
	//current scene level - non multi terrain
	public int CurrentScene;
	
	//current scene level - multi terrain
	public string CurrentSceneName;
	
	//xp level
	public int CurrentLevel;
	
	public float BaseHp;
	public float TotalHp;
	public float BonusHp;
	public float CurrentHp;
	public float HpRegeneration = 0.0f;
	
	public float BaseMana;
	public float TotalMana;
	public float BonusMana;
	public float CurrentMana;
	public float ManaRegeneration = 0.5f;
	
	public int CurrentXP;
	public int CurrentLevelXP;
	
	public int AttributePoint;
	public int SkillPoint;
	
	public float AttackDelay;
	public float SpellDelay;
	public float UseItemDelay;
	
	public int RaceID;
	public int ClassID;
	
	[XmlIgnore]
	public GameObject playerGameObject;
	
	public PlayerInformation()
	{
		Buffs = new ActiveBuffs();
		Bonuses = new BonusCollection();
		ActionsToDo = new List<ActionEvent>();
		Attributes = new List<RPGAttribute>();
		Skills = new List<RPGSkill>();
		Quest = new QuestLog();
		Log = new AllLog();
		Inventory = new Inventory();
		Settings = new GlobalSettings();
		Controls = new InputControls();
		HeroPosition = new ObjectPosition();
		Equip = new Equipment();
		Spellbook = new PlayerSpellbook();
		gameTime = new GameTime();
		Bar = new HotBar();
        characterRace = new RPGCharacterRace();
		SharedContainers = new List<RPGContainer>();
		Guild = new PlayerGuild();
		Reputations = new List<HeroReputation>();
		
		TotalHp = BaseHp = CurrentHp = Settings.StartHitPoint;
		TotalMana = BaseMana = Settings.StartMana;
		CurrentHp = CurrentMana = 50;
		CurrentScene = -1;
	}
	
	public void StorePlayerPosition(Transform transform)
	{	
		
		HeroPosition = ObjectPosition.FromTransform(transform);
		
		//game time and day cycle
		gameTime.Store(RenderSettings.fogDensity);
	}
	
	public void UpdatePlayerInformation()
	{
		//load items in inventory
		foreach(InventoryItem item in Inventory.Items)
			item.LoadItem();
	
		//load equipment
		Equip.LoadItems();
		
		//game time and day cycle
		gameTime.FillGameObjects();
		RenderSettings.fogDensity = gameTime.FogIntensity;
	}

	public void LevelUp()
	{
		//you cannot have higher level than in settings
		if (CurrentLevel >= Settings.MaximumLevel)
			return;
		
		CurrentLevel++;
		CurrentXP = CurrentXP - CurrentLevelXP;
		//determine new amount for XP
		if (Settings.Leveling == LevelingSystem.XP && !Settings.AllLevelsSameXp)
		{
			int totalXp = (CurrentLevelXP * (100 + Settings.NextLevelXp));
			CurrentLevelXP = (int)(totalXp / 100);
		}
		
		//add level up bonuses
		BaseHp += Settings.HitPointPerLevel;
        if (Settings.IsNewLevelHealPlayer)
		    CurrentHp = BaseHp;
		AttributePoint += Settings.AttributePointPerLevel;
		SkillPoint += Settings.SkillPointPerLevel;
	}
	
	public void AddXp(int amount)
	{
		CurrentXP += amount;
		
		if (IsNewLevel)
			LevelUp();
	}
	
	public void AddXpPercent(int percentOfLevel)
	{
		int amountToGain = (int)((CurrentLevelXP /100) * (100 + percentOfLevel));
		CurrentXP += amountToGain;
		if (IsNewLevel)
			LevelUp();
	}
	
	public void ChangeHitPoint(int hitPointChange)
	{
		if (CurrentHp + hitPointChange > TotalHp)
			CurrentHp = TotalHp;
		else if (CurrentHp + hitPointChange < 0)
			CurrentHp = 0;
		else
			CurrentHp += hitPointChange;
	}
	
	public void CalculateDamage(Player player)
	{
		PlayerAttackStats pa = new PlayerAttackStats(1, null, player);
		PlayerAttackStats.MinimumDamage = pa.SimulateMinimum();
		PlayerAttackStats.MaximumDamage = pa.SimulateMaximum();
		PlayerAttackStats.ChanceToHit = pa.CalculateChanceToHit();
	}

    public void StartNewGame(Player player)
	{
		PlayerStart ps = new PlayerStart();
		ps.StartNewGame(player);
	}
	
	private bool IsNewLevel
	{
		get
		{
			return CurrentXP >= CurrentLevelXP;
		}
	}
	
	public void AddReputation(int reputationID, int amount)
	{
		RPGReputation rp = Storage.LoadById<RPGReputation>(reputationID, new RPGReputation());
		foreach(HeroReputation hr in Reputations)
		{
			if (hr.ReputationID == reputationID)
			{
				hr.Value += amount;
				rp = (RPGReputation)Storage.LoadById<RPGReputation>(reputationID, new RPGReputation());
				hr.Rank = rp.RankName(hr.Value);
				return;
			}
		}
		
		HeroReputation h = new HeroReputation();
		h.ReputationID = reputationID;
		h.Value = amount;
		h.Name = rp.Name;
		h.Rank = rp.RankName(amount);
		h.TextBefore = rp.TextBefore;
		
		Reputations.Add(h);
	}
	
	public bool IsReputationValue(int reputationId, int amount)
	{
		foreach(HeroReputation hr in Reputations)
		{
			if (reputationId == hr.ReputationID && hr.Value >= amount)
				return true;
		}
		
		return false;
	}
}
