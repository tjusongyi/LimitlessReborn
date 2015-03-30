using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(Reward))]
[XmlInclude(typeof(QuestStep))]
public class RPGQuest : BasicItem 
{
	public List<Reward> Rewards;
	public List<QuestStep> QuestSteps;
	public List<AlternateEnd> AlternateEnds;
	public bool Repeatable;
	public bool Failed;
	public int GuildID;
	public int GuildRank;
	public string FinalQuestLog;
    public int QuestCategoryID;
	
	public RPGQuest()
	{
		Rewards = new List<Reward>();
		QuestSteps = new List<QuestStep>();
		FinalQuestLog = string.Empty;
        preffix = "QUEST";
		AlternateEnds = new List<AlternateEnd>();
	}
	
	public QuestStep CurrentStep
	{
		get
		{
			foreach(QuestStep questStep in QuestSteps)
			{
				if (questStep.IsQuestStepFinished() == false)
					return questStep;
			}
			return null;
		}
	}
	
	public bool IsCurrentStep(int stepId)
	{
		QuestStep qs = CurrentStep;
		
		if (qs == null)
			return false;
		
		if (qs.StepNumber == stepId)
			return true;
		
		return false;
	}
	
	public bool IsFinished
	{
		get
		{
			bool result = true;
			foreach(QuestStep questStep in QuestSteps)
			{
				if (!questStep.IsQuestStepFinished())
					result = false;
			}
			return result;
		}
	}
	
	//paragraph task
	public void CheckParagraph(int paragraphID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.PARAGRAPH && task.TaskType == TaskTypeEnum.ReachPartOfConversation 
			    	&& task.TaskTarget == paragraphID)
				task.CurrentAmount = 1;
		}
	}
	
	//line text task
	public void CheckLineText(int lineTextID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.LINETEXT && task.TaskType == TaskTypeEnum.ReachPartOfConversation 
			    	&& task.TaskTarget == lineTextID)
				task.CurrentAmount = 1;
		}
	}
	
	//enemytask
	public void CheckKilledEnemy(int enemyID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.ENEMY && task.TaskType == TaskTypeEnum.KillEnemy
			    	&& task.TaskTarget == enemyID)
				task.CurrentAmount++;
		}
	}
	
	//bring item task
	public void CheckInventory(Player player)
	{
		QuestStep questStep = CurrentStep;
		
		if (questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.TaskType != TaskTypeEnum.BringItem)
				continue;
			if (task.PreffixTarget == PreffixType.ITEM || task.PreffixTarget == PreffixType.ARMOR || task.PreffixTarget == PreffixType.WEAPON)
			{
				task.CurrentAmount = player.Hero.Inventory.CountItem(task.PreffixTarget.ToString() + task.TaskTarget.ToString()); 
			}
		}
	}
	
	//visit area
	public void VisitArea(int WorldObjectID)
	{
		QuestStep questStep = CurrentStep;
		
		if (questStep == null)
			return;
		
		foreach(Task task in questStep.Tasks)
		{
			if (task.TaskType == TaskTypeEnum.VisitArea && task.TaskTarget == WorldObjectID)
			{
				task.CurrentAmount = 1; 
			}
		}
	}
	
	public void GiveReward(Player player)
	{
		foreach(Reward r in Rewards)
		{
			PreffixSolver.GiveItem(r.Preffix, r.ItemId, r.Amount, player);
		}
	}
	
	
	public void OrderAlternateEnd()
	{
		int index = 1;
		foreach(AlternateEnd end in AlternateEnds)
		{
			end.ID = index;
			index++;
		}
	}
}

