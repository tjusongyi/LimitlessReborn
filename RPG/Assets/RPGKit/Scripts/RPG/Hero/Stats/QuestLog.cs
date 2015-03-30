using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestLog 
{
	public List<CompletedQuest> CompletedQuests;
	public List<RPGQuest> CurrentQuests;
	
	public QuestLog()
	{
		CompletedQuests = new List<CompletedQuest>();
		CurrentQuests = new List<RPGQuest>();
	}
	
	//quest step in progress
	public bool IsQuestStepInProgress(int questId, int questStepNumber)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsCurrentStep(questStepNumber))
				return true;
		}
		return false;
	}
	
	//returns true if is quest failed
	public bool IsQuestFailed(int questId)
	{
		foreach(CompletedQuest cq in CompletedQuests)
		{
			if (cq.ID == questId && cq.Failed)
				return true;
		}
		return false;
	}
	
	//returns true if quest is finished by alternate end
	public bool IsAlternateQuestCompleted(int questId, int endId)
	{
		foreach(CompletedQuest cq in CompletedQuests)
		{
			if (cq.ID == questId && cq.EndID == endId)
				return true;
		}
		return false;
	}
	
	// Returns true is quest is finished but not get reward from it (wasn't ended)
	public bool IsQuestFinished(int questId)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsFinished)
				return true;
		}
		return false;
	}
	
	// Is quest is completed (rewarded)
	public bool IsQuestCompleted(int questId)
	{
		foreach(CompletedQuest q in CompletedQuests)
		{
			if (q.ID == questId)
				return true;
		}
		return false;
	}
	
	public bool IsQuestInProgress(int questId)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsFinished == false)
				return true;
		}
		return false;
	}
	
	// Is quest in quest log (started or completed)
	public bool IsQuestStarted(int questId)
	{
		foreach(CompletedQuest q in CompletedQuests)
		{
			if (q.ID == questId)
				return true;
		}
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
				return true;
		}
		return false;
	}
	
	// Start new quest
	public bool StartQuest(int questId)
	{
		if (IsQuestStarted(questId))
			return false;
		        
		List<RPGQuest> quests = Storage.Load<RPGQuest>(new RPGQuest());
		bool result = false;	
		
		foreach(RPGQuest q in quests)
		{
			if (q.ID == questId)
			{
				CurrentQuests.Add(q);
				result = true;
			}
		}
		
		return result;
	}
	
	// End quest in quest log
	public bool EndQuest(int questId, Player player)
	{
		RPGQuest quest = new RPGQuest();
		
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				quest = q;
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		if (!result)
			return false;
        quest.GiveReward(player);
		
		if (!quest.Repeatable)
		{
			CompletedQuest cq = new CompletedQuest();
			cq.Name = quest.Name;
			cq.ID = quest.ID;
			cq.GuildID = quest.GuildID;
			cq.GuildRankID = quest.GuildRank;
			cq.Description = quest.FinalQuestLog;
			CompletedQuests.Add(cq);
		}
		return result;
	}
	
	public bool FailQuest(int questId)
	{
		RPGQuest quest = new RPGQuest();
		
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				quest = q;
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		if (!result)
			return false;
		
		if (!quest.Repeatable)
		{
			CompletedQuest cq = new CompletedQuest();
			cq.Name = quest.Name;
			cq.ID = quest.ID;
			cq.Failed = true;
			cq.GuildID = quest.GuildID;
			cq.GuildRankID = quest.GuildRank;
			cq.Description = quest.FinalQuestLog;
			CompletedQuests.Add(cq);
		}
		return result;
	}
	
	public bool AlternateEndQuest(int questId, int endId)
	{
		RPGQuest quest = new RPGQuest();
		
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				quest = q;
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		if (!result)
			return false;
		foreach(AlternateEnd end in quest.AlternateEnds)
		{
			if (end.ID == endId)
			{
				CompletedQuest cq = new CompletedQuest();
				cq.Name = quest.Name;
				cq.ID = quest.ID;
				cq.GuildID = quest.GuildID;
				cq.GuildRankID = quest.GuildRank;
				cq.Description = end.QuestLogEntry;
				cq.EndID = end.ID;
				CompletedQuests.Add(cq);
				return result;
			}
		}
		return false;
	}
	
	// Abandond quest
	public bool AbandonQuest(int questId)
	{
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		return result;
	}
	
	// Check paragraph if it is task of current quest
	public void CheckParagraph(int paragraphId)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckParagraph(paragraphId);
		
		UpdateQuests();
	}
	
	// Check line text if it is task of current quest
	public void CheckLineText(int lineTextId)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckLineText(lineTextId);
		
		UpdateQuests();
	}
	
	// Check if killed enemy is task of current quest
	public void KillEnemy(int enemyID)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckKilledEnemy(enemyID);
		
		UpdateQuests();
	}
	
	public void VisitArea(int worldObjectID)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.VisitArea(worldObjectID);
		
		UpdateQuests();
	}
	
	public void CheckInventoryItem(Player player)
	{
		foreach(RPGQuest q in CurrentQuests)
            q.CheckInventory(player);
		
		UpdateQuests();
	}
	
	public void UpdateQuests()
	{
		
	}
}
