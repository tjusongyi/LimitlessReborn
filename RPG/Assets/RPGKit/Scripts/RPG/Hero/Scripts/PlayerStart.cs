using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStart
{
	public void StartNewGame(Player player)
	{
		BasicGUI.isMainMenuDisplayed = false;

        //starting player level
        player.Hero.CurrentLevel = 1;
		
		/*player.Hero.Attributes = Storage.Load<RPGAttribute>(new RPGAttribute());
		
		player.Hero.Skills = new List<RPGSkill>();
		
		List<RPGSkill> skills = Storage.Load<RPGSkill>(new RPGSkill());
		
		foreach(RPGSkill skill in skills)
			player.Hero.Skills.Add(skill);
		
		foreach(RPGAttribute atr in player.Hero.Attributes)
			atr.Value = atr.BaseValue;
		
		foreach(RPGSkill atr in player.Hero.Skills)
			atr.Value = atr.BaseValue;
		
		player.Hero.Bonuses.DefaultFill(player);
		
		
		
		//default attribute point
		player.Hero.AttributePoint = 0;
		
		//default skill point
		player.Hero.SkillPoint = 0;
		
		//character race
		if (player.Hero.Settings.UseCharacterRace)
		{
			player.Hero.RaceID = 1;
            player.Hero.characterRace = Storage.LoadById<RPGCharacterRace>(player.Hero.RaceID, new RPGCharacterRace());
		}
		
		//default game scene
		player.Hero.CurrentScene = player.Hero.Settings.StartScene;
		
		if (player.Hero.Settings.Leveling == LevelingSystem.XP)
			player.Hero.CurrentLevelXP = player.Hero.Settings.FirstLevelXp;
		else if (player.Hero.Settings.Leveling == LevelingSystem.Skill)
			player.Hero.CurrentLevelXP = player.Hero.Settings.SkillsToGainLevel;

        player.Hero.CalculateDamage(player);
		
		//adding some items to inventory
		PreffixSolver.GiveItem(PreffixType.ITEM, 1, 200, player);
		
		if (player.Hero.Settings.IsAllReputationLoaded)
		{
			List<RPGReputation> repuations = Storage.Load<RPGReputation>(new RPGReputation());
			
			foreach(RPGReputation r in repuations)
			{
				player.Hero.AddReputation(r.ID, 0);
			}
		}*/
	}
}
