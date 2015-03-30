using UnityEngine;
using System;
using System.Collections;

public class QuestLogGUI : BasicGUI 
{

    QuestLogMenuStatus status;
	
	void Start()
	{
		Prepare();
        status = QuestLogMenuStatus.Current;
	}

	void OnGUI()
	{
		if (BasicGUI.isMainMenuDisplayed)
			return;
		
		if (BasicGUI.isQuestLogDisplayed)
			DisplayQuestLog();
	}

    void Update()
    {
        timeDelay += 0.01f;

        if (IsOpenKey(player.Hero.Controls.QuestLog))
        {
            ChangeQuestLogStatus();
        }
    }
	
	void DisplayQuestLog()
	{
		RightBox("Quest log");
		
		if (Button(new Rect (RightTopX + 50,LeftTopY + 40,120,25), "Active quests"))
		{
            status = QuestLogMenuStatus.Current;
		}
		
		if (Button(new Rect (RightTopX + 200,LeftTopY + 40,120,25), "Completed quests"))
		{
            status = QuestLogMenuStatus.Completed;
		}


        switch (status)
        { 
            case QuestLogMenuStatus.Completed:
                DisplayFinishedQuests();
                break;
            case QuestLogMenuStatus.Current:
                DisplayActiveQuests();
                break;
            
        }
	}
	
	void DisplayActiveQuests()
	{
		int windowY = LeftTopY + 95;
		int YrowIndex = 0;
		int questIndex = 1;
		Label(new Rect(RightTopX + 80, LeftTopY + 65,150,20), "Active quests");
		foreach(RPGQuest q in player.Hero.Quest.CurrentQuests)
		{
			string label = questIndex.ToString() + ". " + q.Name;
			if (q.IsFinished)
				label = questIndex + ". COMPLETED " + q.Name;
			
			Label(new Rect(RightTopX + 20, windowY + (YrowIndex * 30), 400,40), label);
			
			QuestStep questStep = q.CurrentStep;
			if (questStep == null)
			{
				questIndex++;
				YrowIndex++;
				continue;
			}
			foreach(Task task in questStep.Tasks)
			{
				YrowIndex++;
				DisplayTask(new Rect(RightTopX + 60, windowY + (YrowIndex * 30), 400,40), task);
			}
			questIndex++;
			YrowIndex++;
		}
	}
	
	private void DisplayTask(Rect position,Task task)
	{
		if (task.IsTaskFinished())
		{
			Label(position, "COMPLETED " + task.QuestLogDescription);
		}
		else
		{
			Label(position, task.QuestLogDescription);
		}
	}
	
	void DisplayFinishedQuests()
	{
		int questIndex = 1;
		int windowY = LeftTopY + 95;
		Label(new Rect(RightTopX + 80, LeftTopY + 65,150,20), "Completed quests");
		foreach(CompletedQuest q in player.Hero.Quest.CompletedQuests)
		{
			string label = questIndex.ToString() + ". " + q.Name;
			
			Label(new Rect(RightTopX + 40, windowY + ((questIndex-1) * 40), 400,20), label);
			Label(new Rect(RightTopX + 40, windowY + ((questIndex-1) * 40) + 20, 400,20), q.Description);
			questIndex++;
		}
	}
}

public enum QuestLogMenuStatus
{ 
    Current,
    Completed
}

