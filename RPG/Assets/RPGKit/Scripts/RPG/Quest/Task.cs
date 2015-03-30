using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class Task {
	
	[XmlAttribute (AttributeName = "TT")]
	public TaskTypeEnum TaskType;
	
	[XmlAttribute (AttributeName = "ATR")]
	public int AmountToReach;
	
	[XmlAttribute (AttributeName = "PT")]
	public PreffixType PreffixTarget;
	
	[XmlAttribute (AttributeName = "TASKTARGET")]
	public int TaskTarget;
	
	[XmlAttribute (AttributeName = "CA")]
	public int CurrentAmount;
	
	[XmlAttribute (AttributeName = "QLD")]
	public string QuestLogDescription;
	
	public Task()
	{
		TaskType = TaskTypeEnum.KillEnemy;
		QuestLogDescription = string.Empty;
		CurrentAmount = 0;
	}
	
	public bool IsTaskFinished()
	{
		switch(TaskType)
		{
			case TaskTypeEnum.KillEnemy:
				if(CurrentAmount >= AmountToReach)
					return true;
				break;
			case TaskTypeEnum.ReachPartOfConversation:
				if (CurrentAmount == 1)
					return true;
				break;
			case TaskTypeEnum.VisitArea:
				if (CurrentAmount == 1)
					return true;
			break;
		}
		return false;
	}
}

public enum TaskTypeEnum
{
	TargetEnemy = 0,
	BringItem = 1,
	KillEnemy = 2,
	ReachPartOfConversation = 3,
	VisitArea = 4
}