using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuGUI : BasicGUI 
{
	public static MainMenuLevel mainMenu;
	
	int indexX;
	
	int buttonCount = 0;
	Rect buttonRect;
	bool isSavedNewPosition = false;
	string fileName;
	bool state = true;
	
	List<SavePosition> savePositions;
	
	void Awake()
	{
		Prepare();
		savePositions = new SaveLoad().GetLoadPositions();
		fileName = string.Empty;
	}

    void Update()
    {
        if (IsOpenKey(KeyCode.Escape) && player.scene.SceneType != SceneTypeEnum.MainMenu)
        {
            isMainMenuDisplayed = true;
            player.ChangeMouseControl(false);
        }
    }

	void OnGUI()
	{
		if (player.scene.SceneType != SceneTypeEnum.MainMenu && BasicGUI.isMainMenuDisplayed == false)
			return;
		
		if (GlobalSettings.GameStyle == GameStyleType.ThirdPerson)
		{
			ChangeDisplayingCharacter(false);
		}
		
		Box(indexX, 30, WindowWidth,WindowHeight ,"Main menu");
		
		buttonCount = 2;
		indexX = (ScreenWidth / 2) - (WindowWidth/2);
		switch(mainMenu)
		{
			case MainMenuLevel.MainMenu:
				fileName = string.Empty;
				DisplayMainMenu();
				break;
			case MainMenuLevel.LoadGame:
				DisplayLoadGame();
				break;
			case MainMenuLevel.SaveGame:
				DisplaySaveGame();
				break;
			case MainMenuLevel.Option:
				DisplayOptions();
				break;
		}
		
		Label(new Rect(indexX+ 30, LeftTopY + WindowHeight - 40, 250,30),"Version 2");
	}
	
	void ChangeDisplayingCharacter(bool enabled)
	{
		if (player.scene.SceneType == SceneTypeEnum.MainMenu)
			return;
		
		if (state == enabled || player.Hero.CurrentScene == player.Hero.Settings.StartScene)
			return;
		
		if (renderer != null)
			renderer.enabled = enabled;
		
		Transform[] allChildren = this.GetComponentsInChildren<Transform>();
		foreach(Transform t in allChildren)
		{
			if (t.renderer != null)
			{
				t.renderer.enabled = enabled;
			}
		}
		state = enabled;
	}
	
	Rect ButtonMenu()
	{
		buttonCount++;
		return new Rect(indexX +40, LeftTopY + (45 * buttonCount) ,WindowWidth-80,35);
	}
	
	void DisplayOptions()
	{
		Label(ButtonMenu(),"Selected leveling system is " + player.Hero.Settings.Leveling.ToString());
		
		
		Label(ButtonMenu(), "Skills are increasing when using: " + player.Hero.Settings.UsingIsIncreasingSkills.ToString());
		
		
		if (Button(ButtonMenu(), "XP system"))
		{
			player.Hero.Settings.Leveling = LevelingSystem.XP;
		}
		
		
		if (Button(ButtonMenu(), "Skill system"))
		{
			player.Hero.Settings.Leveling = LevelingSystem.Skill;
		}
		
		if (Button(ButtonMenu(), "Skill is increasing"))
		{
			player.Hero.Settings.UsingIsIncreasingSkills = !player.Hero.Settings.UsingIsIncreasingSkills;
		}
		
		if (Button( ButtonMenu(), "No levels"))
		{
			player.Hero.Settings.Leveling = LevelingSystem.NoLevels;
		}
		
		
		if (Button(ButtonMenu(), "Back to main menu"))
		{
			mainMenu = MainMenuLevel.MainMenu;
		}
	}
	
	void DisplaySaveGame()
	{
		if (savePositions != null && savePositions.Count > 0)
		{
			foreach(SavePosition sp in savePositions)
			{
				Label(ButtonMenu(), sp.FileName);
			}
		}
		
		if (isSavedNewPosition)
		{
			buttonRect = new Rect(indexX + 40, LeftTopY + (45 * buttonCount) ,WindowWidth - 140,25);
			fileName = TextField(buttonRect, fileName);
			buttonRect = new Rect(indexX + WindowWidth - 90, LeftTopY + (45 * buttonCount) ,80,25);
			if (Button(buttonRect, "Cancel"))
			{
				isSavedNewPosition = false;
			}
		}
		else
		{
			if (Button(ButtonMenu(), "New save position"))
			{
				isSavedNewPosition = true;
			}
		}
		
		buttonCount++;
		buttonCount++;
		//save game
		if (Button(ButtonMenu(), "Save"))
		{
			if (string.IsNullOrEmpty(fileName))
				return;
			//class for save load
			SaveLoad sl = new SaveLoad();
			//store game content
			sl.Save(fileName, transform, player.GameObjects, player.Hero.CurrentScene, player);
			//refresh save positions
			savePositions = sl.GetLoadPositions();
			//back to the main menu
			mainMenu = MainMenuLevel.MainMenu;
		}
		
		buttonCount++;
		buttonCount++;
		if (Button(ButtonMenu(), "Cancel"))
		{
			mainMenu = MainMenuLevel.MainMenu;
		}
	}
	
	
	
	void DisplayLoadGame()
	{
		if (savePositions != null && savePositions.Count > 0)
		{
			foreach(SavePosition sp in savePositions)
			{
				if (Button(ButtonMenu(), sp.FileName))
				{
					LoadGame(sp.FileName);	
				}
			
			}
		}
		
		buttonCount++;
		buttonCount++;
		if (Button(ButtonMenu(), "Cancel"))
		{
			mainMenu = MainMenuLevel.MainMenu;
		}
	}
	
	void LoadGame(string fileName)
	{
		SaveLoad sl = new SaveLoad();
					
		//load information from save position
		sl.Load(fileName, player);
			
		//load scene only if it is different scene
		if (player.Hero.CurrentScene > -1)
		{
			if (Application.loadedLevel != player.Hero.CurrentScene)
				Application.LoadLevel(player.Hero.CurrentScene);
			
			//remove objects that dont exits in saved position
			foreach(GameObject go in sl.objectsToRemove)
				Destroy(go);
		}
		else
		{
			if (Application.loadedLevelName != player.Hero.CurrentSceneName)
				Application.LoadLevel(player.Hero.CurrentSceneName);
		}
		//set that world must be refreshed
		Player.isChangingScene = true;
		transform.position = player.Hero.HeroPosition.GetPosition();
		transform.rotation = player.Hero.HeroPosition.GetRotation();
		//update player information
		player.Hero.UpdatePlayerInformation();
		player.Hero.Equip.EquipAll(); 
		BasicGUI.isMainMenuDisplayed = false;
	}
	
	void StartNewGame()
	{
		if (GlobalSettings.GameStyle == GameStyleType.ThirdPerson)
			ChangeDisplayingCharacter(true);
		Application.LoadLevel(player.Hero.Settings.StartScene);
		transform.position = player.Hero.Settings.StartPosition;
		transform.rotation = player.Hero.Settings.StartRotation;
		player.Hero.StartNewGame(player);
	}
	
	void DisplayMainMenu()
	{
		if (player.scene.SceneType == SceneTypeEnum.MainMenu)
		{
			if (Button(ButtonMenu(), "Start new game"))
			{
				StartNewGame();
			}
			
			if (Button(ButtonMenu(), "Options"))
			{
				mainMenu = MainMenuLevel.Option;
			}
		}
		
		if (player.scene.SceneType != SceneTypeEnum.MainMenu)
		{
			if (Button(ButtonMenu(), "Return to the game"))
			{
				AudioListener.volume = 1;
				Time.timeScale = 1;
				BasicGUI.isMainMenuDisplayed = false;
                player.ChangeMouseControl(true);
			}
		}
		
		
		if (Button(ButtonMenu(), "Load game"))
		{
			//load menu
			mainMenu = MainMenuLevel.LoadGame;
		}
		
		if (player.scene.SceneType != SceneTypeEnum.MainMenu)
		{
			if (Button(ButtonMenu(), "Save game"))
			{
				//save menu
				mainMenu = MainMenuLevel.SaveGame;
			}
		}
		
		if (Button(ButtonMenu(), "Quit the game"))
		{
			Application.Quit();
		}
	}
}

public enum MainMenuLevel
{
	MainMenu = 0,
	LoadGame = 1,
	SaveGame = 2,
	Option = 3
}