using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AllLog 
{
	public List<LogEvent> Events;
	
	public AllLog()
	{
		Events = new List<LogEvent>();
	}
	
	public void BuyItem(int NPCId, int Amount, string uniqueId)
	{
		LogEvent logEvent = new LogEvent();
		logEvent.Date = Weather.CurrentDateTime;
		logEvent.EventType = LogEventType.BuyItem;
		logEvent.Amount = Amount;
		logEvent.NPCId = NPCId;
		logEvent.UniqueId = uniqueId;
		Events.Add(logEvent);
	}
	
	public void KillEnemey(int EnemyID, string uniqueId)
	{
		LogEvent logEvent = new LogEvent();
		logEvent.Date = Weather.CurrentDateTime;
		logEvent.EventType = LogEventType.KilledCreature;
		logEvent.Id = EnemyID;
		logEvent.UniqueId = uniqueId;
		Events.Add(logEvent);
	}
	
	public bool IsTargetKilled(int EnemyID)
	{
		foreach(LogEvent log in Events)
		{
			if (log.EventType == LogEventType.KilledCreature && EnemyID == log.Id)
				return true;
		}
		return false;
	}
	
	public bool IsTargetKilled(int EnemyID, int AmountToReach)
	{
		int count = 0;
		
		foreach(LogEvent log in Events)
		{
			if (log.EventType == LogEventType.KilledCreature && EnemyID == log.Id)
				count++;
		}
		if (count >= AmountToReach)
			return true;
		else
			return false;
	}
}

public class LogEvent
{
	public DateTime Date;
	public LogEventType EventType;
	public int NPCId;
	public int Id;
	public string UniqueId;
	public PreffixType Preffix;
	//used only for buy item
	public int Amount;
}

public enum LogEventType
{
	KilledCreature = 0,
	QuestStarted = 1,
	QuestCompleted = 2,
	VisitArea = 3,
	BuyItem = 4,
    GainedItem = 5
}
